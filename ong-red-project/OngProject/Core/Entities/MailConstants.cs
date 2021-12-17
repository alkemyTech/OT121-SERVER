using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Entities
{
    public class MailConstants
    {
		public string PathTemplate { get; set; }
		public string ReplaceMailTitle { get; set; }
		public string ReplaceMailBody { get; set; }
		public string ReplaceMailContact { get; set; }
		public string ReplaceMailConfirm { get; set; }
		public string TitleMailConfirm { get; set; }
		public string WelcomeMailBody { get; set; }
		public string MailONG { get; set; }
	}
}
