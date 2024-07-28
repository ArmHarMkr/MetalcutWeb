using MetalcutWeb.DAL.Data;
using MetalcutWeb.DAL.Repository;
using MetalcutWeb.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetalcutWeb.DAL.Implementations
{
    public class ServiceRepository : Repository<ServiceEntity>, IServiceRepository
    {
        private readonly AppDbContext Context;
        public ServiceRepository(AppDbContext db) : base(db)
        {
            Context = db;
        }

        public async Task UpdateService(ServiceEntity serviceEntity)
        {
            var updatingService = await Context.Services.FirstOrDefaultAsync(x => x.Id == serviceEntity.Id);
            if (updatingService != null)
            {
                updatingService.Name = serviceEntity.Name;
                updatingService.Description = serviceEntity.Description;
                updatingService.StartRangeAmount = serviceEntity.StartRangeAmount;
                updatingService.EndRangeAmount = serviceEntity.EndRangeAmount;
            }
        }
    }
}