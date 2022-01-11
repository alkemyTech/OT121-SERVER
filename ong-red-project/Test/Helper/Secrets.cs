using OngProject.Core.Entities;
using OngProject.Core.Helper.Common;

namespace Test.Helper
{
    public class Secrets
    {
        private MailConstants MailConstants { get; set; }
        private MailSettings MailSettings { get; set; }
        private JWTSettings JwtSettings { get; set; }
        private AWSSettings AwsSettings { get; set; }
    }
}