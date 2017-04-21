using System.Windows;
using System.Diagnostics;
using SeniorDesign.ViewModel;
using Vlc.DotNet.Forms;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Interops;
using Vlc.DotNet.Wpf;
using System.Reflection;
using System;
using System.IO;

namespace SeniorDesign
{
    public partial class MainWindow : Window
    {
        ROVControlsViewModel ViewModel;

        public MainWindow()
        {
            InitializeComponent();
            //ROVStream.MediaPlayer.VlcLibDirectoryNeeded += OnVlcControlNeedsLibDirectory;
            ROVStream.MediaPlayer.VlcLibDirectory = new DirectoryInfo(@"C:\Program Files\VideoLAN\VLC");
            ROVStream.MediaPlayer.EndInit();

            ViewModel = new ROVControlsViewModel(/*ROVStream*/);
            this.DataContext = ViewModel;
        }

        private void OnVlcControlNeedsLibDirectory(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            if (currentDirectory == null)
                return;
            if (AssemblyName.GetAssemblyName(currentAssembly.Location).ProcessorArchitecture == ProcessorArchitecture.X86)
                e.VlcLibDirectory = new DirectoryInfo(@"C:\Program Files\VideoLAN\VLC");
            else
                e.VlcLibDirectory = new DirectoryInfo(@"C:\Program Files\VideoLAN\VLC");
        }


        // These methods are for implementing the 'play' etc buttons.
        //private void OnPlayButtonClick(object sender, RoutedEventArgs e)
        //{
        //    ROVStream.MediaPlayer.Play(new Uri("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_surround-fix.avi"));
        //    //myControl.MediaPlayer.Play(new FileInfo(@"..\..\..\Vlc.DotNet\Samples\Videos\BBB trailer.mov"));
        //}

        //private void OnForwardButtonClick(object sender, RoutedEventArgs e)
        //{
        //    ROVStream.MediaPlayer.Rate = 2;
        //}
        
        //private void GetLength_Click(object sender, RoutedEventArgs e)
        //{
        //    GetLength.Content = ROVStream.MediaPlayer.Length + " ms";
        //}
        
        //private void GetCurrentTime_Click(object sender, RoutedEventArgs e)
        //{
        //    GetCurrentTime.Content = ROVStream.MediaPlayer.Time + " ms";
        //}
        
        //private void SetCurrentTime_Click(object sender, RoutedEventArgs e)
        //{
        //    ROVStream.MediaPlayer.Time = 5000;
        //    SetCurrentTime.Content = ROVStream.MediaPlayer.Time + " ms";
        //}
    }
}
