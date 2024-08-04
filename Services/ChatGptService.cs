// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-04-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-04-2024
// ******************************************************************************************
// <copyright file="WhetstoneChatGPTService.cs" company="Terry D. Eppler">
//     Booger is a quick & dirty WPF application that interacts with OpenAI GPT-3.5 Turbo API
//     based on NET6 and written in C-Sharp.
// 
//     Copyright ©  2022 Terry D. Eppler
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
//    You can contact me at:  terryeppler@gmail.com or eppler.terry@epa.gov
// </copyright>
// <summary>
//   WhetstoneChatGPTService.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Whetstone.ChatGPT.Models;
    using Whetstone.ChatGPT;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Local" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    public class ChatGptService
    {
        /// <summary>
        /// The chat GPT client
        /// </summary>
        private ChatGPTClient _chatGPTClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatGptService"/> class.
        /// </summary>
        /// <param name="openaiApiKey">The openai API key.</param>
        public ChatGptService( string openaiApiKey )
        {
            _chatGPTClient = new ChatGPTClient( openaiApiKey );
        }

        // After 2024-01-04, must use GPT-3.5 with ChatGPT35Models.Turbo because Davinci003 (etc) is deprecated.
        // https://platform.openai.com/docs/deprecations/deprecation-history
        /// <summary>
        /// Creates the chat completion asynchronous.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns></returns>
        public async Task<ChatGPTChatCompletionResponse> CreateChatCompletionAsync( string prompt )
        {
            var gptRequest = new ChatGPTChatCompletionRequest
            {
                Model = ChatGPT35Models.Turbo,
                Messages = new List<ChatGPTChatCompletionMessage>( )
                {
                    new ChatGPTChatCompletionMessage( )
                    {
                        Role = ChatGPTMessageRoles.System,
                        Content = "You are a helpful assistant."
                    },
                    new ChatGPTChatCompletionMessage( )
                    {
                        Role = ChatGPTMessageRoles.User,
                        Content = prompt
                    }
                },
                Temperature = 0.9f,
                MaxTokens = 500
            };

            return await _chatGPTClient.CreateChatCompletionAsync( gptRequest );
        }

        // After 2024-01-04, must use GPT-3.5 with ChatGPT35Models.Turbo
        /// <summary>
        /// Streams the chat completion asynchronous.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns></returns>
        public IAsyncEnumerable<ChatGPTChatCompletionStreamResponse> StreamChatCompletionAsync(
            string prompt )
        {
            var completionRequest = new ChatGPTChatCompletionRequest
            {
                Model = ChatGPT35Models.Turbo,
                Messages = new List<ChatGPTChatCompletionMessage>( )
                {
                    new ChatGPTChatCompletionMessage( )
                    {
                        Role = ChatGPTMessageRoles.System,
                        Content = "You are a helpful assistant."
                    },
                    new ChatGPTChatCompletionMessage( )
                    {
                        Role = ChatGPTMessageRoles.User,
                        Content = prompt
                    }
                },
                Temperature = 0.9f,
                MaxTokens = 500
            };

            return _chatGPTClient.StreamChatCompletionAsync( completionRequest );
        }

        /// <summary>
        /// Creates the image asynchronous.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns></returns>
        public async Task<byte[ ]> CreateImageAsync( string prompt )
        {
            var imageRequest = new ChatGPTCreateImageRequest
            {
                Prompt = prompt,
                Size = CreatedImageSize.Size1024,
                ResponseFormat = CreatedImageFormat.Base64
            };

            byte[ ] imageBytes = null;
            var imageResponse = await _chatGPTClient.CreateImageAsync( imageRequest );
            if( imageResponse != null )
            {
                var imageData = imageResponse.Data?[ 0 ];
                if( imageData != null )
                {
                    imageBytes = await _chatGPTClient.DownloadImageAsync( imageData );
                }
            }

            return imageBytes;
        }
    }
}