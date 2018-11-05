using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smtp.Client.Commands.Sasl
{
    class Plain : SaslMechanism
    {
        public Plain(SmtpRequest req)
        {
            Request​ = req;
        }

        public override void Run()
        {
            WriteLineToRequestStream​("AUTH PLAIN");
            var rstr = Request​.GetResponse();
            if (rstr.StartsWith("502") == false)
            {
                if (rstr.StartsWith("3") == false)
                {
                    throw new Exception("エラー:" + rstr);
                }

                WriteLineToRequestStream​(GetBase64String(String.Format("{0}\0{0}\0{1}", User, Password)));
                rstr = Request​.GetResponse();
                if (rstr.StartsWith("2") == false)
                {
                    throw new Exception("エラー:" + rstr);
                }
            }
        }
    }
}
