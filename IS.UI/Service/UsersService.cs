﻿using IS.Domain;
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
    public class UsersService: IDataStore<User>
    {
        readonly Context context;
        public UsersService(Context _context)
        {
            context = _context;
        }
        public async Task<IEnumerable<User>> GetItemsAsync(bool forceRefresh = false) => await context.Users.Include(x=>x.Role).ToListAsync();
        public async Task<bool> AddOrUpdateItemAsync(User user)
        {
            if (user.ID == 0)
                return await AddItemAsync(user);
            return await UpdateItemAsync(user);
        }
        public async Task<bool> AddItemAsync(User user)
        {
            context.Add(user);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateItemAsync(User user)
        {
            var local = context.Set<User>()
                         .Local
                         .FirstOrDefault(f => f.ID == user.ID);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(user).State = EntityState.Modified;
            context.Update(user);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteItemAsync(int id)
        {
            var item = await context.Users.Include(x => x.Role).SingleOrDefaultAsync(x => x.ID == id);
            context.Entry<User>(item).State = EntityState.Detached;
            context.Remove(item);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<User> GetItemAsync(int id)
        {
            return await context.Users.Include(x => x.Role).SingleOrDefaultAsync(x => x.ID == id);
        }
    }
}
