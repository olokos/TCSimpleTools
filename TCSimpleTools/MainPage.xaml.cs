using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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

        private void GetWindowsVersion()
        {
            var analyticsInfo = AnalyticsInfo.VersionInfo;
            DeviceFamily = analyticsInfo.DeviceFamily;
            DeviceFamilyVersion = analyticsInfo.DeviceFamilyVersion;
            ProductName = analyticsInfo.ProductName;

            // Parsing the version information
            ulong v = ulong.Parse(DeviceFamilyVersion);
            ulong v1 = (v & 0xFFFF000000000000L) >> 48;
            ulong v2 = (v & 0x0000FFFF00000000L) >> 32;
            ulong v3 = (v & 0x00000000FFFF0000L) >> 16;
            ulong v4 = v & 0x000000000000FFFFL;
            string version = $"{v1}.{v2}.{v3}.{v4}";

            // Set values to the TextBoxes
            txtBlock.Text = $"Windows Version: {version}";
            txtDeviceFamily.Text = $"Device Family: {DeviceFamily}";
            txtDeviceFamilyVersion.Text = $"Device Family Version: {DeviceFamilyVersion}";
            txtProductName.Text = $"Product Name: {ProductName}";
        }
    }
}