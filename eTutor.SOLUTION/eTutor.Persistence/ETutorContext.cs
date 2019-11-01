using System;
using Microsoft.EntityFrameworkCore;

namespace eTutor.Persistence
{
    public class ETutorContext : DbContext
    {
        public ETutorContext(DbContextOptions options) : base(options)
        {

        }
    }
}
