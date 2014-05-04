using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace InventoryManagement1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Load the style library
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string styleLibraryPath = new List<string>(assembly.GetManifestResourceNames()).Find(i => i.EndsWith(".isl"));
            if (string.IsNullOrEmpty(styleLibraryPath) == false)
            {
                using (System.IO.Stream stream = assembly.GetManifestResourceStream(styleLibraryPath))
                {
                    if (stream != null)
                        Infragistics.Win.AppStyling.StyleManager.Load(stream);
                }
            }

            Application.Run(new InventoryManagementForm());
        }
    }
}
