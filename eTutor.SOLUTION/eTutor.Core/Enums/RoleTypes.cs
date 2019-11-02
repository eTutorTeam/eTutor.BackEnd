using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace eTutor.Core.Enums
{
    public enum RoleTypes
    {
        [Description("admin")]
        Admin = 1,

        [Description("tutor")]
        Tutor = 2,

        [Description("student")]
        Student = 3,

        [Description("parent")]
        Parent = 4
    }
}
