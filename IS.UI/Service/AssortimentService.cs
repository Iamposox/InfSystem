using IS.Domain;
using IS.Domain.Model;
using IS.UI.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS.UI.Service
{
    public class AssortimentService:IDataStore<Assortment>
    {
        readonly Context context;
        public AssortimentService(Context _context)
        {
            context = _context;
        }
        public async Task<IEnumerable<Assortment>> GetItemsAsync(bool forceRefresh = false) => await context.Assortments.ToListAsync();
        public async Task<bool> AddOrUpdateItemAsync(Assortment _item)
        {
            if (_item.Validate())
            {
                if (_item.ID == 0)
                    return await AddItemAsync(_item);
                return await UpdateItemAsync(_item);
            }
            return false;
        }
        public async Task<bool> UpdateItemAsync(Assortment _item)
        {
            var local = context.Set<Assortment>()
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
        public async Task<Assortment> GetItemAsync(int _id)
        {
            return await context.Assortments.SingleOrDefaultAsync(x => x.ID == _id);
        }
        public async Task<bool> AddItemAsync(Assortment _item)
        {
            try
            {
                await context.AddAsync(_item);
                await context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> DeleteItemAsync(int _id)
        {
            var item = await context.Assortments.SingleOrDefaultAsync(x => x.ID == _id);
            var local = context.Set<Assortment>()
                        .Local
                        .FirstOrDefault(f => f.ID == item.ID);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(item).State = EntityState.Modified;
            context.Remove(item);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
