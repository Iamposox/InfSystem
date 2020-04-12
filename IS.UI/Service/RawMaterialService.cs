using IS.Domain;
using IS.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace IS.UI.Service
{
    public class RawMaterialService
    {
        private readonly Context context;
        public RawMaterialService(Context _context)
        {
            context = _context;
        }
        public async Task<bool> AddOrUpdateRawMaterials(RawMaterial raw)
        {
            if (raw.ID == 0)
                return await AddNewRawMaterials(raw);
            context.Update(raw);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> RemoveRawMaterial(RawMaterial raw)
        {
            context.Remove(raw);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> AddNewRawMaterials(RawMaterial raw)
        {
            try
            {
                await context.AddAsync(raw);
                await context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }
        public async Task<IEnumerable<RawMaterial>> GetRawMaterials() => await context.RawMaterials.ToListAsync();
    }
}
