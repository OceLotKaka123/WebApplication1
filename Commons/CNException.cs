using System;
using System.Collections.Generic;
using System.Text;

namespace Commons
{
    public class CNException:Exception
    {
        public CNException(string context):base(context)
        { 
        }
        public override string Message => base.Message;
    }
}
