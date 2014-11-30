using RM.Friendly.WPFStandardControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVVMCalcDriver
{
    public class MainViewDirver
    {
        public WPFTextBox Lhs { get; private set; }
        public WPFComboBox CalculateTypes { get; private set; }
        public WPFTextBox Rhs { get; private set; }
        public WPFButtonBase CalculateCommand { get; private set; }
        public dynamic Answer { get; private set; }

        public MainViewDirver(dynamic mainView)
        {
            Lhs = new WPFTextBox(By.BindingIdentify(mainView, mainView.DataContext, "Lhs"));
            CalculateTypes = new WPFComboBox(By.BindingIdentify(mainView, mainView.DataContext, "CalculateTypes"));
            Rhs = new WPFTextBox(By.BindingIdentify(mainView, mainView.DataContext, "Rhs"));
            CalculateCommand = new WPFButtonBase(By.BindingIdentify(mainView, mainView.DataContext, "CalculateCommand"));
            Answer = By.BindingIdentify(mainView, mainView.DataContext, "Answer");
        }
    }
}
