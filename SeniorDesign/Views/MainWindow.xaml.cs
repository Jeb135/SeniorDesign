using System.Windows;
using System.Diagnostics;
using SeniorDesign.ViewModel;
using Vlc.DotNet.Forms;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Interops;
using Vlc.DotNet.Wpf;

namespace SeniorDesign
{
    public partial class MainWindow : Window
    {
        ROVControlsViewModel ViewModel;

        public MainWindow()
        {
            // This doesnt work, for some reason VlcContext isn't in scope here? I can't find enough documentation to figure it out.

            // dll and plugin paths.
            //VlcContext context = new VlcContext();
            //VlcContext.LibVlcDllsPath = "C:\\Program Files\\VideoLAN\\VLC";
            //VlcContext.LibVlcPluginsPath = "C:\\Program Files\\VideoLAN\\VLC\\Plugins";

            //VlcContext.StartupOptions.IgnoreConfig = true;
            //VlcContext.StartupOptions.LogOptions.LogInFile = true;
            //VlcContext.StartupOptions.LogOptions.ShowLoggerConsole = false;
            //VlcContext.StartupOptions.LogOptions.Verbosity - VlcLogVerbosities.None;

            //VlcContext.CloseAll();
            //VlcContext.Initialize();

            InitializeComponent();
            ViewModel = new ROVControlsViewModel(/*ROVStream*/);
            DataContext = ViewModel;
        }
    }
}
