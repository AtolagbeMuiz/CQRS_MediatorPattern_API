using CQRS_MediatorPattern.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS_MediatorPattern.Context
{
    public interface IApplicationDBContext
    {
        DbSet<Product> Product { get; set; }
        Task<int> SaveChangesAsync();
    }
}
