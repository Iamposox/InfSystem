using IS.Domain;
using IS.Domain.Model;
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
        public SupplierViewModel() 
        {
            context = new Context();
            Suppliers = new ObservableCollection<Supplier>(context.Suppliers.ToList());
            rawMaterialsToOrder = new ObservableCollection<RawMaterialsToOrder>(context.RawMaterialsToOrder.ToList());
        }
        private Supplier m_supplier;
        public Supplier Supplier
        {
            
            get => m_supplier;
            set 
            {
                m_supplier = value;
            }
        }
        public ICommand AddSuplier { get => new Command.ActionCommand((obj) => AddSupli(obj)); }
        public void AddSupli(object obj) 
        {
            context.Suppliers.Add(m_supplier);
            context.SaveChanges();
        }
    }
}
