using System;
using System.Collections.Generic;
using System.Text;

namespace eTutor.Core.Models.Configuration
{
    public sealed class SMTPConfiguration
    {

        public string Server { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public int Port { get; set; }
    }
}
