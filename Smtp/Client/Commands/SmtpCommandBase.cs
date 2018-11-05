using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smtp.Client.Commands
{
    abstract class SmtpCommandBase
    {
        private static readonly string NewLine = "\r\n";

        protected static readonly string Charset = "ISO-2022-JP";
        protected SmtpRequest Request​;

        public Encoding Request​Encoding { get; set; } = Encoding.GetEncoding(50220);

        abstract public void Run();

        protected void WriteToRequestStream(string req, string cst)
        {
            Debug.Assert(Request​ != null);
            var stream = Request​.Get​Request​Stream();
            Debug.Assert(stream != null);

            // メールサーバへリクエスト送信
            if (req != "")
            {
                Byte[] sdat;
                sdat = Encoding.GetEncoding(cst).GetBytes(req);
                for (int offset = 0, quota = sdat.Length;
                     quota > 0;)
                {
                    int count = Math.Min(512, quota);

                    stream.Write(sdat, offset, count);
                    stream.Flush();

                    offset += count;
                    quota -= count;
                }
            }
        }

        protected void WriteLineToRequestStream​(string req)
        {
            WriteToRequestStream(String.Format("{0}{1}", req, NewLine), Charset);
        }
    }
}
