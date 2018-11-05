using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smtp.Client.Commands.Sasl
{
    class Login : SaslMechanism
    {
        public Login(SmtpRequest req)
        {
            Request​ = req;
        }

        public override void Run()
        {
            WriteLineToRequestStream​("AUTH LOGIN");
            var rstr = Request​.GetResponse();
            if (rstr.StartsWith("334") == false)
            {
                if (rstr.StartsWith("502"))
                    //認証の必要なし
                    return;
                //throw new SmtpException(_receivedMessage);
                throw new Exception("エラー:" + rstr);
            }

            WriteLineToRequestStream​(GetBase64String(User));
            rstr = Request​.GetResponse();
            if (rstr.StartsWith("334") == false)
                throw new Exception("エラー:" + rstr);

            WriteLineToRequestStream​(GetBase64String(Password));
            if (rstr.StartsWith("235") == false)
                throw new Exception("エラー:" + rstr);
        }
    }
}
