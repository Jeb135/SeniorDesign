using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorDesign.Models
{
    class UDPStatus
    {
        // This class is useless at the moment.
        // Video bytes
        
        // Depth
        private int _Depth;
        public int Depth
        {
            get
            {
                return _Depth;
            }
            set
            {
                _Depth = value;
            }
        }

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

    }
}
