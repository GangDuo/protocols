using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smtp.Client
{
    public class Config
    {
        private string emailAdr { get; set; }

        public bool UseSsl { get; set; } = true;
        public string Server { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        public string EmailAdr
        {
            get { return emailAdr; }
            set { emailAdr = value.Trim(); }
        }
    }
}
