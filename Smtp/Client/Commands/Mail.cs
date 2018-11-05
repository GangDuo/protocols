using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smtp.Client.Commands
{
    class Mail : SmtpCommandBase
    {
        private string ReversePath;
        private string MailCommand
        {
            get
            {
                return String.Format("MAIL FROM:<{0}>", ReversePath);
            }
        }

        public Mail(SmtpRequest req, string adr)
        {
            Request​ = req;
            ReversePath = new System.Net.Mail.MailAddress(adr).Address;
        }

        public override void Run()
        {
            // MAIL FROMの送信
            WriteLineToRequestStream​(MailCommand);
            var rstr = Request​.GetResponse();
            if (rstr.StartsWith("2") == false)
            {
                throw new Exception("エラー:" + rstr);
            }
        }
    }
}
