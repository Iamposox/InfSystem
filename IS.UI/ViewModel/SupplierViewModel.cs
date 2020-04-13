using IS.Domain;
using IS.Domain.Model;
using IS.UI.Interface;
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
        readonly IDataStore<Supplier> dataStore;

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

        public ICommand CancleCommand { get => new Command.ActionCommand((obj) => ResetEditableSupplier(obj)); }

        public SupplierViewModel()
        {
            context = new Context();
            dataStore = new Service.SupplierService(context);

            RePopulateSuppliersList();
            new Service.RawMaterialService(context).GetRawMaterials().GetAwaiter().GetResult().ToList().ForEach(x=> 
            {
                var temp = new RawMaterialWrapper(x);
                temp.ItemSelected += SelectedRawMaterialOrderToBeAddedToTheSelectedSupplier;
                RawMaterials.Add(temp);
            });
        }

        private void ResetEditableSupplier(object para)
        {
            m_EditedSupplier = new SupplierWrapper(new Supplier());
            OnPropertyChanged(nameof(EditedSupplier));
        }

        private async Task ModifySelectedSupplier()
        {
            if (EditedSupplier.GetModel.Validate())
            {
                if (!await dataStore.AddOrUpdateItemAsync(EditedSupplier.GetModel))
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
            var SuppliersList = await dataStore.GetItemsAsync();
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
                if(!await dataStore.DeleteItemAsync((_sender as SupplierWrapper).GetModel.ID))
                    MessageBox.Show("Something went wrong during the Process. Please try again later...");
                RePopulateSuppliersList();
            }
            else
                EditedSupplier = (SupplierWrapper)_sender;
        }

        private void SelectedRawMaterialOrderToBeAddedToTheSelectedSupplier(object _sender, object _sendObject)
        {
            RawMaterialsToOrder temp = new RawMaterialsToOrder();
            temp.Material = ((RawMaterialWrapper)_sender).GetMaterial;
            var toOrder = new RawMaterialsToOrderWrapper(temp);
            EditedSupplier.AddMaterialToOrder(toOrder.GetRawMaterialsToOrder);
            OnPropertyChanged(nameof(EditedSupplier));
        }

    }
}
