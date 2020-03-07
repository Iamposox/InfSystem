using IS.Domain;
using IS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace IS.UI.ViewModel
{
    public class SupplierViewModel:Abstract.BindableObject
    {
        readonly Context context;
        public ObservableCollection<Supplier> Suppliers { get; set; }
        public SupplierViewModel() 
        {
            context = new Context();
            Suppliers = new ObservableCollection<Supplier>(context.Suppliers.ToList());
        }
    }
}
