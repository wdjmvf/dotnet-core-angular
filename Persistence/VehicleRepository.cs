using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Core.Models;
using vega.Extensions;
using vega.Models;

namespace vega.Persistence
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VegaDbContext context;

        public VehicleRepository(VegaDbContext context)
        {
            this.context = context;
        }

        public async Task<Vehicle> GetVehicle(int id, bool includeRelated = true){
            if(!includeRelated){
                return await context.Vehicles.FindAsync(id);
            }
            
            return await context.Vehicles
            .Include(v => v.Features)
                .ThenInclude(vf => vf.Feature)
            .Include(v => v.Model)
                .ThenInclude(m => m.Make)
            .SingleOrDefaultAsync(v => v.Id == id);
        }

        public async Task<QueryResult<Vehicle>> GetVehicles(VehicleQuery queryObj){
            var result = new QueryResult<Vehicle>();
            var query = context.Vehicles
                .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                .Include(f => f.Features)
                    .ThenInclude(vf => vf.Feature)
                .AsQueryable();

                if(queryObj.MakeId.HasValue){
                    query = query.Where(v => v.Model.MakeId == queryObj.MakeId);
                }

                var columnMap = new Dictionary<string,Expression<Func<Vehicle,object>>>()
                {
                    ["make"] = v => v.Model.Make.Name,
                    ["model"] = v => v.Model.Name,
                    ["contactName"] = v => v.ContactName,
                    // ["id"] = v => v.Id, //ลบออกเพราะ sort แล้วจะ error เพราะ entity framwork เป็นตัว gen id จะไม่ยอมให้ client sort ได้
                };

                query = query.ApplyOrdering(queryObj, columnMap);
                result.TotalItems = await query.CountAsync();
                query = query.ApplyPaging(queryObj);
                result.Items = await query.ToListAsync();
                return result;
        }

        

        public void Add(Vehicle Vehicle){
            context.Vehicles.Add(Vehicle);
        }

        public void Remove(Vehicle Vehicle){
            context.Remove(Vehicle);
        }
    }
}