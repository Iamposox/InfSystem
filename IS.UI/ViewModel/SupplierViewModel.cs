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
        public ObservableCollection<RawMaterial> RawMaterials { get; set; }
        
        public SupplierViewModel() 
        {
            context = new Context();
            Suppliers = new ObservableCollection<Supplier>(context.Suppliers.ToList());
            RawMaterials = new ObservableCollection<RawMaterial>(context.RawMaterials.ToList());
            rawMaterialsToOrder = new ObservableCollection<RawMaterialsToOrder>(context.RawMaterialsToOrder.ToList());
        }
        private Supplier m_supplier = new Supplier();
        public Supplier Supplier
        {
            
            get => m_supplier;
            set 
            {
                m_supplier = value;
                m_supplier.RawMaterials = new List<RawMaterial>();
                m_supplier.RawMaterials.Add(m_RawMaterialsInSupplier);
            }
        }
        private double m_RawMaterialAmount;
        public double AmountOfRawMaterial 
        {
            get=>m_RawMaterialAmount;
            set 
            {
                m_RawMaterialAmount = value;
            }
        }
        private string m_NameOfRawMaterial;
        public string NameOfRawMaterial 
        {
            get => m_NameOfRawMaterial;
            set 
            {
                m_NameOfRawMaterial = value;
            }
        }
        private RawMaterial m_RawMaterialsInSupplier=new RawMaterial();
        public RawMaterial RawMaterialsInSupplier 
        {
            get => m_RawMaterialsInSupplier;
            set 
            {
                m_RawMaterialsInSupplier = value;
            }
        }
        public ICommand AddSuplier { get => new Command.ActionCommand((obj) => AddSupliers(obj)); }
        public void AddSupliers(object obj) 
        {
            context.Suppliers.Add(m_supplier);
            context.SaveChanges();
        }
    }
}
