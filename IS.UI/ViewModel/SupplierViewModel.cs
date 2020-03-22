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
    public delegate void SelectedItemDelegate(object _sender, object _sendObject);

    public class RawMaterialWrapper
    {
        private RawMaterial rawMaterial;
        public event SelectedItemDelegate ItemSelected;
        public RawMaterialWrapper(RawMaterial _rawMaterial)
        {
            rawMaterial = _rawMaterial;
        }


        public string Name { get => rawMaterial.Name; }
        public ICommand Selected { get => new Command.ActionCommand((obj) =>
        {
            ItemSelected?.Invoke(this, rawMaterial);
        });}
    }

    public class RawMaterialsToOrderWrapper
    {
        private RawMaterialsToOrder rawMaterial;
        public event SelectedItemDelegate ItemSelected;
        public RawMaterialsToOrderWrapper(RawMaterialsToOrder _rawMaterial)
        {
            rawMaterial = _rawMaterial;
        }


        public string Name { get => rawMaterial.Material.Name; }
        public ICommand Selected
        {
            get => new Command.ActionCommand((obj) =>
            {
                ItemSelected?.Invoke(this, rawMaterial);
            });
        }
    }

    public class SupplierViewModel:Abstract.BindableObject
    {
        readonly Context context;
        public ObservableCollection<Supplier> Suppliers { get; set; }

        public ObservableCollection<RawMaterialWrapper> RawMaterials { get; set; }

        public ObservableCollection<RawMaterialsToOrderWrapper> rawMaterialsToOrder { get; set; } 
            = new ObservableCollection<RawMaterialsToOrderWrapper>();

        public Supplier NewSupplier { get; set; } = new Supplier();

        public ICommand AddNewSupplierCommand
        {
            get => new Command.ActionCommand((obj) =>
            {

            });
        }

        public SupplierViewModel() 
        {
            context = new Context();
            Suppliers = new ObservableCollection<Supplier>(context.Suppliers.Include(x=>x.RawMaterials).ToList());
            
            RawMaterials = new ObservableCollection<RawMaterialWrapper>();
            context.RawMaterials.ToList().ForEach(x=> RawMaterials.Add(new RawMaterialWrapper(x)));
            RawMaterials.ToList().ForEach(x => x.ItemSelected += X_ItemSelected);
        }

        private void X_ItemSelected(object _sender, object _sendObject)
        {
            RawMaterialsToOrder temp = new RawMaterialsToOrder();
            temp.Material = (RawMaterial)_sendObject;
            var toOrder = new RawMaterialsToOrderWrapper(temp);
            toOrder.ItemSelected += ToOrder_ItemSelected;
            rawMaterialsToOrder.Add(toOrder);
        }

        private void ToOrder_ItemSelected(object _sender, object _sendObject)
        {
            var ToOrder = (RawMaterialsToOrderWrapper)_sender;
            ToOrder.ItemSelected -= ToOrder_ItemSelected;
            rawMaterialsToOrder.Remove(ToOrder);
        }
    }
}
