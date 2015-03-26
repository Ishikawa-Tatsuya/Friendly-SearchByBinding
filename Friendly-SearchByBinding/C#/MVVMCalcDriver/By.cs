using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MVVMCalcDriver
{
    public class ExplicitVar : IAppVarOwner
    {
        public ExplicitVar(AppVar v)
        {
            AppVar = v;
        }
        public AppVar AppVar { get; private set; }
    }

    public class WPFDependencyObjectCollection : IAppVarOwner
    {
        public AppVar AppVar { get; private set; }
        public WPFDependencyObjectCollection(AppVar appVar)
        {
            AppVar = appVar;
        }

        public AppVar Identify()
        {
            if ((int)this.Dynamic().Length != 1)
            {
                throw new NotSupportedException();
            }
            return this.Dynamic()[0];
        }

        public IEnumerable<AppVar> Enumerate()
        {
            return new Enumerate(AppVar);
        }
    }

    public enum TreeOption
    {
        Ancestor,
        DepthFirst,
        BreadthFirst
    }

    public static class TreeExtensions
    {
        public static WPFDependencyObjectCollection VisualTree(this AppVar start, TreeOption option = TreeOption.DepthFirst)
        {
            Init(start);
            string name = string.Empty;
            switch (option)
            {
                case TreeOption.DepthFirst:
                    name = "GetVisualTreeCollectionDepthFirst";
                    break;
                case TreeOption.Ancestor:
                    name = "GetVisualTreeCollectionAncestor";
                    break;
                default:
                    throw new NotSupportedException();
            }
            return new WPFDependencyObjectCollection(start.App[typeof(TreeExtensions), name](start));
        }

        public static WPFDependencyObjectCollection VisualTree(this IAppVarOwner start, TreeOption option = TreeOption.DepthFirst)
        {
            return VisualTree(start.AppVar, option);
        }

        public static WPFDependencyObjectCollection LogicalTree(this AppVar start, TreeOption option = TreeOption.DepthFirst)
        {
            Init(start);
            string name = string.Empty;
            switch (option)
            {
                case TreeOption.DepthFirst:
                    name = "GeLogicalTreeCollectionDepthFirst";
                    break;
                case TreeOption.Ancestor:
                    name = "GeLogicalTreeCollectionAncestor";
                    break;
                default:
                    throw new NotSupportedException();
            }
            return new WPFDependencyObjectCollection(start.App[typeof(TreeExtensions), name](start));
        }

        public static WPFDependencyObjectCollection LogicalTree(this IAppVarOwner start, TreeOption option = TreeOption.DepthFirst)
        {
            return LogicalTree(start.AppVar, option);
        }

        static DependencyObject[] GetVisualTreeCollectionDepthFirst(DependencyObject obj)
        {
            List<DependencyObject> list = new List<DependencyObject>();
            list.Add(obj);
            int count = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                list.AddRange(GetVisualTreeCollectionDepthFirst(VisualTreeHelper.GetChild(obj, i)));
            }
            return list.ToArray();
        }

        static DependencyObject[] GetVisualTreeCollectionAncestor(DependencyObject obj)
        {
            List<DependencyObject> list = new List<DependencyObject>();
            while (obj != null)
            {
                list.Add(obj);
                obj = VisualTreeHelper.GetParent(obj);
            }
            return list.ToArray();
        }

        static DependencyObject[] GeLogicalTreeCollectionDepthFirst(DependencyObject obj)
        {
            List<DependencyObject> list = new List<DependencyObject>();
            list.Add(obj);
            foreach (var e in LogicalTreeHelper.GetChildren(obj))
            {
                DependencyObject d = e as DependencyObject;
                if (d != null)
                {
                    list.AddRange(GeLogicalTreeCollectionDepthFirst(d));
                }
            }
            return list.ToArray();
        }

        static DependencyObject[] GeLogicalTreeCollectionAncestor(DependencyObject obj)
        {
            List<DependencyObject> list = new List<DependencyObject>();
            while (obj != null)
            {
                list.Add(obj);
                obj = LogicalTreeHelper.GetParent(obj);
            }
            return list.ToArray();
        }

        static void Init(AppVar appVar)
        {
            var app = (WindowsAppFriend)appVar.App;
            object dmy;
            string key = typeof(TreeExtensions).Assembly.FullName;
            if (!app.TryGetAppControlInfo(key, out dmy))
            {
                WindowsAppExpander.LoadAssembly(app, typeof(TreeExtensions).Assembly);
                app.AddAppControlInfo(key, new object());
            }
        }
    }

    public static class BindingSearchExtensions
    {
        public static WPFDependencyObjectCollection ByBinding(this WPFDependencyObjectCollection collection, string path, ExplicitVar dataItem = null)
        {
            return new WPFDependencyObjectCollection(collection.AppVar.App.Type(typeof(BindingSearchExtensions)).ByBindingCore(collection, dataItem, path));
        }

        static DependencyObject[] ByBindingCore(IEnumerable<DependencyObject> collection, object dataItem, string path)
        {
            return collection.Where(element => 
               GetDependencyProperties(element).
                    Select(e => BindingOperations.GetBindingExpression(element, e)).
                    Any(e => e != null && e.ParentBinding.Path.Path == path && (dataItem == null || ReferenceEquals(e.DataItem, dataItem)))
            ).ToArray();
        }

        static IEnumerable<DependencyProperty> GetDependencyProperties(object obj)
        {
            List<DependencyProperty> list = new List<DependencyProperty>();
            PropertyDescriptorCollection propertyDescriptors =
                TypeDescriptor.GetProperties(obj, new Attribute[] { new PropertyFilterAttribute(PropertyFilterOptions.All) });
            foreach (PropertyDescriptor property in propertyDescriptors)
            {
                DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(property);
                if (dpd != null)
                {
                    list.Add(dpd.DependencyProperty);
                }
            }
            return list;
        }
    }


    public static class TypeSearchExtensions
    {
        public static WPFDependencyObjectCollection ByType(this WPFDependencyObjectCollection collection, string typeFullName)
        {
            return new WPFDependencyObjectCollection(collection.AppVar.App.Type(typeof(TypeSearchExtensions)).ByTypeCore(collection, typeFullName));
        }

        static DependencyObject[] ByTypeCore(IEnumerable<DependencyObject> collection, string typeFullName)
        {
            return collection.Where(e => e.GetType().FullName == typeFullName).ToArray();
        }
    }
}
