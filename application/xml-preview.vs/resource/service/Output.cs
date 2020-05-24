using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace service
{
    public static class Output
    {
        private static IVsOutputWindowPane s_Output = null;

        public static void Activate()
        {
        }

        public static void Clear()
        {
            try
            {
                var a_Context = __GetPane();
                if (a_Context != null)
                {
                    a_Context.Clear();
                    //a_Context.Activate();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void WriteLine(string message)
        {
            try
            {
                var a_Context = __GetPane();
                if (a_Context != null)
                {
                    a_Context.OutputString(message);
                    //s_Output.Activate();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private static IVsOutputWindowPane __GetPane()
        {
            try
            {
                if (s_Output == null)
                {
                    var a_Context = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
                    {
                        var a_Context1 = Guid.NewGuid();
                        {
                            a_Context.CreatePane(ref a_Context1, "Preview", 1, 1);
                            a_Context.GetPane(ref a_Context1, out s_Output);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return s_Output;
        }
    }
}
