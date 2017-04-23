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
                RecalculateControls();
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
                RecalculateControls();
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
                RecalculateControls();
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

        private bool _AlreadySentSpecial;
        public bool AlreadySentSpecial
        {
            get
            {
                return _AlreadySentSpecial;
            }
            set
            {
                _AlreadySentSpecial = value;
            }
        }

        //private string _streamUrl;
        //public string streamUrl
        //{
        //    get
        //    {
        //        return _streamUrl;
        //    }
        //    set
        //    {
        //        _streamUrl = value;
        //    }
        //}

        //private Uri _ROVVideo;
        //public Uri ROVVideo
        //{
        //    get
        //    {
        //        if(_ROVVideo == null) { _ROVVideo = new Uri(streamUrl); }
        //        return _ROVVideo;
        //    }
        //    set
        //    {
        //        _ROVVideo = value;
        //        //OnPropertyChanged("something");
        //    }
        //}
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

        private ICommand _DiveCommand;
        public ICommand DiveCommand
        {
            get
            {
                if(_DiveCommand == null)
                {
                    _DiveCommand = new RelayCommand(DiveExecute, CanExecuteDiveCommand);
                }
                return _DiveCommand;
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

        private bool CanExecuteDiveCommand()
        {
            return true;
        }

        public void ConnectWirelessExecute()
        {
            // Attempt to make a connection to the ROV here.
            ConnectROV();
        }

        public void ReturnToSurfaceExecute()
        {
            // The API Does not support this, so do nothing.
            // Tell the ROV to return to the surface.
            //block.status = Status.Bad;
            block.Special = "Surface";
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

        public void DiveExecute()
        {
            block.Special = "Dive";
        }


        // Add Commands for other buttons.
        // To be added:
        // ReturnToSurfaceCommand

        #endregion

        public ROVControlsViewModel()
        {
            server = "169.254.250.128";
            port = 3000;
            block = new ControlBlock();
            AlreadySentSpecial = false;
            if (ConnectROV())
            {
                StartDispatchTimer();
            }
            EventManager.RegisterClassHandler(typeof(System.Windows.Controls.Control), System.Windows.Controls.Control.KeyDownEvent, new KeyEventHandler(MovementController), true);
        }

        public bool ConnectROV()
        {
            // Return if connection was successful or not.
            // For testing.
            _SendControlSignal();
            if (!connected)
            {
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
            //SendControlSignalDispatchTimer = new DispatcherTimer();
            //SendControlSignalDispatchTimer.Tick += new EventHandler(SendControlSignal);
            //SendControlSignalDispatchTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            //SendControlSignalDispatchTimer.Start();
        }

        public void SendControlSignal(object sender, EventArgs e)
        {
            _SendControlSignal();
        }

        public void _SendControlSignal()
        {
            try
            {
                string url = "http://" + server + ":" + port + "/";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
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

                using (WebResponse response = request.GetResponse())
                {
                    // Do something with returning result if necessary.
                    string body = "";
                }
            }
            catch (WebException ex)
            {
                // Bad Gateway/No connection/Timeout error would cause this.
            }
            catch (Exception ex)
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

        private void RecalculateControls()
        {
            // Super rough calculations of how to handle turning here.
            // This method is very prone to bugs at the moment by overflowing the motor amount.

            //if (TurningAngle >= 0)
            //{
            //    // Turn angle up to 100, go right, which means engage left faster than right.
            //    block.LeftHorizontal = ForwardSpeed + TurningAngle;
            //    block.RightHorizontal = ForwardSpeed - TurningAngle;
            //}
            //else
            //{
            //    // Turn angle up to -100, go left, which means engage right faster than left.
            //    block.LeftHorizontal = ForwardSpeed + TurningAngle;
            //    block.RightHorizontal = ForwardSpeed - TurningAngle;
            //}
            //block.Vertical = VerticalSpeed;

            // Interpret direction and speed controls into something the API can read correctly.
            if (TurningAngle != 0)
            {
                if(TurningAngle > 0)
                {
                    block.Direction = "Right";
                    block.Speed = ForwardSpeed;
                }
                else
                {
                    block.Direction = "Left";
                    block.Speed = ForwardSpeed;
                }
            }
            else
            {
                if (ForwardSpeed > 0)
                {
                    block.Direction = "Forward";
                    block.Speed = ForwardSpeed;
                }
                else if (ForwardSpeed < 0)
                {
                    block.Direction = "Backward";
                    block.Speed = ForwardSpeed;
                }
                else
                {
                    if (VerticalSpeed != 0)
                    {
                        if (VerticalSpeed > 0)
                        {
                            block.Direction = "Up";
                            block.Speed = 5;
                        }
                        else
                        {
                            block.Direction = "Down";
                            block.Speed = 5;
                        }
                    }
                    else
                    {
                        block.Direction = "None";
                        block.Speed = 0;
                    }

                }
            }

            // block.Special interpretation. flip flop between AlreadySentSpecial so that it is only sent once.
            if (AlreadySentSpecial)
            {
                block.Special = "none";
                AlreadySentSpecial = false;
            }

            if(block.Special != "none")
            {
                AlreadySentSpecial = true;
            }

            // Check to make sure Speed is non-negative.
            if(block.Speed < 0)
            {
                block.Speed = block.Speed * -1;
            }
        }

        private void MovementController(Object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.W:
                    ForwardSpeed += 10;
                    break;
                case Key.A:
                    TurningAngle += -1;
                    break;
                case Key.S:
                    ForwardSpeed += -10;
                    break;
                case Key.D:
                    TurningAngle += 1;
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
    }
}
