using IS.Domain;
using IS.Domain.Model;
using IS.UI.Model;
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
                if(NewSupplier.Validate())
                {
                    rawMaterialsToOrder.ToList().ForEach(x => 
                    { 
                        NewSupplier.RawMaterials.Add(x.GetRawMaterialsToOrder);
                        x.ItemSelected -= ToOrder_ItemSelected;
                    });
                    context.Add(NewSupplier);
                    context.SaveChanges();
                    Suppliers = new ObservableCollection<Supplier>(context
                        .Suppliers
                        .Include(x => x.RawMaterials)
                        .ThenInclude(x => x.Material)
                        .ToList());
                    NewSupplier = new Supplier();
                    rawMaterialsToOrder.Clear();
                    OnPropertyChanged(nameof(NewSupplier));
                    OnPropertyChanged(nameof(Suppliers));
                }
            });
        }

        public SupplierViewModel() 
        {
            context = new Context();
            Suppliers = new ObservableCollection<Supplier>(context
                .Suppliers
                .Include(x=>x.RawMaterials)
                .ThenInclude(x=>x.Material)
                .ToList());
            
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
