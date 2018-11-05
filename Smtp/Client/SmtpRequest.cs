using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Smtp.Client
{
    class SmtpRequest : IDisposable
    {
        private static readonly string Charset = "ISO-2022-JP";
        //        private System.Net.Security.SslStream stream;
        private Stream SmtpStream;
        private System.Net.Sockets.TcpClient Tcp;
        private string Hostname;
        private int Port;

        public SmtpRequest(string hostname, int port)
        {
            Hostname = hostname;
            Port = port;
        }

        public void Dispose()
        {
            if (SmtpStream != null)
            {
                SmtpStream.Close();
            }
            if (Tcp != null)
            {
                Tcp.Close();
            }
        }

        public bool TryGet​Request​Stream()
        {
            if (SmtpStream == null)
            {
                Tcp = new System.Net.Sockets.TcpClient();
                Tcp.Connect(Hostname, Port);

                var stream = new SslStream(Tcp.GetStream()) { ReadTimeout = 30000 };
                stream.AuthenticateAsClient(Hostname);
                SmtpStream = stream;
            }
            return SmtpStream != null;
        }

        public Stream Get​Request​Stream()
        {
            TryGet​Request​Stream();
            return SmtpStream;
        }

        public String GetResponse()
        {
            if (!TryGet​Request​Stream())
            {
                throw new NullReferenceException();
            }
            // SMTPサーバからのレスポンス受信
            string rstr = "";
            byte[] rdat = new Byte[] { };
            Array.Resize<byte>(ref rdat, 1024 * 1024);
            int l = SmtpStream.Read(rdat, 0, rdat.Length);
            if (l > 0)
            {
                Array.Resize<byte>(ref rdat, l);
                rstr = Encoding.GetEncoding(Charset).GetString(rdat);
            }
            else
            {
                throw new Exception("Read Error.");
            }
            Debug.WriteLine("<--- " + rstr);
            // TODO: SMTPサーバーからの応答をクラス化
            var res = rstr.Split(new string[] { "" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(a => new
                {
                    ReplyCode = a.Substring(0, 3),
                    ReplyText = a.Substring(3)
                });
            return rstr;
        }

        public Commands.Transaction BeginTransaction()
        {
            var rstr = GetResponse();
            if (rstr.StartsWith("2") == false)
            {
                throw new Exception("エラー:" + rstr);
            }
            return new Commands.Transaction();
        }
    }
}
