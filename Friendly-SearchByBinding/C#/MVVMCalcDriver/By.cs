using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace MVVMCalcDriver
{
    static class By
    {
        public static dynamic BindingIdentify(AppVar element, AppVar dataItem, string path)
        {
            var v = Binding(element, dataItem, path);
            if (v.Count() != 1)
            {
                throw new NotSupportedException();
            }
            return v.First().Dynamic();
        }

        public static IEnumerable<AppVar> Binding(AppVar element, AppVar dataItem, string path)
        {
            WindowsAppExpander.LoadAssembly((WindowsAppFriend)element.App, typeof(By).Assembly);
            List<AppVar> l = new List<AppVar>();
            foreach (AppVar e in element.App.Type(typeof(By)).ByBindingCore(element, dataItem, path))
            {
                l.Add(e);
            }
            return l;
        }

        static IEnumerable<FrameworkElement> ByBindingCore(FrameworkElement element, object dataItem, string path)
        {
            var l = new List<FrameworkElement>();

            if (GetDependencyProperties(element.GetType()).
                Select(e => element.GetBindingExpression(e)).
                Any(e => e != null && ReferenceEquals(e.DataItem, dataItem) && e.ParentBinding.Path.Path == path))
            {
                l.Add(element);
            }

            foreach (object child in LogicalTreeHelper.GetChildren(element))
            {
                var next = child as FrameworkElement;
                if (next != null)
                {
                    l.AddRange(ByBindingCore(next, dataItem, path));
                }
            }
            return l;
        }

        static IEnumerable<DependencyProperty> GetDependencyProperties(Type type)
        {
            var l = new List<DependencyProperty>();
            if (type == null)
            {
                return l;
            }
            l.AddRange(type.GetFields(BindingFlags.Public | BindingFlags.Static).
               Where(e => e.FieldType == typeof(DependencyProperty)).
               Select(e => (DependencyProperty)type.InvokeMember(e.Name, BindingFlags.GetField, null, null, new object[0])));
            l.AddRange(GetDependencyProperties(type.BaseType));
            return l;
        }
    }
}
