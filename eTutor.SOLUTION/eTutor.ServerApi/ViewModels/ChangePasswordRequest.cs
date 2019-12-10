using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTutor.ServerApi.ViewModels
{
    public sealed class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
