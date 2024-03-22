using AutoUpdaterDotNET;

namespace FFXIVLooseTextureCompiler {
    internal static class Program {
        private static string _version;

        public static string Version { get => _version; set => _version = value; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            bool launchForm = true;
            _version = Application.ProductVersion.Split('+')[0];
            AutoUpdater.InstalledVersion = new Version(_version);
            AutoUpdater.DownloadPath = Application.StartupPath;
            AutoUpdater.Synchronous = true;
            AutoUpdater.Mandatory = true;
            AutoUpdater.UpdateMode = Mode.ForcedDownload;

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length <= 1) {
                AutoUpdater.Start("https://raw.githubusercontent.com/Sebane1/FFXIVLooseTextureCompiler/main/Updater/update6.xml");
                AutoUpdater.ApplicationExitEvent += delegate () {
                    launchForm = false;
                };
            }

            if (launchForm) {
                ApplicationConfiguration.Initialize();
                Application.Run(new MainWindow());
            }
        }
    }
}