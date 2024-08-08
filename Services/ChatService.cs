﻿

namespace Booger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using OpenAI;
    using OpenAI.Chat;

    public class ChatService
    {
        public ChatService(
            ChatStorageService chatStorageService,
            ConfigurationService configurationService)
        {
            ChatStorageService = chatStorageService;
            ConfigurationService = configurationService;
        }

        private OpenAIClient client;
        private string client_apikey;
        private string client_organization;
        private string client_apihost;

        public ChatStorageService ChatStorageService { get; }

        public ConfigurationService ConfigurationService { get; }

        private void NewOpenAIClient(
            [NotNull] out OpenAIClient client, 
            [NotNull] out string client_apikey,
            [NotNull] out string client_apihost,
            [NotNull] out string client_organization)
        {
            client_apikey = ConfigurationService.Configuration.ApiKey;
            client_apihost = ConfigurationService.Configuration.ApiHost;
            client_organization = ConfigurationService.Configuration.Organization;

            client = new OpenAIClient(
                new OpenAIAuthentication(client_apikey, client_organization),
                new OpenAIClientSettings(client_apihost));
        }

        private OpenAIClient GetOpenAIClient()
        {
            if (client == null ||
                client_apikey != ConfigurationService.Configuration.ApiKey ||
                client_apihost != ConfigurationService.Configuration.ApiHost ||
                client_organization != ConfigurationService.Configuration.Organization)
                NewOpenAIClient(out client, out client_apikey, out client_apihost, out client_organization);

            return client;
        }

        CancellationTokenSource cancellation;

        public ChatSession NewSession(string name)
        {
            ChatSession session = ChatSession.Create(name);
            ChatStorageService.SaveSession(session);

            return session;
        }

        public Task<ChatDialogue> ChatAsync(Guid sessionId, string message, Action<string> messageHandler)
        {
            cancellation?.Cancel();
            cancellation = new CancellationTokenSource();

            return ChatCoreAsync(sessionId, message, messageHandler, cancellation.Token);
        }

        public Task<ChatDialogue> ChatAsync(Guid sessionId, string message, Action<string> messageHandler, CancellationToken token)
        {
            cancellation?.Cancel();
            cancellation = CancellationTokenSource.CreateLinkedTokenSource(token);

            return ChatCoreAsync(sessionId, message, messageHandler, cancellation.Token);
        }

        public Task<string> GetTitleAsync(Guid sessionId, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public void Cancel()
        {
            cancellation?.Cancel();
        }

        private async Task<ChatDialogue> ChatCoreAsync(Guid sessionId, string message, Action<string> messageHandler, CancellationToken token)
        {
            ChatSession session = 
                ChatStorageService.GetSession(sessionId);

            ChatMessage ask = ChatMessage.Create(sessionId, "user", message);

            OpenAIClient client = GetOpenAIClient();

            List<Message> messages = new List<Message>();

            foreach (var sysmsg in ConfigurationService.Configuration.SystemMessages)
                messages.Add(new Message(Role.System, sysmsg));

            if (session != null)
                foreach (var sysmsg in session.SystemMessages)
                    messages.Add(new Message(Role.System, sysmsg));

            if (session?.EnableChatContext ?? ConfigurationService.Configuration.EnableChatContext)
                foreach (var chatmsg in ChatStorageService.GetAllMessages(sessionId))
                    messages.Add(new Message(Enum.Parse<Role>(chatmsg.Role, true), chatmsg.Content));

            messages.Add(new Message(Role.User, message));

            string modelName =
                ConfigurationService.Configuration.Model;
            double temperature =
                ConfigurationService.Configuration.Temerature;

            DateTime lastTime = DateTime.Now;

            StringBuilder sb = new StringBuilder();

            CancellationTokenSource completionTaskCancellation =
                CancellationTokenSource.CreateLinkedTokenSource(token);

            Task completionTask = client.ChatEndpoint.StreamCompletionAsync(
                new ChatRequest(messages, modelName, temperature),
                response =>
                {
                    string content = response.Choices.FirstOrDefault()?.Delta?.Content;
                    if (!string.IsNullOrEmpty(content))
                    {
                        sb.Append(content);

                        while (sb.Length > 0 && char.IsWhiteSpace(sb[0]))
                            sb.Remove(0, 1);

                        messageHandler.Invoke(sb.ToString());

                        // 有响应了, 更新时间
                        lastTime = DateTime.Now;
                    }
                }, completionTaskCancellation.Token);

            Task cancelTask = Task.Run(async () =>
            {
                try
                {
                    TimeSpan timeout = 
                        TimeSpan.FromMilliseconds(ConfigurationService.Configuration.ApiTimeout);

                    while (!completionTask.IsCompleted)
                    {
                        await Task.Delay(100);

                        // 如果当前时间与上次响应的时间相差超过配置的超时时间, 则扔异常
                        if ((DateTime.Now - lastTime) > timeout)
                        {
                            completionTaskCancellation.Cancel();
                            throw new TimeoutException();
                        }
                    }
                }
                catch (TaskCanceledException)
                {
                    return;
                }
            });

            await Task.WhenAll(completionTask, cancelTask);

            ChatMessage answer = ChatMessage.Create(sessionId, "assistant", sb.ToString());
            ChatDialogue dialogue = new ChatDialogue(ask, answer);

            ChatStorageService.SaveMessage(ask);
            ChatStorageService.SaveMessage(answer);

            return dialogue;
        }
    }
}
