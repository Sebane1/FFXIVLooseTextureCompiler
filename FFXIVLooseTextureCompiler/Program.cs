using AutoUpdaterDotNET;

namespace FFXIVLooseTextureCompiler {
    internal static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            bool launchForm = true;
            AutoUpdater.InstalledVersion = new Version(Application.ProductVersion);
            AutoUpdater.DownloadPath = Application.StartupPath;
            AutoUpdater.Synchronous = true;
            AutoUpdater.Mandatory = true;
            AutoUpdater.UpdateMode = Mode.ForcedDownload;
            AutoUpdater.Start("https://raw.githubusercontent.com/Sebane1/FFXIVLooseTextureCompiler/main/update_air.xml");
            AutoUpdater.ApplicationExitEvent += delegate () {
                launchForm = false;
            };

            if (launchForm) {
                ApplicationConfiguration.Initialize();
                Application.Run(new MainWindow());
            }
        }
    }
}