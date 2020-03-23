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

    public class SupplierViewModel : Abstract.BindableObject
    {
        readonly Context context;
        public ObservableCollection<SupplierWrapper> Suppliers { get; set; } = new ObservableCollection<SupplierWrapper>();

        public ObservableCollection<RawMaterialWrapper> RawMaterials { get; set; } = new ObservableCollection<RawMaterialWrapper>();

        public ObservableCollection<RawMaterialsToOrderWrapper> rawMaterialsToOrder { get; set; }
            = new ObservableCollection<RawMaterialsToOrderWrapper>();

        private Supplier m_EditedSupplier = new Supplier();

        public Supplier EditedSupplier
        {
            get => m_EditedSupplier;
            set
            {
                rawMaterialsToOrder.ToList().ForEach(x => x.ItemSelected -= ToOrder_ItemSelected);
                rawMaterialsToOrder.Clear();
                m_EditedSupplier = value;
                OnPropertyChanged(nameof(EditedSupplier));
                OnPropertyChanged(nameof(EditedSupplier.Contact));
                OnPropertyChanged(nameof(EditedSupplier.Name));
                OnPropertyChanged(nameof(EditedSupplier.Transport));
                EditedSupplier.RawMaterials.ForEach(x =>
                {
                    var toOrder = new RawMaterialsToOrderWrapper(x);
                    toOrder.ItemSelected += ToOrder_ItemSelected;
                    rawMaterialsToOrder.Add(toOrder);
                });
            }
        }

        public ICommand AddNewSupplierCommand
        {
            get => new Command.ActionCommand((obj) =>
            {
                if (EditedSupplier.Validate())
                {
                    rawMaterialsToOrder.ToList().ForEach(x =>
                    {
                        EditedSupplier.RawMaterials.Add(x.GetRawMaterialsToOrder);
                        x.ItemSelected -= ToOrder_ItemSelected;
                    });

                    //New Supplier
                    if (EditedSupplier.ID == 0)
                        context.Add(EditedSupplier);
                    EditedSupplier.RawMaterials.Clear();
                    foreach (var item in rawMaterialsToOrder)
                    {
                        EditedSupplier.RawMaterials.Add(item.GetRawMaterialsToOrder);
                    }
                    context.SaveChanges();
                    Suppliers.Clear();
                    context
                        .Suppliers
                        .Include(x => x.RawMaterials)
                        .ThenInclude(x => x.Material)
                        .ToList().ForEach(x => Suppliers.Add(new SupplierWrapper(x)));
                    foreach (var item in Suppliers)
                    {
                        item.ItemSelected += Item_ItemSelected;
                    }
                    EditedSupplier = new Supplier();
                    rawMaterialsToOrder.Clear();
                    OnPropertyChanged(nameof(EditedSupplier));
                    OnPropertyChanged(nameof(Suppliers));
                    context.SaveChanges();
                }
            });
        }

        public SupplierViewModel()
        {
            context = new Context();

            context
                .Suppliers
                .Include(x => x.RawMaterials)
                .ThenInclude(x => x.Material)
                .ToList().ForEach(x => Suppliers.Add(new SupplierWrapper(x)));

            foreach (var item in Suppliers)
            {
                item.ItemSelected += Item_ItemSelected;
            }
            RawMaterials = new ObservableCollection<RawMaterialWrapper>();
            context.RawMaterials.ToList().ForEach(x => RawMaterials.Add(new RawMaterialWrapper(x)));
            RawMaterials.ToList().ForEach(x => x.ItemSelected += X_ItemSelected);
        }

        private void Item_ItemSelected(object _sender, object _sendObject)
        {
            EditedSupplier = (Supplier)_sendObject;
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
