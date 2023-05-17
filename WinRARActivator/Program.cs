using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WinRARActivator
{
    internal class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        static void Main(string[] args)
        {
            //ShowWindow(GetConsoleWindow(), SW_HIDE);

            Console.Title = "WinRARActivator";

            if (MessageBox.Show("Activate?\n\nYes - activate\nNo - abort & close", "Selection", MessageBoxButtons.YesNo) == DialogResult.No)
                Environment.Exit(0);

            string WinRARPath_cached = string.Empty;

            Console.WriteLine("[LOG] Receiving WinRar installation data");

            //Get WinRARPath
            try 
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\WinRAR.exe");
                if (key != null)
                {
                    string Path = (string)key.GetValue("Path");
                    if (!string.IsNullOrWhiteSpace(Path))
                    {
                        //Console.WriteLine($"Path: {Path}");
                    }
                    else
                    {
                        Console.WriteLine("[FAILURE] Failed to get path. Error: 1");
                        Thread.Sleep(-1);
                    }

                    key.Dispose();

                    WinRARPath_cached = Path;
                }
                else
                {
                    Console.WriteLine("[FAILURE] Failed to get path. Error: 2");
                    Thread.Sleep(-1);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("[FAILURE] Exception: " + ex.Message);
                Thread.Sleep(-1);
            }

            string WinRARLicense = "RAR registration data" + Environment.NewLine + "thecoolersoftwares.net" + Environment.NewLine + "Unlimited Company License" + Environment.NewLine + "UID=db302b358a9065b716aa" + Environment.NewLine + "641221225016aa7dbb2b55c3d1d2eb7bb869d90ec3b58d7fd725f9" + Environment.NewLine + "abd8e4f0af5a3cd5784960fce6cb5ffde62890079861be57638717" + Environment.NewLine + "7131ced835ed65cc743d9777f2ea71a8e32c7e593cf66794343565" + Environment.NewLine + "b41bcf56929486b8bcdac33d50ecf7739960160bbbb0f38a242932" + Environment.NewLine + "1b6fb9c0da9df82dfc4090c5f65742aff7b328539dbab45453fc12" + Environment.NewLine + "6d44b2a1abc8f6a05af978736d0f6c8216dbd6f608bcf12160ec7c" + Environment.NewLine + "796c5b80e8748f83e712c73ae13086053790b2323f790690058353";

            if (File.Exists(WinRARPath_cached + "\\rarreg.key"))
            {
                Console.WriteLine("[LOG] Deleting previously installed license");
                File.Delete(WinRARPath_cached + "\\rarreg.key");
            }

            Console.WriteLine("[LOG] Installing license");

            File.WriteAllText(WinRARPath_cached + "\\rarreg.key", WinRARLicense);

            if (File.Exists(WinRARPath_cached + "\\rarreg.key"))
            {
                //Console.WriteLine("[SUCCESS] WinRAR license has been installed.");
                //Console.WriteLine("          You may now close this window.");

                MessageBox.Show("License installed. WinRar is now permanently activated.", "Success", MessageBoxButtons.OK);
            }
            else
            {
                Console.WriteLine("[FAILURE] WinRAR license has not been installed.");

                MessageBox.Show("Activation failed.", "Error", MessageBoxButtons.OK);
            }

            Environment.Exit(0);
        }
    }
}
