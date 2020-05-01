using IS.Domain;
using IS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IS.UI.Interface;

namespace IS.UI.Service
{
    public class SupplierService: IDataStore<Supplier>
    {
        private readonly Context context;
        public SupplierService(Context _context)
        {
            context = _context;
        }
        public async Task<IEnumerable<Supplier>> GetItemsAsync(bool forceRefresh = false)
        {
            return await context.Suppliers.Include(x => x.RawMaterials).ThenInclude(x => x.Material).ToListAsync();
        }
        public async Task<bool> DeleteItemAsync(int _id)
        {
            var item = await context.Suppliers.SingleAsync(x => x.ID == _id);
            context.Entry<Supplier>(item).State = EntityState.Detached;
            context.Suppliers.Remove(item);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> AddOrUpdateItemAsync(Supplier _item)
        {
            if (!IsSupplierRecordValid(_item))
                return false;
            if (_item.ID == 0)
                return await AddItemAsync(_item);
            return await UpdateItemAsync(_item);
        }
        public async Task<bool> UpdateItemAsync(Supplier _item)
        {
            var local = context.Set<Supplier>()
                .Local
                .FirstOrDefault(f => f.ID == _item.ID);
            if (local != null)
                context.Entry(local).State = EntityState.Detached;
            context.Entry(_item).State = EntityState.Modified;
            context.Update(_item);
//            var item = await context.Suppliers.Include(x=>x.RawMaterials).ThenInclude(x=>x.Material).SingleAsync(x => x.ID == _item.ID);
//            context.Entry<Supplier>(item).State = EntityState.Detached;
//            context.Update(_item);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> AddItemAsync(Supplier _record)
        {
            try
            {
                await context.AddAsync(_record);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public  async Task<Supplier> GetItemAsync(int _id)
        {
           return await context.Suppliers.Include(x => x.RawMaterials).ThenInclude(x => x.Material).SingleOrDefaultAsync(x => x.ID == _id);
        }
        private bool IsSupplierRecordValid(Supplier _record) => _record.Validate();
    }
}
