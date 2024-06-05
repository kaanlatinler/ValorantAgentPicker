using ValorantAgentPicker.Models;

namespace ValorantAgentPicker
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            AgentPickerContext context = new AgentPickerContext();
            Database database = new Database(context);
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm(database));
        }
    }
}