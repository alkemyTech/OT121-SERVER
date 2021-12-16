using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Entities
{
    public class MailSettings
    {
        public string ApiKey { get; set; }

        public string SenderMail { get; set; }

        public string SenderName { get; set; }
    }
}
