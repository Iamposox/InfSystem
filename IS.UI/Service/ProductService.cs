using IS.Domain;
using IS.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IS.UI.Service
{
    public class ProductService
    {
        private readonly Context context;
        public ProductService(Context _context)
        {
            context = _context;
        }
        public async Task<IEnumerable<Product>> GetProducts() => await context.Products.ToListAsync();
    }
}
