﻿using MetalcutWeb.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetalcutWeb.DAL.Repository
{
    public interface IServiceRepository : IRepository<ServiceEntity>
    {
        Task UpdateService(ServiceEntity serviceEntity);
    }
}
