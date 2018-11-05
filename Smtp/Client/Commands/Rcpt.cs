using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smtp.Client.Commands
{
    class Rcpt : SmtpCommandBase
    {
        private IEnumerable<string> Rcpts;

        public Rcpt(SmtpRequest req, IEnumerable<string> rcpts)
        {
            Request​ = req;
            Rcpts = rcpts;
        }

        public override void Run()
        {
            Debug.Assert(Rcpts != null);

            // RCPT TOの送信
            foreach (var tadr in Rcpts)
            {
                String ta = new System.Net.Mail.MailAddress(tadr.Trim()).Address;
                WriteLineToRequestStream​(RcptCommand(ta));
                var rstr = Request​.GetResponse();
                if (rstr.StartsWith("2") == false)
                {
                    throw new Exception("エラー:" + rstr);
                }
            }
        }

        private static string RcptCommand(string forwardPath)
        {
            return String.Format("RCPT TO:<{0}>", forwardPath);
        }
    }
}
