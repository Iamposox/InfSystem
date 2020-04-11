using IS.Domain;
using IS.Domain.Model;
using IS.UI.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IS.UI.ViewModel
{
    public delegate void SelectedItemDelegate(object _sender, object _sendObject);

    public class SupplierViewModel : Abstract.BindableObject
    {
        readonly Context context;

        public ObservableCollection<SupplierWrapper> Suppliers { get; set; } = new ObservableCollection<SupplierWrapper>();

        public ObservableCollection<RawMaterialWrapper> RawMaterials { get; set; } = new ObservableCollection<RawMaterialWrapper>();

        private SupplierWrapper m_EditedSupplier = new SupplierWrapper(new Supplier());

        public SupplierWrapper EditedSupplier
        {
            get => m_EditedSupplier;
            set
            {
                m_EditedSupplier = value;
                OnPropertyChanged(nameof(EditedSupplier));
            }
        }

        public ICommand AddNewSupplierCommand
        {
            get => new Command.ActionCommand(async (obj) =>
            {
                if (EditedSupplier.GetModel.Validate())
                {
                    if (!await new Service.SupplierService(context).AddORUpdateSupplierRecord(EditedSupplier.GetModel))
                        MessageBox.Show("Something went wrong during the Process. Please try again later...");
                    RePopulateSuppliersList();
                    OnPropertyChanged(nameof(EditedSupplier));
                    OnPropertyChanged(nameof(Suppliers));
                }
            });
        }

        public SupplierViewModel()
        {
            context = new Context();
            RePopulateSuppliersList();
            RawMaterials = new ObservableCollection<RawMaterialWrapper>();
            context.RawMaterials.ToList().ForEach(x => RawMaterials.Add(new RawMaterialWrapper(x)));
            RawMaterials.ToList().ForEach(x => x.ItemSelected += SelectedRawMaterialOrderToBeAddedToTheSelectedSupplier);
        }

        public void RemoveOrderableMaterialFromSelectedSupplier(int ID)
        {
            EditedSupplier.RemoveMaterialToORder(ID);
            OnPropertyChanged(nameof(EditedSupplier));
        }

        private void RePopulateSuppliersList()
        {
            Suppliers.Clear();
            context
            .Suppliers
            .Include(x => x.RawMaterials)
            .ThenInclude(x => x.Material)
            .ToList()
            .ForEach(x => Suppliers.Add(new SupplierWrapper(x)));
            foreach (var item in Suppliers)
            {
                item.ItemSelected += SupplierItemSelected;
            }
        }

        private async void SupplierItemSelected(object _sender, object _sendObject)
        {
            if (_sendObject.ToString() == "Remove")
            {
                context.Remove((_sender as SupplierWrapper).GetModel);
                await context.SaveChangesAsync();
                RePopulateSuppliersList();
            }
            else
                EditedSupplier = (SupplierWrapper)_sender;
        }

        private void SelectedRawMaterialOrderToBeAddedToTheSelectedSupplier(object _sender, object _sendObject)
        {
            RawMaterialsToOrder temp = new RawMaterialsToOrder();
            temp.Material = (RawMaterial)_sendObject;
            var toOrder = new RawMaterialsToOrderWrapper(temp);
            //toOrder.ItemSelected += ToOrder_ItemSelected;
            EditedSupplier.AddMaterialToOrder(toOrder.GetRawMaterialsToOrder);
            OnPropertyChanged(nameof(EditedSupplier));
        }

        //private void ToOrder_ItemSelected(object _sender, object _sendObject)
        //{
        //    var ToOrder = (RawMaterialsToOrderWrapper)_sender;
        //    ToOrder.ItemSelected -= ToOrder_ItemSelected;
        //    EditedSupplier.RemoveMaterialToORder(ToOrder.GetRawMaterialsToOrder.ID);
        //    OnPropertyChanged(nameof(EditedSupplier));
        //}


    }
}
