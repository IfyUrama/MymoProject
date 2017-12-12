using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessageManager
{
    public class MymoMessage
    {
        public int MessageID { get; set; }
        public Nullable<System.DateTime> ArrivalTime { get; set; }
        public string ReceivedMessage { get; set; }
    }
}
