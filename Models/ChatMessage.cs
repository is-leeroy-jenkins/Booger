
namespace Booger
{
    using System;
    using LiteDB;

    public record class ChatMessage
    {
        public ChatMessage(Guid id, Guid sessionId, string role, string content, DateTime timestamp)
        {
            Id = id;
            SessionId = sessionId;
            Role = role;
            Content = content;
            Timestamp = timestamp;
        }

        [BsonId]
        public Guid Id { get; }
        public Guid SessionId { get; }
        public string Role { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; }

        public static ChatMessage Create(Guid sessionId, string role, string content) => 
            new ChatMessage(Guid.NewGuid(), sessionId, role, content, DateTime.Now);
    }
}
