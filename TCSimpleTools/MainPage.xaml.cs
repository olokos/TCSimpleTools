using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Windows.System.Profile;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace TCSimpleTools
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string DeviceFamily { get; private set; }
        public string DeviceFamilyVersion { get; private set; }
        public string ProductName { get; private set; }

        public MainPage()
        {
            this.InitializeComponent();
            GetWindowsVersion();
        }

        private async void GetWindowsVersion()
        {
            // Try to read all 4 variables, each of them might not exist
            try
            {
                var analyticsInfo = AnalyticsInfo.VersionInfo;
                DeviceFamily = analyticsInfo.DeviceFamily;
                //await LogToFile("Device Family set successfully: " + DeviceFamily);
            }
            catch (Exception ex)
            {
                DeviceFamily = "NOT FOUND";
                await LogErrorToFile("Error setting Device Family: " + ex.Message);
            }

            try
            {
                var analyticsInfo = AnalyticsInfo.VersionInfo;
                DeviceFamilyVersion = analyticsInfo.DeviceFamilyVersion;
                //await LogToFile("Device Family Version set successfully: " + DeviceFamilyVersion);
            }
            catch (Exception ex)
            {
                DeviceFamilyVersion = "NOT FOUND";
                await LogErrorToFile("Error setting Device Family Version: " + ex.Message);
            }

            try
            {
                var analyticsInfo = AnalyticsInfo.VersionInfo;
                ProductName = analyticsInfo.ProductName;
                //await LogToFile("Product Name set successfully: " + ProductName);
            }
            catch (Exception ex)
            {
                ProductName = "NOT FOUND";
                await LogErrorToFile("Error setting Product Name: " + ex.Message);
            }

            try
            {
                string version = "NOT FOUND";
                if (DeviceFamilyVersion != "NOT FOUND")
                {
                    // Parsing the version information
                    ulong v = ulong.Parse(DeviceFamilyVersion);
                    ulong v1 = (v & 0xFFFF000000000000L) >> 48;
                    ulong v2 = (v & 0x0000FFFF00000000L) >> 32;
                    ulong v3 = (v & 0x00000000FFFF0000L) >> 16;
                    ulong v4 = v & 0x000000000000FFFFL;
                    version = $"{v1}.{v2}.{v3}.{v4}";
                }

                // Set values to the TextBoxes
                txtWindowsVersion.Text = $"Windows Version: {version}";
                txtWindowsVersion.IsTextSelectionEnabled = true;

                txtDeviceFamily.Text = $"Device Family: {DeviceFamily}";
                txtDeviceFamily.IsTextSelectionEnabled = true;

                txtDeviceFamilyVersion.Text = $"Device Family Version: {DeviceFamilyVersion}";
                txtDeviceFamilyVersion.IsTextSelectionEnabled = true;

                txtProductName.Text = $"Product Name: {ProductName}";
                txtProductName.IsTextSelectionEnabled = true;

                // Logging variables
                await LogToFile(txtWindowsVersion.Text + "\n" + txtDeviceFamily.Text + "\n" + txtDeviceFamilyVersion.Text + "\n" + txtProductName.Text);
            }
            catch (Exception ex)
            {
                await LogErrorToFile("Error parsing or setting version information: " + ex.Message);
            }
        }

        private async Task LogErrorToFile(string error)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile logFile = await localFolder.CreateFileAsync("tc-tools-log.txt", CreationCollisionOption.OpenIfExists);
            string AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            using (StreamWriter writer = File.AppendText(logFile.Path))
            {
                await writer.WriteLineAsync($"\n[Error] {DateTime.Now:dd.MM.yyyy HH:mm:ss} {AppName} {AppVersion}:\n{error}");
            }
        }

        private async Task LogToFile(string message)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile logFile = await localFolder.CreateFileAsync("tc-tools-log.txt", CreationCollisionOption.OpenIfExists);
            string AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            using (StreamWriter writer = File.AppendText(logFile.Path))
            {
                await writer.WriteLineAsync($"\n{DateTime.Now:dd.MM.yyyy HH:mm:ss} {AppName} {AppVersion}:\n{message}");
            }
        }

        private async void OpenLogs_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile logFile = await localFolder.CreateFileAsync("tc-tools-log.txt", CreationCollisionOption.OpenIfExists);

            try
            {
                // Launch the default app associated with the file
                await Launcher.LaunchFileAsync(logFile);
            }
            catch (Exception ex) when (ex.HResult == -2147220991)
            {
                // File type not associated with any app
                var messageDialog = new MessageDialog("You don't have any default app set in Windows to open .txt files!");
                await messageDialog.ShowAsync();
                Application.Current.Exit();
            }
            catch (Exception ex)
            {
                // Other exceptions
                var messageDialog = new MessageDialog("An error occurred: " + ex.Message);
                await messageDialog.ShowAsync();
            }
        }

    }
}