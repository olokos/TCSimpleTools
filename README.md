# TCSimpleTools
A very simple C#/UWP application meant to access some windows-specific APIs, that are only available via the UWP framework. Meant for educational purposes.


Right-clicking install.ps1 should be enough to install the app with all dependencies, but in case it fails:
Make sure to install all Dependencies/X86 and X64. TelemetryDependencies are not needed, only for debugging of this application.
Then install the .appxbundle by double clicking

Press windows key and open the app like any other windows store app.
""Windows key + tc" should also bring up this application

To check the logs of this application, go to:
C:\Users\<YOUR_USERNAME>\AppData\Local\Packages\RANDOMCHARS\LocalState\

Sort by modified and check the latest package.
and there's the log in the .txt file.