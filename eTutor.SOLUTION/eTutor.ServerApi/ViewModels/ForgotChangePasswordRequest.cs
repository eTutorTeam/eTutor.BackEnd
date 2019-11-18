using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTutor.ServerApi.ViewModels
{
    public sealed class ForgotChangePasswordRequest
    {
        public int UserId { get; set; }

        public string NewPassword { get; set; }

        public string ChangePasswordToken { get; set; }
    }
}
