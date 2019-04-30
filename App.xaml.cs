using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using BD_oneLove.Tools;

namespace BD_oneLove
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                var file = new FileInfo(FileFolderHelper.LogFilePath);
                if (!file.Exists)
                {
                    file.Create();
                }

                using (var stream = new StreamWriter(FileFolderHelper.LogFilePath, true))
                {
                    stream.WriteLine("--------------------------------------------------");
                    stream.WriteLine("Message: " + e.Exception.Message);
                    stream.WriteLine("StackTrace: " + e.Exception.StackTrace);
                    stream.WriteLine("Target: " + e.Exception.TargetSite);
                    Exception ex = e.Exception.InnerException;
                    while (ex != null)
                    {
                        stream.WriteLine("-----------------------");
                        stream.WriteLine("Inner: " + ex.Message + "\n" + ex.StackTrace);
                        ex = ex.InnerException;
                    }
                    
                }
            }
            catch (Exception exception)
            {
            }
        }
    }
}
