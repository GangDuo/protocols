using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smtp.Client.Commands
{
    class Data : SmtpCommandBase
    {
        private static readonly string DataCommand= "DATA";
        private static readonly string EndOfMailData = "\r\n" + "." + "\r\n";
        private Config _profile;
        private MailMessage msg;

        public Data(SmtpRequest req, Config profile, MailMessage m)
        {
            Request​ = req;
            _profile = profile;
            msg = m;
        }

        public override void Run()
        {
            {
                // DATAの送信
                WriteLineToRequestStream​(DataCommand);
                var rstr = Request​.GetResponse();
                if (rstr.StartsWith("2") == false && rstr.StartsWith("3") == false)
                {
                    throw new Exception("エラー:" + rstr);
                }
            }

            {
                // TODO: べた書きしました
                // 本文DATAの作成(From)
                String data = "";
                String frd = new System.Net.Mail.MailAddress(_profile.EmailAdr).DisplayName;
                String fa = new System.Net.Mail.MailAddress(_profile.EmailAdr).Address;
                data += "From: " + GenerateMimeEncodedWord(frd, Charset) + "<" + fa + ">" + "\r\n";

                // 本文DATAの作成(To)
                var to = new HashSet<string>();
                foreach (var item in msg.To)
                {
                    String ta = new System.Net.Mail.MailAddress(item.Trim()).Address;
                    String tod = new System.Net.Mail.MailAddress(item).DisplayName;
                    to.Add(GenerateMimeEncodedWord(tod, Charset) + "<" + ta + ">");
                }
                data += "To: " + String.Join(",", to) + "\r\n";

                // 本文DATAの作成(Cc)
                if (null != msg.Cc)
                {
                    var cc = new HashSet<string>();
                    foreach (var item in msg.Cc)
                    {
                        String ta = new System.Net.Mail.MailAddress(item.Trim()).Address;
                        String tod = new System.Net.Mail.MailAddress(item).DisplayName;
                        cc.Add(GenerateMimeEncodedWord(tod, Charset) + "<" + ta + ">");
                    }
                    data += "Cc: " + String.Join(",", cc) + "\r\n";
                }

                // 本文DATAの作成(Subject)
                data += "Subject: " + GenerateMimeEncodedWord(msg.Subject, Charset) + "\r\n";

                // 本文DATAの作成(MIME-Version)
                data += "MIME-Version: 1.0" + "\r\n";

                if (0 == msg.Attachments.Count)
                {
                    // 本文DATAの作成(Content-Type) -- 添付ファイル無しの時 --
                    data += "Content-Type: text/plain; charset=\"" + Charset + "\"" + "\r\n";

                    // 本文DATAの作成(Content-Transfer-Encoding)
                    data += "Content-Transfer-Encoding: 7bit" + "\r\n" + "\r\n";

                    // 本文DATAの作成(本文)
                    data += msg.Body + "\r\n";
                }
                else
                {
                    var boundary = String.Format("_{0}_MULTIPART_MIXED_", Guid.NewGuid().ToString("N").ToUpper());

                    // (参考)添付ファイル有りの時は(Content-Type: multipart/mixed; boundary="XXX")
                    data += "Content-Type: multipart/mixed; boundary=\"" + boundary + "\"" + "\r\n";
                    data += "Content-Transfer-Encoding: 7bit" + "\r\n" + "\r\n";
                    data += "--" + boundary + "\r\n";
                    data += "Content-Type: text/plain; charset=\"" + Charset + "\"" + "\r\n";
                    data += "Content-Transfer-Encoding: 7bit" + "\r\n" + "\r\n";
                    data += msg.Body + "\r\n";
                    data += "\r\n";

                    foreach (var attachment in msg.Attachments)
                    {
                        //attachment.NameEncoding = Encoding.GetEncoding("iso-2022-jp");
                        //attachment.ContentDisposition.FileName = attachment.Name;
                        var fileName = GenerateMimeEncodedWord(attachment.Name, Charset);

                        data += "--" + boundary + "\r\n";
                        data += "Content-Type: " + attachment.ContentType.MediaType + ";\r\n";
                        data += " name=\"" + fileName + "\"\r\n";
                        data += "Content-Disposition: " + attachment.ContentDisposition.DispositionType + ";\r\n";
                        data += " filename=\"" + fileName + "\"\r\n";
                        data += "Content-Transfer-Encoding: base64" + "\r\n";
                        data += "\r\n";

                        string base64String = GetBase64String(attachment.ContentStream);
                        for (int i = 0; i < base64String.Length;)
                        {
                            int nextchunk = 100;
                            if (base64String.Length - (i + nextchunk) < 0)
                            {
                                nextchunk = base64String.Length - i;
                            }
                            data += (base64String.Substring(i, nextchunk));
                            data += ("\r\n");
                            i += nextchunk;
                        }
                    }

                    data += "--" + boundary + "--\r\n";
                }

                // 本文DATAの作成(.を..に変換)
                data = data.Replace("\r\n" + "." + "\r\n", "\r\n" + ".." + "\r\n");

                // 本文DATAの作成(.を付加)
                data += EndOfMailData;

                // 本文DATAの送信
                WriteToRequestStream(data, Charset);
                var rstr = Request​.GetResponse();
                if (rstr.StartsWith("2") == false)
                {
                    throw new Exception("エラー:" + rstr);
                }
            }
        }

        private static string GenerateMimeEncodedWord(string str, string charset)
        {
            if (String.IsNullOrEmpty(str))
            {
                return String.Empty;
            }
            var encodedText = Convert.ToBase64String(Encoding.GetEncoding(charset).GetBytes(str));
            var encodedWord = String.Format("=?{0}?B?{1}?=", charset, encodedText);
            if(encodedWord.Length > 75)
            {
                throw new Exception("Too long");
            }
            return encodedWord;
        }

        private static String GetBase64String(System.IO.Stream inFile)
        {
            byte[] binaryData = new Byte[inFile.Length];
            int readBytes = inFile.Read(binaryData, 0, (int)inFile.Length);
            return Convert.ToBase64String(binaryData, 0, binaryData.Length);
        }
    }
}
