
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;

namespace resource.service
{
    public static class Menu
    {
        internal static class CONSTANT
        {
            public const int ID = 0x0100;
            public const string GUID = "99F5CD5A-C99A-486F-BB4B-38A82AA943AC";
        }

        public static void Connect(AsyncPackage package)
        {
            var a_Context = package.GetService<IMenuCommandService, OleMenuCommandService>();
            if (a_Context != null)
            {
                var a_Context1 = new CommandID(new Guid(CONSTANT.GUID), CONSTANT.ID);
                if (a_Context.FindCommand(a_Context1) == null)
                {
                    var a_Context2 = new OleMenuCommand(__Execute, a_Context1);
                    {
                        a_Context2.BeforeQueryStatus += __BeforeQueryStatus;
                    }
                    {
                        a_Context.AddCommand(a_Context2);
                    }
                }
            }
        }

        private static void __Execute(object sender, EventArgs e)
        {
            var a_Context = Package.GetGlobalService(typeof(SDTE)) as DTE2;
            if (a_Context != null)
            {
                var a_Context1 = (Array)a_Context.ToolWindows.SolutionExplorer.SelectedItems;
                if (a_Context1 != null)
                {
                    foreach (UIHierarchyItem a_Context2 in a_Context1)
                    {
                        var a_Name = __GetFileName(a_Context2.Object);
                        if (string.IsNullOrEmpty(a_Name) == false)
                        {
                            cartridge.AnyPreview.Execute(a_Name);
                        }
                    }
                }
            }
        }

        private static void __BeforeQueryStatus(object sender, EventArgs e)
        {
            var a_Context = Package.GetGlobalService(typeof(SDTE)) as DTE2;
            if ((sender is OleMenuCommand) == false)
            {
                return;
            }
            if (a_Context != null)
            {
                var a_Context1 = (Array)a_Context.ToolWindows.SolutionExplorer.SelectedItems;
                if (a_Context1 != null)
                {
                    foreach (UIHierarchyItem a_Context2 in a_Context1)
                    {
                        var a_Name = __GetFileName(a_Context2.Object);
                        if (string.IsNullOrEmpty(a_Name) == false)
                        {
                            if (cartridge.AnyPreview.IsEnabled(a_Name))
                            {
                                (sender as OleMenuCommand).Enabled = true;
                                return;
                            }
                        }
                    }
                }
            }
            {
                (sender as OleMenuCommand).Enabled = false;
            }
        }

        private static string __GetFileName(object value)
        {
            if (value != null)
            {
                var a_Context = value as ProjectItem;
                if (a_Context != null)
                {
                    var a_Context1 = a_Context.Properties;
                    if (a_Context1 != null)
                    {
                        return a_Context1.Item("FullPath").Value.ToString();
                    }
                }
            }
            return "";
        }
    }
}
