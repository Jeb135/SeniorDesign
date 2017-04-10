using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorDesign.Models
{
    class ControlBlock
    {
        private Status _status;
        public Status status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        private bool _lights;
        public bool lights
        {
            get
            {
                return _lights;
            }
            set
            {
                _lights = value;
            }
        }

        // Movement properties. Talk to Peyton to figure out how we plan to do this.
        private int _LeftHorizontal;
        public int LeftHorizontal
        {
            get
            {
                return _LeftHorizontal;
            }
            set
            {
                if (value > 100) _LeftHorizontal = 100;
                else if (value < -100) _LeftHorizontal = -100;
                else _LeftHorizontal = value;
            }
        }

        private int _RightHorizontal;
        public int RightHorizontal
        {
            get
            {
                return _RightHorizontal;
            }
            set
            {
                if (value > 100) _RightHorizontal = 100;
                else if (value < -100) _RightHorizontal = -100;
                else _RightHorizontal = value;
            }
        }

        private int _Vertical;
        public int Vertical
        {
            get
            {
                return _Vertical;
            }
            set
            {
                if (value > 100) _Vertical = 100;
                else if (value < -100) _Vertical = -100;
                else _Vertical = value;
            }
        }

    }
}
