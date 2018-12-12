using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    public interface ISend
    {
        void SendMessage(string msg);
    }
}
