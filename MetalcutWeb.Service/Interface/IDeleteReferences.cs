using MetalcutWeb.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetalcutWeb.Service.Interface
{
    public interface IDeleteReferences<T>
    {
        Task DeleteReferences(T entity);
    }
}
