
namespace Booger
{
    using System;
    using System.Text;
    using System.Text.Json;

    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        static SnakeCaseNamingPolicy()
        {
            SnakeCaseNamingPolicy._laziedInstance = new Lazy<SnakeCaseNamingPolicy>();
        }

        private SnakeCaseNamingPolicy()
        {

        }

        private static readonly Lazy<SnakeCaseNamingPolicy> _laziedInstance;
        public static SnakeCaseNamingPolicy SnakeCase => SnakeCaseNamingPolicy._laziedInstance.Value;

        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            var _builder = new StringBuilder();
            _builder.Append(char.ToLowerInvariant(name[0]));

            for (var _i = 1; _i < name.Length; _i++)
            {
                if (char.IsUpper(name[_i]))
                {
                    _builder.Append('_');
                    _builder.Append(char.ToLowerInvariant(name[_i]));
                }
                else
                {
                    _builder.Append(name[_i]);
                }
            }

            return _builder.ToString();
        }
    }
}
