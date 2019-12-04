using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTutor.ServerApi.ViewModels
{
    public sealed class SimpleUserResponse
    {
        public int Id { get; set; }

        public string FullName { get; set; }
        
        public string Email { get; set; }
    }
}
