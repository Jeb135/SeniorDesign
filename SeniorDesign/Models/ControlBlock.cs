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
        
    }
}
