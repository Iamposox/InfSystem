using IS.Domain;
using IS.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using IS.UI.Interface;
using System.Linq;

namespace IS.UI.Service
{
    public class RawMaterialService:IDataStore<RawMaterial>
    {
        private readonly Context context;
        public RawMaterialService(Context _context)
        {
            context = _context;
        }
        public async Task<IEnumerable<RawMaterial>> GetItemsAsync(bool forceRefresh = false) => await context.RawMaterials.ToListAsync();
        public async Task<bool> AddOrUpdateItemAsync(RawMaterial raw)
        {
            if (raw.ID == 0)
                return await AddItemAsync(raw);
            return await UpdateItemAsync(raw);
        }
        public async Task<bool> DeleteItemAsync(int id)
        {
            var raw = await context.RawMaterials.SingleOrDefaultAsync(x => x.ID == id);
            context.Entry<RawMaterial>(raw).State = EntityState.Detached;
            context.Remove(raw);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateItemAsync(RawMaterial _item)
        {
            var local = context.Set<RawMaterial>()
                         .Local
                         .FirstOrDefault(f => f.ID == _item.ID);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(_item).State = EntityState.Modified;
            context.Update(_item);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<RawMaterial> GetItemAsync(int _id)
        {
            return await context.RawMaterials.SingleOrDefaultAsync(x => x.ID == _id);
        }
        public async Task<bool> AddItemAsync(RawMaterial raw)
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
    }
}
