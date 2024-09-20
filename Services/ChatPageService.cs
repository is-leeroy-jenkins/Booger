

namespace Booger
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// 
    /// </summary>
    public class ChatPageService
    {
        /// <summary>
        /// The pages
        /// </summary>
        private Dictionary<Guid, ChatPage> _pages =
            new Dictionary<Guid, ChatPage>();

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ChatPageService"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        public ChatPageService( IServiceProvider services)
        {
            Services = services;
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns></returns>
        public ChatPage GetPage(Guid sessionId)
        {
            if (!_pages.TryGetValue( sessionId, out ChatPage _chatPage ) )
            {
                using ( var _scope = Services.CreateScope( ) )
                {
                    _chatPage = (ChatPage)_scope.ServiceProvider.GetRequiredService( typeof( ChatPage ) );
                    _chatPage.InitSession(sessionId);
                    _pages[sessionId] = _chatPage;
                }
            }

            return _chatPage;
        }

        /// <summary>
        /// Removes the page.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns></returns>
        public bool RemovePage(Guid sessionId)
        {
            return _pages.Remove(sessionId);
        }
    }
}
