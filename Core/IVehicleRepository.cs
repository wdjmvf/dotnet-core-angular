using System.Collections.Generic;
using System.Threading.Tasks;
using vega.Core.Models;
using vega.Models;

namespace vega.Core
{
    public interface IVehicleRepository
    {
         Task<Vehicle> GetVehicle(int id, bool includeRelated = true);
         Task<QueryResult<Vehicle>> GetVehicles(VehicleQuery filter);
         void Add(Vehicle Vehicle);
         void Remove(Vehicle Vehicle);
         
    }
}