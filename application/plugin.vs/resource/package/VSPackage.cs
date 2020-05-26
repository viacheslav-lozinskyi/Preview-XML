
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace resource.package
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(CONSTANT.NAME, CONSTANT.DESCRIPTION, CONSTANT.VERSION)]
    [Guid(CONSTANT.GUID)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.FirstLaunchSetup_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.ShellInitialized_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionHasMultipleProjects_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionHasSingleProject_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class XMLPreviewPackage : AsyncPackage
    {
        internal static class CONSTANT
        {
            public const string GUID = "9E4EBB7D-C389-407B-900F-ADCD2E3F1B80";
            public const string NAME = "XML-Preview";
            public const string DESCRIPTION = "Quick preview for selected XML files";
            public const string VERSION = "1.0.1.0";
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            {
                cartridge.AnyMenu.Connect(this);
                cartridge.AnyPreview.Connect(new preview.XML());
            }
        }

        protected override int QueryClose(out bool canClose)
        {
            {
                cartridge.AnyPreview.Disconnect();
                cartridge.AnyMenu.Disconnect();
                canClose = true;
            }
            return VSConstants.S_OK;
        }
    }
}
