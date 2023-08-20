using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatGPTWebAPI.SharedObjects
{
    public class Queries
    {
        public int QueryID;
        public string Input;
        public string Output;
        public int UserID;
        public DateTime dt_Created;
    }
}