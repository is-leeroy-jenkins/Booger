// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-05-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-05-2024
// ******************************************************************************************
// <copyright file="ChatHistory.cs" company="Terry D. Eppler">
//    Booger is a quick & dirty WPF application that interacts with OpenAI GPT-3.5 Turbo API
//    based on NET6 and written in C-Sharp.
// 
//    Copyright ©  2024  Terry D. Eppler
// 
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the “Software”),
//    to deal in the Software without restriction,
//    including without limitation the rights to use,
//    copy, modify, merge, publish, distribute, sublicense,
//    and/or sell copies of the Software,
//    and to permit persons to whom the Software is furnished to do so,
//    subject to the following conditions:
// 
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
// 
//    THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//    INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT.
//    IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
//    DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//    DEALINGS IN THE SOFTWARE.
// 
//    You can contact me at: terryeppler@gmail.com or eppler.terry@epa.gov
// </copyright>
// <summary>
//   ChatHistory.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System.Collections.Generic;
    using System.Linq;

    public class ChatHistory
    {
        private const string _NewChatName = "New Chat";

        public ChatHistory( )
        {
            ChatList = new List<Chat>( );
        }

        // On the left panel
        public List<Chat> ChatList { get; }

        public bool NewChatExists
        {
            get
            {
                return ChatList.Exists( x => x.Name == ChatHistory._NewChatName );
            }
        }

        public bool IsNewChat( string name )
        {
            return name == ChatHistory._NewChatName;
        }

        public Chat AddNewChat( )
        {
            return AddChat( ChatHistory._NewChatName );
        }

        public Chat AddChat( string name )
        {
            var chat = new Chat( name );
            ChatList.Add( chat );
            return chat;
        }

        public void DeleteChat( string name )
        {
            ChatList.RemoveAll( x => x.Name == name );
        }

        public void RenameNewChat( string newName )
        {
            var chat = ChatList.FirstOrDefault( x => x.Name.Equals( ChatHistory._NewChatName ) );
            if( chat != null )
            {
                var maxToDisplayInSelectedChat = 120;
                if( newName.Length <= maxToDisplayInSelectedChat )
                {
                    chat.Name = newName;
                }
                else
                {
                    // NOTE: lose original prompt for a short UI display
                    chat.Name = newName.Substring( 0, maxToDisplayInSelectedChat );
                }
            }
        }
    }
}