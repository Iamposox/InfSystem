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
    public class ProductService:IDataStore<Product>
    {
        private readonly Context context;
        public ProductService(Context _context)
        {
            context = _context;
        }
        public async Task<bool> AddOrUpdateItemAsync(Product _item)
        {
            if (_item.Validate())
            {
                if (_item.ID == 0)
                    return await AddItemAsync(_item);
                return await UpdateItemAsync(_item);
            }
            return false;
        }
        public async Task<bool> UpdateItemAsync(Product _item)
        {
            var entry = context.Set<Product>()
                         .Local
                         .FirstOrDefault(f => f.ID == _item.ID);
            if (entry != null)
            {
                context.Entry(entry).State = EntityState.Detached;
            }
            context.Entry(_item).State = EntityState.Modified;
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> AddItemAsync(Product _item)
        {
            try
            {
                await context.AddAsync(_item);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public async Task<Product> GetItemAsync(int _id) => await context.Products.Include(x => x.RequeredMaterials).SingleOrDefaultAsync(x => x.ID == _id);
        public async Task<bool> DeleteItemAsync(int _id)
        {
            var item = await context.Products.Include(x => x.RequeredMaterials).SingleOrDefaultAsync(x => x.ID == _id);
            var local = context.Set<Product>()
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
        public async Task<IEnumerable<Product>> GetItemsAsync(bool forceRefresh = false) => await context.Products.Include(x=>x.RequeredMaterials).ToListAsync();
    }
}
