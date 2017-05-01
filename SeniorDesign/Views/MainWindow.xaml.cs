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
        //MediaPlayerViewModel MediaPlayerViewModel;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                //myControl.MediaPlayer.VlcLibDirectoryNeeded += OnVlcControlNeedsLibDirectory;

                //myControl.MediaPlayer.EndInit();

                ViewModel = new ROVControlsViewModel();
                this.DataContext = ViewModel;

                // For databinding on the media player. Currently just a place holder and does nothing.
                //MediaPlayerViewModel = new MediaPlayerViewModel();
                //ROVStream.DataContext = MediaPlayerViewModel;

                //ROVStream.Source = new Uri("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_surround-fix.avi");
                //ROVStream.Source = new Uri("rtsp://169.254.250.129:8554/");

                this.Closing += MainWindow_Closing;
                Dispatcher.UnhandledException += UnhandledExceptionHandler;
            }
            catch(Exception ex)
            {
                MessageBoxResult result = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            }

        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ROVStream.Stop();
        }

        private void OnPlayButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ROVStream.Play();
                //myControl.MediaPlayer.Play(new Uri("rtsp://169.254.250.128:8554/"));
            }
            catch(Exception ex)
            {
                MessageBoxResult result = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            }
        }

        private void UnhandledExceptionHandler(object sender, EventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("An unhandled Exception occurred.", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
        }




        //private void OnVlcControlNeedsLibDirectory(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        //{
        //    var currentAssembly = Assembly.GetEntryAssembly();
        //    var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
        //    if (currentDirectory == null)
        //        return;
        //    if (AssemblyName.GetAssemblyName(currentAssembly.Location).ProcessorArchitecture == ProcessorArchitecture.X86)
        //        e.VlcLibDirectory = new DirectoryInfo(@"C:\Users\Joe\Documents\VLCDlls");
        //    else
        //        e.VlcLibDirectory = new DirectoryInfo(@"C:\Users\Joe\Documents\VLCDlls");
        //}
    }
}
