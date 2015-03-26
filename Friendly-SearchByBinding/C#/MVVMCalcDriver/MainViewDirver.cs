using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows.Grasp;
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
            var w = new WindowControl(mainView);

            //指定のオブジェクトを起点とした
            //ロジカルツリー中にあるLhsというパスにバインドされている要素のコレクション
            //Identifyは一つだけであることを保障する
            //コレクションの要素が1以外だった場合は例外発生
            w.LogicalTree().ByBinding("Lhs").Identify();

            //ビジュアルツリーでは、こんな感じで書く
            w.VisualTree().ByBinding("Lhs").Identify();

            //データアイテムを指定して、さらに厳密な指定にできる
            var dataItem = new ExplicitVar(w.Dynamic().DataContext);
            w.LogicalTree().ByBinding("Lhs", dataItem).Identify();


            Lhs = new WPFTextBox(w.LogicalTree().ByBinding("Lhs").Identify());
            CalculateTypes = new WPFComboBox(w.LogicalTree().ByBinding("CalculateTypes").Identify());
            Rhs = new WPFTextBox(w.LogicalTree().ByBinding("Rhs").Identify());
            CalculateCommand = new WPFButtonBase(w.LogicalTree().ByBinding("CalculateCommand").Identify());
            Answer = w.LogicalTree().ByBinding("Answer").Identify().Dynamic();
        }
    }
}
