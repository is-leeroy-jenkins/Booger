

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

        private OpenAIClient _client;
        private string _clientApikey;
        private string _clientOrganization;
        private string _clientApihost;

        public ChatStorageService ChatStorageService { get; }

        public ConfigurationService ConfigurationService { get; }

        private void NewOpenAiClient(
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

        private OpenAIClient GetOpenAiClient()
        {
            if (_client == null ||
                _clientApikey != ConfigurationService.Configuration.ApiKey ||
                _clientApihost != ConfigurationService.Configuration.ApiHost ||
                _clientOrganization != ConfigurationService.Configuration.Organization)
                NewOpenAiClient(out _client, out _clientApikey, out _clientApihost, out _clientOrganization);

            return _client;
        }

        CancellationTokenSource _cancellation;

        public ChatSession NewSession(string name)
        {
            ChatSession _session = ChatSession.Create(name);
            ChatStorageService.SaveSession(_session);

            return _session;
        }

        public Task<ChatDialogue> ChatAsync(Guid sessionId, string message, Action<string> messageHandler)
        {
            _cancellation?.Cancel();
            _cancellation = new CancellationTokenSource();

            return ChatCoreAsync(sessionId, message, messageHandler, _cancellation.Token);
        }

        public Task<ChatDialogue> ChatAsync(Guid sessionId, string message, Action<string> messageHandler, CancellationToken token)
        {
            _cancellation?.Cancel();
            _cancellation = CancellationTokenSource.CreateLinkedTokenSource(token);

            return ChatCoreAsync(sessionId, message, messageHandler, _cancellation.Token);
        }

        public Task<string> GetTitleAsync(Guid sessionId, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public void Cancel()
        {
            _cancellation?.Cancel();
        }

        private async Task<ChatDialogue> ChatCoreAsync(Guid sessionId, string message, Action<string> messageHandler, CancellationToken token)
        {
            ChatSession _session = 
                ChatStorageService.GetSession(sessionId);

            ChatMessage _ask = ChatMessage.Create(sessionId, "user", message);

            OpenAIClient _client = GetOpenAiClient();

            List<Message> _messages = new List<Message>();

            foreach (var _sysmsg in ConfigurationService.Configuration.SystemMessages)
                _messages.Add(new Message(Role.System, _sysmsg));

            if (_session != null)
                foreach (var _sysmsg in _session.SystemMessages)
                    _messages.Add(new Message(Role.System, _sysmsg));

            if (_session?.EnableChatContext ?? ConfigurationService.Configuration.EnableChatContext)
                foreach (var _chatmsg in ChatStorageService.GetAllMessages(sessionId))
                    _messages.Add(new Message(Enum.Parse<Role>(_chatmsg.Role, true), _chatmsg.Content));

            _messages.Add(new Message(Role.User, message));

            string _modelName =
                ConfigurationService.Configuration.Model;
            double _temperature =
                ConfigurationService.Configuration.Temerature;

            DateTime _lastTime = DateTime.Now;

            StringBuilder _sb = new StringBuilder();

            CancellationTokenSource _completionTaskCancellation =
                CancellationTokenSource.CreateLinkedTokenSource(token);

            Task _completionTask = _client.ChatEndpoint.StreamCompletionAsync(
                new ChatRequest(_messages, _modelName, _temperature),
                response =>
                {
                    string _content = response.Choices.FirstOrDefault()?.Delta?.Content;
                    if (!string.IsNullOrEmpty(_content))
                    {
                        _sb.Append(_content);

                        while (_sb.Length > 0 && char.IsWhiteSpace(_sb[0]))
                            _sb.Remove(0, 1);

                        messageHandler.Invoke(_sb.ToString());

                        // 有响应了, 更新时间
                        _lastTime = DateTime.Now;
                    }
                }, _completionTaskCancellation.Token);

            Task _cancelTask = Task.Run(async () =>
            {
                try
                {
                    TimeSpan _timeout = 
                        TimeSpan.FromMilliseconds(ConfigurationService.Configuration.ApiTimeout);

                    while (!_completionTask.IsCompleted)
                    {
                        await Task.Delay(100);

                        // 如果当前时间与上次响应的时间相差超过配置的超时时间, 则扔异常
                        if ((DateTime.Now - _lastTime) > _timeout)
                        {
                            _completionTaskCancellation.Cancel();
                            throw new TimeoutException();
                        }
                    }
                }
                catch (TaskCanceledException)
                {
                    return;
                }
            });

            await Task.WhenAll(_completionTask, _cancelTask);

            ChatMessage _answer = ChatMessage.Create(sessionId, "assistant", _sb.ToString());
            ChatDialogue _dialogue = new ChatDialogue(_ask, _answer);

            ChatStorageService.SaveMessage(_ask);
            ChatStorageService.SaveMessage(_answer);

            return _dialogue;
        }
    }
}
