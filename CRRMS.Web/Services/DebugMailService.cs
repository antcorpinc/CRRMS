using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CRRMS.Web.Services
{
    public class DebugMailService : IMailService
    {
        public void SendEmail(string to, string from, string subject, string body)
        {
            Debug.WriteLine($"Sending Mail:To:{to} From:{from} Subject:{subject}");
        }
    }
}
