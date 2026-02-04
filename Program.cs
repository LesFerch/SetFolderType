using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Management;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SetFolderType
{
    public class ExtractData
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFile);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int LoadString(IntPtr hInstance, int ID, StringBuilder lpBuffer, int nBufferMax);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeLibrary(IntPtr hModule);

        public string ExtractStringFromDLL(string file, int number)
        {
            IntPtr lib = LoadLibrary(file);
            StringBuilder result = new StringBuilder(2048);
            LoadString(lib, number, result, result.Capacity);
            FreeLibrary(lib);
            return result.ToString();
        }
    }
    public class IniFileHelper
    {
        private string filePath;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);

        public IniFileHelper(string path)
        {
            filePath = path;
        }

        public void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, filePath);
        }

        public string ReadValue(string section, string key)
        {
            const int size = 255;
            System.Text.StringBuilder builder = new System.Text.StringBuilder(size);
            GetPrivateProfileString(section, key, "", builder, size, filePath);
            return builder.ToString();
        }

        public void UpdateValue(string section, string key, string value)
        {
            WriteValue(section, key, value);
        }
    }

    class Program
    {
        static string myName = typeof(Program).Namespace;
        static string lang = "en-US";
        static string[] types = { "Generic", "Documents", "Pictures", "Music", "Videos", "None", "Help" };
        static string[] labels = { "General items", "Documents", "Pictures", "Music", "Videos", "(None)", "Help" };
        static string[] cmds = { "/g", "/d", "/p", "/m", "/v", "/x", "/h" };
        static string sMain = "Set folder type to";
        static string sYes = "Yes";
        static string sNo = "No";
        static string sInstall = "Install";
        static string sRemove = "Remove";
        static string sComplete = "Done";
        static string sLocalDisk = "Local Disk";
        static string sNTFS = "NTFS";
        static bool ctrlKey = false;
        static bool shiftKey = false;

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ctrlKey = (GetAsyncKeyState(0x11) & 0x8000) != 0;
            shiftKey = (GetAsyncKeyState(0x10) & 0x8000) != 0;

            lang = GetLang();
            if (lang.Substring(0, 2) != "en") { GetStrings(); }

            if (args.Length == 0)
            {
                InstallRemove();
                return;
            }

            string Folder = "";
            string command = "";

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("/")) { command = args[i].ToLower(); }
                else
                {
                    Folder = args[i].Replace("\"", "").Trim();
                }
            }

            if (command == "/install") { InstallContextMenuEntries(); }

            if (command == "/remove") { RemoveContextMenuEntries(); }

            if (Folder == "") { return; }

            CheckRegCmd(Folder);

            for (int i = 0; i < types.Length; i++)
            {
                if (command == cmds[i])
                {
                    if (command == "/h")
                    {
                        Process.Start("https://lesferch.github.io/SetFolderType/");
                    }
                    else
                    {
                        string driveLetter = Path.GetPathRoot(Folder).TrimEnd('\\');

                        if ((command == "/x") || DriveOK(driveLetter))
                        {
                            if (ctrlKey) ApplyFolderType(types[i], Folder);
                            else ApplyFolderTypeRecursive(types[i], Folder);
                        }
                        else
                        {
                            MessageBox.Show($"{driveLetter}\n\n{sLocalDisk}\n{sNTFS}", myName);
                        }
                    }
                }
            }
        }

        static void InstallRemove()
        {
            DialogResult result = MessageBox.Show($"{sYes} = {sInstall}\n\n{sNo} = {sRemove}", myName, MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                InstallContextMenuEntries();
                MessageBox.Show(sComplete, myName);
            }
            if (result == DialogResult.No)
            {
                RemoveContextMenuEntries();
                MessageBox.Show(sComplete, myName);
            }
        }
        static bool DriveOK(string driveLetter)
        {
            bool bOK = true;

            // Check if the drive type is "Local Disk"
            DriveInfo driveInfo = new DriveInfo(driveLetter);

            if (driveInfo.DriveType == DriveType.Fixed)
            {
                sLocalDisk = $"✓ {sLocalDisk}";
            }
            else
            {
                sLocalDisk = $"✗ {sLocalDisk}";
                bOK = false;
            }

            // Check if drive is formatted NTFS
            if (IsNTFS(driveLetter))
            {
                sNTFS = $"✓ {sNTFS}";
            }
            else
            {
                sNTFS = $"✗ {sNTFS}";
                bOK = false;
            }

            return bOK;
        }

        // Verify that the registry command ends with a quote (fixes bug from previous versions)
        static void CheckRegCmd(string Folder)
        {
            bool QuoteFound = false;
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Classes\Directory\shell\SetFolderType\shell\0-Generic\command");
                if (key != null)
                {
                    string value = key.GetValue("") as string;
                    QuoteFound = value.EndsWith("\"");
                    key.Close();
                }
            }
            catch { }
            if (!QuoteFound)
            {
                InstallContextMenuEntries();
                if (!Folder.Contains("\\"))
                {
                    MessageBox.Show("Update applied to context menu registry entries. Please try again.", myName);
                    Environment.Exit(0);
                }
            }
        }

        static string GetLang()
        {
            string lang = "en-US";

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\International");
                if (key != null)
                {
                    lang = key.GetValue("LocaleName") as string;
                    key.Close();
                }
            }
            catch { }

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop");
                if (key != null)
                {
                    string[] preferredLanguages = key.GetValue("PreferredUILanguages") as string[];
                    if (preferredLanguages != null && preferredLanguages.Length > 0)
                    {
                        lang = preferredLanguages[0];
                    }
                    key.Close();
                }
            }
            catch { }

            return lang;
        }
        static void GetStrings()
        {
            string file = $@"C:\Windows\System32\{lang}\Shell32.dll.mui";

            if (!File.Exists(file))
            {
                MessageBox.Show($"Language file not found:\n\n{file}", myName);
                return;
            }

            sYes = GetString(file, 33224); // Yes
            sNo = GetString(file, 33232); // No
            sInstall = GetString(file, 10210); // Install
            sRemove = GetString(file, 38314); // Remove
            sComplete = GetString(file, 12574); // Finished
            sLocalDisk = GetString(file, 9315); // Local Disk
            sMain = GetString(file, 6495); // Folder View
            labels[0] = GetString(file, 29990); // General items
            labels[1] = GetString(file, 21770); // Documents
            labels[2] = GetString(file, 17451); // Pictures
            labels[3] = GetString(file, 17450); // Music
            labels[4] = GetString(file, 17452); // Videos
            labels[5] = GetString(file, 4256); // (None)
            labels[6] = GetString(file, 30489); // Help
        }
        static string GetString(string file, int num)
        {
            ExtractData ed = new ExtractData();
            string sLocStr = ed.ExtractStringFromDLL(file, num);
            sLocStr = sLocStr.Replace("&", "");
            // Remove any keyboard shortcut indicator from string
            if (sLocStr.Length > 3) { sLocStr = Regex.Replace(sLocStr, @"\((.)\)", ""); }
            return sLocStr;
        }
        static void InstallContextMenuEntries()
        {

            RemoveContextMenuEntries();

            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\Classes\Directory\shell\{myName}"))
            {
                key.SetValue("SubCommands", "");
                key.SetValue("", "");
                key.SetValue("MUIVerb", sMain);
                key.SetValue("Icon", exePath);
            }

            for (int i = 0; i < labels.Length; i++)
            {
                using (RegistryKey subKey = Registry.CurrentUser.CreateSubKey($@"Software\Classes\Directory\shell\{myName}\shell\{i}-{types[i]}"))
                {
                    subKey.SetValue("", labels[i]);

                    using (RegistryKey commandKey = subKey.CreateSubKey("command"))
                    {
                        commandKey.SetValue("", $"\"{exePath}\" {cmds[i]} \"%v\"");
                    }
                }
            }
        }

        static void RemoveContextMenuEntries()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Classes\Directory\shell", true))
            {
                try { key.DeleteSubKeyTree(myName, false); }
                catch { }
            }
        }

        static void ApplyFolderTypeRecursive(string folderType, string directory)
        {
            // Apply to the selected folder
            try { ApplyFolderType(folderType, directory); }
            catch { }

            // Recursively apply to subdirectories
            string[] subDirectories = Directory.GetDirectories(directory);
            foreach (string subDirectory in subDirectories)
            {
                try { ApplyFolderTypeRecursive(folderType, subDirectory); }
                catch { }
            }
        }

        static void ApplyFolderType(string folderType, string directory)
        {
            string desktopIniPath = Path.Combine(directory, "desktop.ini");

            if (folderType == "None")
            {
                if (File.Exists(desktopIniPath))
                {
                    // If Shift key is held, force delete the file (old behavior)
                    if (shiftKey)
                    {
                        File.Delete(desktopIniPath);
                    }
                    else
                    {
                        // Remove the FolderType entry
                        IniFileHelper iniFile = new IniFileHelper(desktopIniPath);
                        iniFile.WriteValue("ViewState", "FolderType", null);
                        
                        // Check if there are other meaningful entries
                        if (!HasOtherEntries(desktopIniPath))
                        {
                            File.Delete(desktopIniPath);
                        }
                    }
                }
                return;
            }

            if (File.Exists(desktopIniPath))
            {
                UpdateValue(desktopIniPath, "ViewState", "FolderType", folderType);
            }
            else
            {
                CreateIniFile(desktopIniPath);
                UpdateValue(desktopIniPath, "ViewState", "FolderType", folderType);
                File.SetAttributes(desktopIniPath, File.GetAttributes(desktopIniPath) | FileAttributes.System | FileAttributes.Hidden);
            }
        }

        static bool HasOtherEntries(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                int entryCount = 0;
                
                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    // Count lines that contain "=" and are not empty
                    if (trimmedLine.Contains("=") && !string.IsNullOrWhiteSpace(trimmedLine))
                    {
                        entryCount++;
                    }
                }
                
                // If there are other entries remaining, keep the file
                return entryCount > 0;
            }
            catch
            {
                return true; // If we can't read the file, assume it has other entries to be safe
            }
        }

        static void CreateIniFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("[ViewState]");
                writer.WriteLine("FolderType=Generic");
            }

        }

        static void UpdateValue(string filePath, string section, string key, string value)
        {
            IniFileHelper iniFile = new IniFileHelper(filePath);
            iniFile.UpdateValue(section, key, value);
        }

        static bool IsNTFS(string driveLetter)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Volume");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                string queryDriveLetter = (string)queryObj["DriveLetter"];
                if (queryDriveLetter != null && queryDriveLetter.Equals(driveLetter, StringComparison.OrdinalIgnoreCase))
                {
                    string fileSystem = (string)queryObj["FileSystem"];
                    if (fileSystem != null && fileSystem.Equals("NTFS", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}