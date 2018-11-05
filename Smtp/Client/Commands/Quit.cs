using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smtp.Client.Commands
{
    class Quit : SmtpCommandBase
    {
        private static readonly string QuitCommand = "QUIT";

        public Quit(SmtpRequest req)
        {
            Request​ = req;
        }

        public override void Run()
        {
            // QUITの送信
            WriteLineToRequestStream​(QuitCommand);
            var rstr = Request​.GetResponse();
            if (rstr.StartsWith("2") == false)
            {
                throw new Exception("エラー:" + rstr);
            }
        }
    }
}
