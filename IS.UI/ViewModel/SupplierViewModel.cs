using IS.Domain;
using IS.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IS.UI.ViewModel
{
    public class SupplierViewModel:Abstract.BindableObject
    {
        readonly Context context;
        public ObservableCollection<Supplier> Suppliers { get; set; }

        public ObservableCollection<RawMaterialsToOrder> rawMaterialsToOrder { get; set; }
        public ObservableCollection<RawMaterial> RawMaterials { get; set; }
        
        public SupplierViewModel() 
        {
            context = new Context();
            Suppliers = new ObservableCollection<Supplier>(context.Suppliers.Include(x=>x.RawMaterials).ToList());

            RawMaterials = new ObservableCollection<RawMaterial>(context.RawMaterials.ToList());
            //rawMaterialsToOrder = new ObservableCollection<RawMaterialsToOrder>(context.RawMaterialsToOrder.ToList());
        }
      
    }
}
