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
    public class CustomerService:IDataStore<Customer>
    {
        private readonly Context context;
        public CustomerService(Context _context)
        {
            context = _context;
        }
        public async Task<IEnumerable<Customer>> GetItemsAsync(bool forceRefresh = false) 
        {
            return await context.Customers.ToListAsync(); 
        }
        public async Task<bool> DeleteItemAsync(int _id)
        {
            var item = await context.Customers.SingleAsync(x => x.ID == _id);
            context.Entry<Customer>(item).State = EntityState.Detached;
            context.Customers.Remove(item);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> AddOrUpdateItemAsync(Customer _item)
        {
            if (_item.ID == 0)
                return await AddItemAsync(_item);
            return await UpdateItemAsync(_item);
        }
        public async Task<bool> UpdateItemAsync(Customer _item)
        {
            var local = context.Set<Customer>()
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
        public async Task<Customer> GetItemAsync(int _id)
        {
            return await context.Customers.SingleOrDefaultAsync(x => x.ID == _id);
        }
        public async Task<bool> AddItemAsync(Customer customer)
        {
            try
            {
                await context.AddAsync(customer);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
