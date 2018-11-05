using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smtp.Client.Commands.Sasl
{
    abstract class SaslMechanism : SmtpCommandBase
    {
        public string User { get; set; }
        public string Password { get; set; }

        protected string GetBase64String(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return String.Empty;
            }
            var encoding = Encoding.GetEncoding(50220);
            //var encoding = Encoding.GetEncoding("ISO-2022-JP");
            return Convert.ToBase64String(encoding.GetBytes(s));
        }
    }
}
