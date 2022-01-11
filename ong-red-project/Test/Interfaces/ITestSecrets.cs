using OngProject.Core.Entities;
using OngProject.Core.Helper.Common;

namespace Test.Interfaces
{
    public interface ITestSecrets
    {
        MailConstants MailConstants { get; set; }
        MailSettings MailSettings { get; set; }
        JWTSettings JwtSettings { get; set; }
        AWSSettings AwsSettings { get; set; }
    }
}