﻿using MetalcutWeb.DAL.Data;
using MetalcutWeb.DAL.Repository;
using MetalcutWeb.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetalcutWeb.DAL.Implementations
{
    public class StorageProductRepository : Repository<StorageProductEntity>, IStorageProductRepository
    {
        private readonly AppDbContext _db;

        public StorageProductRepository(AppDbContext db)
            :base(db)
        {
            _db = db;
        }
    }
}
