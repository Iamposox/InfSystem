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
    public class RawMaterialsViewModel : Abstract.BindableObject
    {
        readonly Context context;
        readonly IDataStore<RawMaterial> dataStore;
        public ObservableCollection<RawMaterialWrapper> RawMaterials { get; set; } = new ObservableCollection<RawMaterialWrapper>();
        private RawMaterialWrapper m_raw = new RawMaterialWrapper(new RawMaterial());
        public RawMaterialWrapper EditerRawMaterial
        {
            get => m_raw;
            set
            {
                m_raw = new RawMaterialWrapper((RawMaterial)value.GetMaterial.Clone());
            }
        }
        public ICommand AddRaw
        {
            get => new Command.ActionCommand(async (obj) => await AddRawMaterialsAsync());
        }
        public ICommand CancelCommand { get => new Command.ActionCommand((obj) => ResetEditableRaw(obj)); }
        public RawMaterialsViewModel()
        {
            context = new Context();
            dataStore = new Service.RawMaterialService(context);
            ReFreshRawMaterialsAsync();
        }
        private async void ReFreshRawMaterialsAsync()
        {
            RawMaterials.Clear();
            var RawMaterialsList = await dataStore.GetItemsAsync();
            RawMaterialsList.ToList().ForEach(x =>
            {
                var temp = new RawMaterialWrapper(x);
                temp.ItemSelected += RawMaterialItem_ItemSelectedAsync;
                RawMaterials.Add(temp);
            });
            OnPropertyChanged(nameof(RawMaterials));
        }
        private void ResetEditableRaw(object para)
        {
            m_raw = new RawMaterialWrapper(new RawMaterial());
            OnPropertyChanged(nameof(EditerRawMaterial));
        }
        private async Task AddRawMaterialsAsync()
        {
            ReFreshRawMaterialsAsync();
            if(!await dataStore.AddOrUpdateItemAsync(EditerRawMaterial.GetMaterial))
                MessageBox.Show("Something went wrong during the Process. Please try again later...");
            EditerRawMaterial = new RawMaterialWrapper(new RawMaterial());
            OnPropertyChanged(nameof(EditerRawMaterial));
            ReFreshRawMaterialsAsync();

        }
        private async void RawMaterialItem_ItemSelectedAsync(object _sender, object _sendObject)
        {
            if (_sendObject.ToString() == "Remove")
            {
                if (!await dataStore.DeleteItemAsync((_sender as RawMaterialWrapper).GetMaterial.ID))
                    MessageBox.Show("Something went wrong during the Process. Please try again later...");
                ReFreshRawMaterialsAsync();
            }
            else
            {
                EditerRawMaterial = (RawMaterialWrapper)_sender;
                OnPropertyChanged(nameof(EditerRawMaterial));
            }
        }
    }
}
