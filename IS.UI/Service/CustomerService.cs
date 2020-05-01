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
    public class CustomerService : IDataStore<Customer>
    {
        private readonly Context context;
        public CustomerService(Context _context)
        {
            context = _context;
        }
        public async Task<IEnumerable<Customer>> GetItemsAsync(bool forceRefresh = false)
        {
            return await context.Customers.Include(x => x.Orders)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Product)
                .Include(x => x.Purchased)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Product).ToListAsync();
        }
        public async Task<bool> DeleteItemAsync(int _id)
        {
            var item = await context.Customers.Include(x => x.Orders)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Product)
                .Include(x => x.Purchased)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Product).SingleAsync(x => x.ID == _id);
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
            //UpdateInsides(_item);
            context.Update(_item);
            return await context.SaveChangesAsync() > 0;
        }
        public void UpdateInsides(Customer _item)
        {
            //var id = _item.Orders;
            //var local = context.Set<ProductForCustomer>()
            //    .Local
            //    .FirstOrDefault(f => f.ID == _item.Orders.SingleOrDefault(x=>x.ID==_));
            //if (local != null)
            //    context.Entry(local).State = EntityState.Detached;
            //context.Entry(_item).State = EntityState.Modified;
        }
        public async Task<Customer> GetItemAsync(int _id)
        {
            return await context.Customers.Include(x => x.Orders)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Product)
                .Include(x => x.Purchased)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Product).SingleOrDefaultAsync(x => x.ID == _id);
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
