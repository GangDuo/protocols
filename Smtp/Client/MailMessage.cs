using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Smtp.Client
{
    public class MailMessage : IDisposable
    {
        public string[] To { get; set; }
        public string[] Cc { get; set; }
        public string[] Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Collection<Attachment> Attachments { get; private set; }
        //public NameValueCollection Headers { get; private set; }

        public MailMessage()
        {
            this.Attachments = new Collection<Attachment>();
        }

        public void Dispose()
        {
            this.Attachments.Clear();
        }

        public IEnumerable<string> Rcpts()
        {
            var rcpts = new HashSet<string>(To);
            if (null != Cc)
            {
                foreach (var item in Cc)
                {
                    rcpts.Add(item);
                }
            }
            if (null != Bcc)
            {
                foreach (var item in Bcc)
                {
                    rcpts.Add(item);
                }
            }
            return rcpts;
        }
    }
}
