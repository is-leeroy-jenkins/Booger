

namespace Booger
{
    using System.Diagnostics;
    using System.IO;

    public static class GlobalValues
    {
        public static string AppName => nameof(Booger);

        public static string JsonConfigurationFilePath { get; } =
            Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName) ?? "./", "AppConfig.json");
    }
}
