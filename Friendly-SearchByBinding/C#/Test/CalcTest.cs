using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using System.Diagnostics;
using RM.Friendly.WPFStandardControls;
using System.Windows;
using MVVMCalcDriver;

namespace Test
{
    [TestClass]
    public class CalcTest
    {
        AppDirver _app;

        [TestInitialize]
        public void TestInitialize()
        {
            _app = new AppDirver();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _app.Dispose();
        }

        [TestMethod]
        public void 足し算()
        {
            _app.MainView.Lhs.EmulateChangeText("3");
            _app.MainView.Rhs.EmulateChangeText("5");
            _app.MainView.CalculateTypes.EmulateChangeSelectedIndex(1);
            _app.MainView.CalculateCommand.EmulateClick();
            Assert.AreEqual("8", (string)_app.MainView.Answer.Text);
        }

        [TestMethod]
        public void 引き算()
        {
            _app.MainView.Lhs.EmulateChangeText("10");
            _app.MainView.Rhs.EmulateChangeText("3");
            _app.MainView.CalculateTypes.EmulateChangeSelectedIndex(2);
            _app.MainView.CalculateCommand.EmulateClick();
            Assert.AreEqual("7", (string)_app.MainView.Answer.Text);
        }

        [TestMethod]
        public void 掛け算()
        {
            _app.MainView.Lhs.EmulateChangeText("3");
            _app.MainView.Rhs.EmulateChangeText("5");
            _app.MainView.CalculateTypes.EmulateChangeSelectedIndex(3);
            _app.MainView.CalculateCommand.EmulateClick();
            Assert.AreEqual("15", (string)_app.MainView.Answer.Text);
        }

        [TestMethod]
        public void 割り算()
        {
            _app.MainView.Lhs.EmulateChangeText("30");
            _app.MainView.Rhs.EmulateChangeText("5");
            _app.MainView.CalculateTypes.EmulateChangeSelectedIndex(4);
            _app.MainView.CalculateCommand.EmulateClick();
            Assert.AreEqual("6", (string)_app.MainView.Answer.Text);
        }
    }
}
