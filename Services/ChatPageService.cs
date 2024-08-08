

namespace Booger
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    public class ChatPageService
    {
        private Dictionary<Guid, ChatPage> _pages =
            new Dictionary<Guid, ChatPage>();

        public ChatPageService(
            IServiceProvider services)
        {
            Services = services;
        }

        public IServiceProvider Services { get; }

        public ChatPage GetPage(Guid sessionId)
        {
            if (!_pages.TryGetValue(sessionId, out ChatPage _chatPage))
            {
                using (var _scope = Services.CreateScope())
                {
                    _chatPage = _scope.ServiceProvider.GetRequiredService<ChatPage>();
                    _chatPage.InitSession(sessionId);

                    _pages[sessionId] = _chatPage;
                }
            }

            return _chatPage;
        }

        public bool RemovePage(Guid sessionId)
        {
            return _pages.Remove(sessionId);
        }
    }
}
