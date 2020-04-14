using IS.Domain;
using IS.Domain.Model;
using IS.UI.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IS.UI.Service
{
    public class RoleService: IDataStore<Role>
    {
        readonly Context context;
        public RoleService(Context _context)
        {
            context = _context;
        }
        public async Task<IEnumerable<Role>> GetItemsAsync(bool forceRefresh = false) => await context.Roles.ToListAsync();
        public async Task<bool> AddOrUpdateItemAsync(Role role)
        {
            if (role.ID == 0)
                return await AddItemAsync(role);
            return await UpdateItemAsync(role);
        }
        public async Task<bool> AddItemAsync(Role role)
        {
            context.Add(role);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateItemAsync(Role role)
        {
            context.Update(role);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteItemAsync(int id)
        {
            var item = await context.Roles.SingleOrDefaultAsync(x => x.ID == id);
            context.Entry<Role>(item).State = EntityState.Detached;
            context.Remove(item);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<Role> GetItemAsync(int id)
        {
            return await context.Roles.SingleOrDefaultAsync(x => x.ID == id);
        }
    }
}
