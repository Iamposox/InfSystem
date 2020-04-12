using IS.Domain;
using IS.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IS.UI.Service
{
    public class AssortimentService
    {
        private readonly Context context;
        public AssortimentService(Context _context)
        {
            context = _context;
        }
        public async Task<IEnumerable<Assortment>> GetAssortments() => await context.Assortments.ToListAsync();
        public async Task<bool> AddOrUpdateAssortment(Assortment assortment)
        {
            if (assortment.ID == 0)
                return await AddNewAssortment(assortment);
            context.Update(assortment);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> AddNewAssortment(Assortment assortment)
        {
            try
            {
                await context.AddAsync(assortment);
                await context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> RemoveAssortment(Assortment assortment)
        {
            context.Remove(assortment);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
