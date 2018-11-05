using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smtp.Client.Commands.Sasl
{
    class CramMd5 : SaslMechanism
    {
        private string Challenge;

        public CramMd5(SmtpRequest req)
        {
            Request​ = req;
        }

        private static string AsHexString(byte[] buf)
        {
            return BitConverter.ToString(buf).Replace("-", "").ToLower();
        }

        private byte[] DecodeChallenge()
        {
            Debug.Assert(!String.IsNullOrEmpty(Challenge));
            return Convert.FromBase64String(Challenge);
        }

        private byte[] ComputeHMACMD5HashByPassword(byte[] buf)
        {
            var hmacMd5 = new System.Security.Cryptography.HMACMD5(Encoding.UTF8.GetBytes(Password));
            byte[] encCha = hmacMd5.ComputeHash(buf);
            hmacMd5.Clear();
            return encCha;
        }

        private string CreateCramMd5ResponseString()
        {
            byte[] decCha = DecodeChallenge();
            byte[] encCha = ComputeHMACMD5HashByPassword(decCha);
            string hexCha = User + " " + AsHexString(encCha);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(hexCha));
        }

        public override void Run()
        {
            WriteLineToRequestStream​("AUTH CRAM-MD5");
            var rstr = Request​.GetResponse();
            if (rstr.StartsWith("334") == false)
            {
                if (rstr.StartsWith("502"))
                    //認証の必要なし
                    return;
                throw new Exception("エラー:" + rstr);
            }

            Challenge = rstr.Substring(4);
            WriteLineToRequestStream​(CreateCramMd5ResponseString());
            rstr = Request​.GetResponse();
            if (rstr.StartsWith("235") == false)
                throw new Exception("エラー:" + rstr);
        }
    }
}
