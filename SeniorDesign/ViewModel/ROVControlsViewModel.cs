using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using SeniorDesign.Models;
using System.Windows.Threading;
using System.Net.Sockets;
using System.Web.Script.Serialization;
using Vlc.DotNet.Wpf;
using System.IO;

namespace SeniorDesign.ViewModel
{
    class ROVControlsViewModel
    {
        #region Properties
        //This setup might not work.
        private ControlBlock _block;
        public ControlBlock block
        {
            get
            {
                return block;
            }
            set
            {
                _block = value;
            }
        }

        private DispatcherTimer _SendControlSignalDispatchTimer;
        public DispatcherTimer SendControlSignalDispatchTimer
        {
            get
            {
                return _SendControlSignalDispatchTimer;
            }
            set
            {
                _SendControlSignalDispatchTimer = value;
            }
        }

        private bool _connected;
        public bool connected
        {
            get
            {
                return _connected;
            }
            set
            {
                _connected = value;
            }
        }

        private Int32 _port;
        public Int32 port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }

        private string _server;
        public string server
        {
            get
            {
                return _server;
            }
            set
            {
                _server = value;
            }
        }

        private string _streamUrl;
        public string streamUrl
        {
            get
            {
                return _streamUrl;
            }
            set
            {
                _streamUrl = value;
            }
        }

        private Uri _ROVVideo;
        public Uri ROVVideo
        {
            get
            {
                if(_ROVVideo == null) { _ROVVideo = new Uri(streamUrl); }
                return _ROVVideo;
            }
            set
            {
                _ROVVideo = value;
                //OnPropertyChanged("something");
            }
        }
        #endregion

        #region Commands
        private ICommand _ConnectWirelessCommand;
        public ICommand ConnectWirelessCommand
        {
            get
            {
                if (_ConnectWirelessCommand == null)
                {
                    _ConnectWirelessCommand = new RelayCommand(ConnectWirelessExecute, CanExecuteConnectWirelessCommand);
                }
                return _ConnectWirelessCommand;
            }
        }

        private ICommand _ReturnToSurfaceCommand;
        public ICommand ReturnToSurfaceCommand
        {
            get
            {
                if (_ReturnToSurfaceCommand == null)
                {
                    _ReturnToSurfaceCommand = new RelayCommand(ReturnToSurfaceExecute, CanExecuteReturnToSurfaceCommand);
                }
                return _ReturnToSurfaceCommand;
            }
        }

        private bool CanExecuteConnectWirelessCommand()
        {
            //Conditions on if this command can be executed.
            return true;
        }

        private bool CanExecuteReturnToSurfaceCommand()
        {
            //Conditions on if this command can be executed.
            return true;
        }

        public void ConnectWirelessExecute()
        {
            // Attempt to make a connection to the ROV here.
            ConnectROV();
        }

        public void ReturnToSurfaceExecute()
        {
            // Tell the ROV to return to the surface.
        }


        // Add Commands for other buttons.
        // To be added:
        // ReturnToSurfaceCommand

        #endregion

        public ROVControlsViewModel(VlcControl ROVVideo)
        {
            server = "";
            port = 0;
            if (ConnectROV())
            {
                StartDispatchTimer();
            }
        }

        public bool ConnectROV()
        {
            // Return if connection was successful or not.
            if (!connected)
            {
                // Attempt to make TCP connection to ROV. 

                // If successful in connecting, start dispatch timer, but check to make sure its not started already.
                // Perform check to see if already running this timer.
                StartDispatchTimer();
            }
            else
            {
                // Show some kind of 'already connected' dialog window. 
            }
            return true;
        }

        public void StartDispatchTimer()
        {
            // Uncomment to start dispatch timer.
            //SendControlSignalDispatchTimer = new DispatcherTimer();
            //SendControlSignalDispatchTimer.Tick += new EventHandler(SendControlSignal);
            //SendControlSignalDispatchTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            //SendControlSignalDispatchTimer.Start();
        }

        public void SendControlSignal(object sender, EventArgs e)
        {
            // Pack up the control block and send it off with TCP connection here.
            TcpClient connection = new TcpClient(server, port);

            // Configure connection content.
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string message = serializer.Serialize(block);
            //string message = System.Runtime.Serialization.JSON.stringify(block);
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            NetworkStream stream = connection.GetStream();

            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);

            // Return bytes.
            data = new Byte[256];
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

            // Close stream and connection.
            stream.Close();
            connection.Close();
        }

        public void StartVideoStream(VlcControl ROVStream)
        {
            //ROVStream.MediaPlayer.VlcLibDirectoryNeeded += OnVlcControlNeedsLibDirectory;
            ROVStream.MediaPlayer.EndInit();
            //ROVStream.MediaPlayer.Play(new Uri("url of your stream"));
            ROVStream.MediaPlayer.Play(ROVVideo);
        }

        //private void OnVlcControlNeedsLibDirectory(object sender, VlcLibDirectoryNeededEventArgs e)
        //{
        //    // path to VLC installation (don't hardcode in real usage, that is just for example)
        //    e.VlcLibDirectory = new DirectoryInfo(@"C:\Program Files (x86)\VideoLAN\VLC\");
        //}
    }
}
