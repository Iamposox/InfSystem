using IS.Domain;
using IS.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IS.UI.Service
{
    public class RawMaterialService
    {
        private readonly Context context;
        public RawMaterialService(Context _context)
        {
            context = _context;
        }

        public async Task<IEnumerable<RawMaterial>> GetRawMaterials() => await context.RawMaterials.ToListAsync();
    }
}
