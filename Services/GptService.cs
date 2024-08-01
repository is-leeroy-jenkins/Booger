// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-01-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-01-2024
// ******************************************************************************************
// <copyright file="GptService.cs" company="Terry D. Eppler">
//     Booger is a quick & dirty application in C sharp for interacting with the OpenAI GPT API.
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
//   GptService.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System.Diagnostics.CodeAnalysis;
    using Whetstone.ChatGPT.Models.Image;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Whetstone.ChatGPT.Models;
    using Whetstone.ChatGPT;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "BadParensLineBreaks" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    public class GptService
    {
        /// <summary>
        /// The chat GPT client
        /// </summary>
        private readonly ChatGPTClient _chatGptClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GptService"/> class.
        /// </summary>
        /// <param name="openaiApiKey">The openai API key.</param>
        public GptService( string openaiApiKey )
        {
            _chatGptClient = new ChatGPTClient( openaiApiKey );
        }

        /// <summary>
        /// Creates the chat completion asynchronous.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns></returns>
        public async Task<ChatGPTChatCompletionResponse> CreateChatCompletionAsync( string prompt )
        {
            var _gptRequest = new ChatGPTChatCompletionRequest
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

            return await _chatGptClient.CreateChatCompletionAsync( _gptRequest );
        }

        /// <summary>
        /// Streams the chat completion asynchronous.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns></returns>
        public IAsyncEnumerable<ChatGPTChatCompletionStreamResponse> StreamChatCompletionAsync(
            string prompt )
        {
            var _completionRequest = new ChatGPTChatCompletionRequest
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

            return _chatGptClient.StreamChatCompletionAsync( _completionRequest );
        }

        /// <summary>
        /// Creates the image asynchronous.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns></returns>
        public async Task<byte[ ]> CreateImageAsync( string prompt )
        {
            var _imageRequest = new ChatGPTCreateImageRequest
            {
                Prompt = prompt,
                Size = CreatedImageSize.Size1024,
                ResponseFormat = CreatedImageFormat.Base64
            };

            byte[ ] _imageBytes = null;
            var _imageResponse = await _chatGptClient.CreateImageAsync( _imageRequest );
            if( _imageResponse != null )
            {
                var _imageData = _imageResponse.Data?[ 0 ];
                if( _imageData != null )
                {
                    _imageBytes = await _chatGptClient.DownloadImageAsync( _imageData );
                }
            }

            return _imageBytes;
        }
    }
}