using IS.Domain;
using IS.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IS.UI.Service
{
    public class CustomerService
    {
        private readonly Context context;
        public CustomerService(Context _context)
        {
            context = _context;
        }
        public async Task<IEnumerable<Customer>> GetCustomers() 
        {
            return await context.Customers.ToListAsync(); 
        }
        public async Task<bool> RemoveCustomers(Customer customer)
        {
            context.Remove(customer);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> AddOrUpdate(Customer customer)
        {
            if (customer.ID == 0)
                return await AddNewCustomer(customer);
            context.Update(customer);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> AddNewCustomer(Customer customer)
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
