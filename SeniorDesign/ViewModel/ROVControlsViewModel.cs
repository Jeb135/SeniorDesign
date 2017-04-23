using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using SeniorDesign.Models;
using System.Windows.Threading;
using System.Net.Sockets;
using System.Web.Script.Serialization;
using Vlc.DotNet.Wpf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace SeniorDesign.ViewModel
{
    class ROVControlsViewModel : INotifyPropertyChanged
    {
        #region Properties
        // Notify Property Changed implementation.
        // Used to notify GUI that values may have changed so that GUI elements can respond accordingly. Should be used on all elements that are bound to the GUI in some way.
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ControlBlock _block;
        public ControlBlock block
        {
            get
            {
                if(_block == null)
                {
                    _block = new ControlBlock();
                }
                return _block;
            }
            set
            {
                _block = value;
            }
        }

        private int _ForwardSpeed;
        public int ForwardSpeed
        {
            get
            {
                return _ForwardSpeed;
            }
            set
            {
                _ForwardSpeed = value;
                RecalculateMotors();
                NotifyPropertyChanged("ForwardSpeed");
            }
        }

        private int _TurningAngle;
        public int TurningAngle
        {
            get
            {
                return _TurningAngle;
            }
            set
            {
                _TurningAngle = value;
                RecalculateMotors();
                NotifyPropertyChanged("TurningAngle");
            }
        }

        private int _VerticalSpeed;
        public int VerticalSpeed
        {
            get
            {
                return _VerticalSpeed;
            }
            set
            {
                _VerticalSpeed = value;
                RecalculateMotors();
                NotifyPropertyChanged("VerticalSpeed");
            }
        }

        private bool _Lights;
        public bool Lights
        {
            get
            {
                return _Lights;
            }
            set
            {
                _Lights = value;
            }
        }

        private DispatcherTimer _SendControlSignalDispatchTimer;
        public DispatcherTimer SendControlSignalDispatchTimer
        {
            get
            {
                if(_SendControlSignalDispatchTimer == null)
                {
                    StartDispatchTimer();
                }
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

        private ICommand _STOPCommand;
        public ICommand STOPCommand
        {
            get
            {
                if(_STOPCommand == null)
                {
                    _STOPCommand = new RelayCommand(STOPExecute, CanExecuteSTOPCommand);
                }
                return _STOPCommand;
            }
        }

        private ICommand _LightsOnCommand;
        public ICommand LightsOnCommand
        {
            get
            {
                if(_LightsOnCommand == null)
                {
                    _LightsOnCommand = new RelayCommand(LightsOnExecute, CanExecuteLightsOnCommand);
                }
                return _LightsOnCommand;
            }
        }

        private ICommand _LightsOffCommand;
        public ICommand LightsOffComamand
        {
            get
            {
                if(_LightsOffCommand == null)
                {
                    _LightsOffCommand = new RelayCommand(LightsOffExecute, CanExecuteLightsOffCommand);
                }
                return _LightsOffCommand;
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

        private bool CanExecuteSTOPCommand()
        {
            //Conditions on if this command can be executed.
            return true;
        }

        private bool CanExecuteLightsOnCommand()
        {
            // Can lights be turned on?
            return true;
        }

        private bool CanExecuteLightsOffCommand()
        {
            // Can lights be turned on?
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
            block.status = Status.Bad;
        }

        public void STOPExecute()
        {
            // Set all speeds to 0 to make the sub stay where it currently is.
            ForwardSpeed = 0;
            TurningAngle = 0;
            VerticalSpeed = 0;
        }

        public void LightsOnExecute()
        {
            Lights = true;
        }

        public void LightsOffExecute()
        {
            Lights = false;
        }


        // Add Commands for other buttons.
        // To be added:
        // ReturnToSurfaceCommand

        #endregion

        public ROVControlsViewModel()
        {
            server = "";
            port = 0;
            block = new ControlBlock();
            if (ConnectROV())
            {
                StartDispatchTimer();
            }
            EventManager.RegisterClassHandler(typeof(System.Windows.Controls.Control), System.Windows.Controls.Control.KeyDownEvent, new KeyEventHandler(MovementController), true);
        }

        public ROVControlsViewModel(VlcControl ROVVideo)
        {
            server = "169.254.250.128";
            port = 3000;
            block = new ControlBlock();
            if (ConnectROV())
            {
                StartDispatchTimer();
            }
            EventManager.RegisterClassHandler(typeof(System.Windows.Controls.Control), System.Windows.Controls.Control.KeyDownEvent, new KeyEventHandler(MovementController), true);
        }

        public bool ConnectROV()
        {
            // Return if connection was successful or not.
            ForwardSpeed = 0;
            if (!connected)
            {
                // Attempt to make TCP connection to ROV. 
                
                // If successful in connecting, start dispatch timer, but check to make sure its not started already.
                // Perform check to see if already running this timer.
                StartDispatchTimer();
                connected = true;
            }
            else
            {
                // Show some kind of 'already connected' dialog window. 
            }
            return true;
        }

        public void StartDispatchTimer()
        {
            //Uncomment to start dispatch timer.
            SendControlSignalDispatchTimer = new DispatcherTimer();
            SendControlSignalDispatchTimer.Tick += new EventHandler(SendControlSignal);
            SendControlSignalDispatchTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            SendControlSignalDispatchTimer.Start();
        }

        public void SendControlSignal(object sender, EventArgs e)
        {
            // This currently doesn't work. Uncomment to debug/fix.

            try
            {
                ////Pack up the control block and send it off with TCP connection here.
                //TcpClient connection = new TcpClient(server, port);

                //// Configure connection content.
                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                //string message = serializer.Serialize(block);
                ////string message = System.Runtime.Serialization.JSON.stringify(block);
                //Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                //NetworkStream stream = connection.GetStream();

                //// Send the message to the connected TcpServer. 
                //stream.Write(data, 0, data.Length);

                //// Return bytes.
                //data = new Byte[256];
                //String responseData = String.Empty;

                //// Read the first batch of the TcpServer response bytes.
                //Int32 bytes = stream.Read(data, 0, data.Length);
                //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                //// Close stream and connection.
                //stream.Close();
                //connection.Close();

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(server + ":" + port);
                request.Method = "POST";
                request.ContentType = "application/json";
                //string json = JSON.stringify({ "data" : block}); //json.stringify(); json-ify the control block data
                string json = JsonConvert.SerializeObject(block);

                byte[] data = Encoding.UTF8.GetBytes(json);
                request.ContentLength = data.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }

                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        // do stuff with response.
                    }
                }
                catch(WebException ex)
                {
                    // Handle web error.
                }
            }
            catch(Exception ex)
            {
                // Launch dialog box with error here.
            }
        }

        public void StartVideoStream(VlcControl ROVStream)
        {
            ////ROVStream.MediaPlayer.VlcLibDirectoryNeeded += OnVlcControlNeedsLibDirectory;
            //ROVStream.MediaPlayer.EndInit();
            ////ROVStream.MediaPlayer.Play(new Uri("url of your stream"));
            //ROVStream.MediaPlayer.Play(ROVVideo);
        }

        //private void OnVlcControlNeedsLibDirectory(object sender, VlcLibDirectoryNeededEventArgs e)
        //{
        //    // path to VLC installation (don't hardcode in real usage, that is just for example)
        //    e.VlcLibDirectory = new DirectoryInfo(@"C:\Program Files (x86)\VideoLAN\VLC\");
        //}

        private void RecalculateMotors()
        {
            // Super rough calculations of how to handle turning here.
            // This method is very prone to bugs at the moment by overflowing the motor amount.
            // 4-17-16
            // This can most likely be simplified to remove the conditional statement, if more complicated calculations are not added in.

            if (TurningAngle >= 0)
            {
                // Turn angle up to 100, go right, which means engage left faster than right.
                block.LeftHorizontal = ForwardSpeed + TurningAngle;
                block.RightHorizontal = ForwardSpeed - TurningAngle;
            }
            else
            {
                // Turn angle up to -100, go left, which means engage right faster than left.
                block.LeftHorizontal = ForwardSpeed + TurningAngle;
                block.RightHorizontal = ForwardSpeed - TurningAngle;
            }
            block.Vertical = VerticalSpeed;
        }

        private void MovementController(Object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.W:
                    ForwardSpeed += 10;
                    break;
                case Key.A:
                    TurningAngle += -10;
                    break;
                case Key.S:
                    ForwardSpeed += -10;
                    break;
                case Key.D:
                    TurningAngle += 10;
                    break;
                case Key.LeftShift:
                    VerticalSpeed += 10;
                    break;
                case Key.LeftCtrl:
                    VerticalSpeed += -10;
                    break;
                case Key.Space:
                    STOPExecute();
                    break;
                case Key.L:
                    Lights = !Lights;
                    break;
                default:
                    break;
            }
        }

        private void PrepControlBlock()
        {
            RecalculateMotors();
            block.lights = Lights;
            block.status = Status.Good;
        }
    }
}
