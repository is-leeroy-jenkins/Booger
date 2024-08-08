namespace Booger
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    public static class JsonHelper
    {
        public static JsonSerializerOptions ConfigurationOptions { get; } =
            new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,

                Converters =
                {
                    new JsonStringEnumConverter(),
                }
            };
    }
}
