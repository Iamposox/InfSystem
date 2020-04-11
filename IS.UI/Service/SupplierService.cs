using IS.Domain;
using IS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace IS.UI.Service
{
    public class SupplierService
    {
        private readonly Context context;

        public SupplierService(Context _context)
        {
            context = _context;
        }


        public async Task<bool> AddORUpdateSupplierRecord(Supplier _record)
        {
            if (!IsSupplierRecordValid(_record))
                return false;
            if (_record.ID == 0)
                return await AddNewSupplierRecord(_record);
            context.Update(_record);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddNewSupplierRecord(Supplier _record)
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

        private bool IsSupplierRecordValid(Supplier _record) => _record.Validate();
    }
}
