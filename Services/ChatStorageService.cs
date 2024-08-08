﻿

namespace Booger
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using LiteDB;

    public class ChatStorageService : IDisposable
    {
        public ChatStorageService(
            ConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }

        private ILiteCollection<ChatSession> ChatSessions { get; set; }
        private ILiteCollection<ChatMessage> ChatMessages { get; set; }


        public LiteDatabase Database { get; private set; }
        public ConfigurationService ConfigurationService { get; }

        public ChatSession GetSession(Guid id)
        {
            if (ChatSessions == null)
                throw new InvalidOperationException("Not initialized");

            return ChatSessions.FindOne(session => session.Id == id);
        }

        public IEnumerable<ChatSession> GetAllSessions()
        {
            if (ChatSessions == null)
                throw new InvalidOperationException("Not initialized");

            return ChatSessions.FindAll();
        }

        public ChatStorageService SaveNewSession(string name)
        {
            ChatSession _session = ChatSession.Create(name);

            return SaveSession(_session);
        }

        public ChatStorageService SaveSession(ChatSession session)
        {
            if (ChatSessions == null)
                throw new InvalidOperationException("Not initialized");

            if (!ChatSessions.Update(session.Id, session))
                ChatSessions.Insert(session.Id, session);

            return this;
        }

        public bool DeleteSession(Guid id)
        {
            if (ChatSessions == null)
                throw new InvalidOperationException("Not initialized");

            return ChatSessions.DeleteMany(session => session.Id == id) > 0;
        }

        public bool DeleteMessage(ChatMessage message)
        {
            if (ChatMessages == null)
                throw new InvalidOperationException("Not initialized");

            return ChatMessages.DeleteMany(msg => msg.Id == message.Id) > 0;
        }

        public IEnumerable<ChatMessage> GetLastMessages(Guid sessionId, int maxCount)
        {
            if (ChatMessages == null)
                throw new InvalidOperationException("Not initialized");

            return ChatMessages.Query()
                .Where(msg => msg.SessionId == sessionId)
                .OrderByDescending(msg => msg.Timestamp)
                .Limit(maxCount)
                .ToEnumerable()
                .Reverse();
        }

        public IEnumerable<ChatMessage> GetLastMessagesBefore(Guid sessionId, int maxCount, DateTime timestamp)
        {
            if (ChatMessages == null)
                throw new InvalidOperationException("Not initialized");

            return ChatMessages.Query()
                .Where(msg => msg.SessionId == sessionId)
                .Where(msg => msg.Timestamp < timestamp)
                .OrderByDescending(msg => msg.Timestamp)
                .Limit(maxCount)
                .ToEnumerable();
        }

        public int DeleteMessagesBefore(Guid sessionId, DateTime timestamp)
        {
            if (ChatMessages == null)
                throw new InvalidOperationException("Not initialized");

            return ChatMessages.DeleteMany(msg => msg.SessionId == sessionId && msg.Timestamp < timestamp);
        }

        public int DeleteMessagesAfter(Guid sessionId, DateTime timestamp)
        {
            if (ChatMessages == null)
                throw new InvalidOperationException("Not initialized");

            return ChatMessages.DeleteMany(msg => msg.SessionId == sessionId && msg.Timestamp > timestamp);
        }

        public IEnumerable<ChatMessage> GetAllMessages(Guid sessionId)
        {
            if (ChatMessages == null)
                throw new InvalidOperationException("Not initialized");

            return ChatMessages.Query()
                .Where(msg => msg.SessionId == sessionId)
                .OrderBy(msg => msg.Timestamp)
                .ToEnumerable();
        }

        public ChatStorageService SaveNewMessage(Guid sessionId, string role, string content)
        {
            ChatMessage _message = ChatMessage.Create(sessionId, role, content);

            return SaveMessage(_message);
        }

        public ChatStorageService SaveMessage(ChatMessage message)
        {
            if (ChatMessages == null)
                throw new InvalidOperationException("Not initialized");

            if (!ChatMessages.Update(message.Id, message))
                ChatMessages.Insert(message.Id, message);

            return this;
        }

        public bool ClearMessage(Guid sessionId)
        {
            if (ChatMessages == null)
                throw new InvalidOperationException("Not initialized");

            return ChatMessages.DeleteMany(msg => msg.SessionId == sessionId) > 0;
        }


        public void Initialize()
        {
            Database = new LiteDatabase(
                new ConnectionString()
                {
                    Filename = Path.Combine(
                        FileSystemUtils.GetEntryPointFolder(),
                        ConfigurationService.Configuration.ChatStoragePath),
                });

            ChatSessions = Database.GetCollection<ChatSession>();
            ChatMessages = Database.GetCollection<ChatMessage>();
        }


        bool _disposed = false;
        public void Dispose()
        {
            if (_disposed)
                return;

            Database?.Dispose();
            _disposed = true;
        }
    }
}
