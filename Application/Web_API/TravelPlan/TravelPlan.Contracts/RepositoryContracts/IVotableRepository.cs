﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Contracts.RepositoryContracts
{
    public interface IVotableRepository : IRepositoryBase<Votable>
    {
        Task<Votable> GetVotableWithVotes(int id);
    }
}
