using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeniorDesign.Models
{
    class ControlBlock
    {
        public string Direction;
        public int Speed;
        public string Special;

        public ControlBlock()
        {
            this.Direction = "Forward";
            this.Speed = 0;
            this.Special = "none";
        }
    }
}
