using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using RM.Friendly.WPFStandardControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace MVVMCalcDriver
{
    public class AppDirver : IDisposable
    {
        WindowsAppFriend _app;
        public MainViewDirver MainView { get; private set; }

        public AppDirver()
        {
            _app = new WindowsAppFriend(Process.Start("MVVMCalc.exe"));
            MainView = new MainViewDirver(_app.Type<Application>().Current.MainWindow);
        }
        public void Dispose()
        {
            Process.GetProcessById(_app.ProcessId).CloseMainWindow();
        }
    }
}
