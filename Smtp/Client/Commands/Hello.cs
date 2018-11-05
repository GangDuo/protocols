using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smtp.Client.Commands
{
    class Hello : SmtpCommandBase
    {
        private string Domain = System.Net.Dns.GetHostName();
        private string HelloCommand
        {
            get
            {
                return String.Format("EHLO {0}", Domain);
            }
        }

        public Hello(SmtpRequest req)
        {
            Request​ = req;
        }

        // TODO: 拡張メソッドへまとめる
        public static void ExecHello(SmtpRequest req)
        {
            new Hello(req).Run();
        }

        public override void Run()
        {
            // EHLOの送信
            WriteLineToRequestStream​(HelloCommand);
            var rstr = Request​.GetResponse();
            if (rstr.StartsWith("2") == false)
            {
                throw new Exception("エラー:" + rstr);
            }
        }
    }
}
