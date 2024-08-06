// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-05-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-05-2024
// ******************************************************************************************
// <copyright file="SqlHistoryRepo.cs" company="Terry D. Eppler">
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
//   SqlHistoryRepo.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class SqlHistoryRepo : IHistoryRepo
    {
        public const string SqlConnectionString =
            "Data Source=localhost\\SQLDev2019;Initial Catalog=CSharpWpfChatGPTDB;Integrated Security=True;Encrypt=False;MultipleActiveResultSets=True";

        private SqlServerContext _sqlServerContext;

        public SqlHistoryRepo( )
        {
            _sqlServerContext = new SqlServerContext( );
        }

        public string DBConfigInfo
        {
            get
            {
                return @"SQL Server DB: localhost\SQLDev2019\CSharpWpfChatGPTDB";
            }
        }

        public List<Chat> LoadChatList( )
        {
            var list = _sqlServerContext
                .HistoryChat
                .Include( x => x.MessageList )
                .Select( x => new Chat( x.Name )
                {
                    Id = x.Id,
                    MessageList =
                        new ObservableCollection<Message>(
                            x.MessageList.Select( y => new Message( y.Sender, y.Text ) ) )
                } ).ToList( );

            return list;
        }

        // chat.Id will be updated to created PK
        public void AddChat( Chat chat )
        {
            var historyChat = new HistoryChat
            {
                Name = chat.Name,
                MessageList = chat.MessageList.Select( x =>
                    new HistoryMessage
                    {
                        Sender = x.Sender,
                        Text = x.Text
                    } ).ToList( ),
                ModifiedTime = DateTime.Now
            };

            // This Add will also add historyChat.MessageList to HistoryMessage table
            _sqlServerContext.HistoryChat.Add( historyChat );
            _sqlServerContext.SaveChanges( );

            // Important to pass back Id (PK)
            chat.Id = historyChat.Id;
        }

        public void DeleteChat( Chat chat )
        {
            var historyChat = _sqlServerContext.HistoryChat.FirstOrDefault( x => x.Id == chat.Id );
            if( historyChat != null )
            {
                // This Remove will also remove historyChat.MessageList from HistoryMessage table
                _sqlServerContext.HistoryChat.Remove( historyChat );
                _sqlServerContext.SaveChanges( );
            }
        }
    }
}