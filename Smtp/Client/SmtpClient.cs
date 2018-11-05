using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smtp.Client
{
    public class SmtpClient
    {
        private Config Profile;

        public SmtpClient(Config profile)
        {
            Profile = profile;
        }

        public void Send(MailMessage msg)
        {
            int port = 465;
            using (var req = new SmtpRequest(Profile.Server, port))
            {
                var tran = req.BeginTransaction();

                var cmds = new List<Commands.SmtpCommandBase>()
                {
                    new Commands.Hello(req),
                    // TODO: LOGIN CRAMMD5 に変更可能
                    new Commands.Sasl.Plain(req) { User = Profile.User, Password = Profile.Pass },
                    new Commands.Mail(req, Profile.EmailAdr),
                    new Commands.Rcpt(req, msg.Rcpts()),
                    new Commands.Data(req, Profile, msg),
                    new Commands.Quit(req)
                };
                foreach (var cmd in cmds)
                {
                    cmd.Run();
                }

                tran.Complete();
            }
        }
    }
}
