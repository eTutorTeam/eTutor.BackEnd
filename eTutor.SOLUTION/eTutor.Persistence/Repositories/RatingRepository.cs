using System;
using System.Collections.Generic;
using System.Text;
using eTutor.Core.Models;
using eTutor.Core.Repositories;

namespace eTutor.Persistence.Repositories
{
    public sealed class RatingRepository: BaseRepository<Rating>, IRatingRepository
    {
        public RatingRepository(ETutorContext context) : base(context)
        {

        }

    }
}
