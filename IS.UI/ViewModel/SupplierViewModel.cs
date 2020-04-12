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

        public ICommand ModifySelectedSupplierCommand
        {
            get => new Command.ActionCommand(async (obj) => await ModifySelectedSupplier());
        }


        public SupplierViewModel()
        {
            context = new Context();
            RePopulateSuppliersList();
            new Service.RawMaterialService(context).GetRawMaterials().GetAwaiter().GetResult().ToList().ForEach(x=> 
            {
                var temp = new RawMaterialWrapper(x);
                temp.ItemSelected += SelectedRawMaterialOrderToBeAddedToTheSelectedSupplier;
                RawMaterials.Add(temp);
            });
        }

        private async Task ModifySelectedSupplier()
        {
            if (EditedSupplier.GetModel.Validate())
            {
                if (!await new Service.SupplierService(context).AddORUpdateSupplierRecord(EditedSupplier.GetModel))
                    MessageBox.Show("Something went wrong during the Process. Please try again later...");
                RePopulateSuppliersList();
                OnPropertyChanged(nameof(EditedSupplier));
            }
        }

        public async void RemoveOrderableMaterialFromSelectedSupplier(int ID)
        {
            EditedSupplier.RemoveMaterialToORder(ID);
            await ModifySelectedSupplier();
            OnPropertyChanged(nameof(EditedSupplier));
        }

        private async void RePopulateSuppliersList()
        {
            Suppliers.Clear();
            var SuppliersList = await new Service.SupplierService(context).GetSuppliersAsync();
            SuppliersList.ToList().ForEach(x =>
            {
                var temp = new SupplierWrapper(x);
                temp.ItemSelected += SupplierItemSelected;
                Suppliers.Add(temp);
            });
            OnPropertyChanged(nameof(Suppliers));
        }

        private async void SupplierItemSelected(object _sender, object _sendObject)
        {
            if (_sendObject.ToString() == "Remove")
            {
                if(!await new Service.SupplierService(context).RemoveSupplierAsync((_sender as SupplierWrapper).GetModel))
                    MessageBox.Show("Something went wrong during the Process. Please try again later...");
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
            EditedSupplier.AddMaterialToOrder(toOrder.GetRawMaterialsToOrder);
            OnPropertyChanged(nameof(EditedSupplier));
        }

    }
}
