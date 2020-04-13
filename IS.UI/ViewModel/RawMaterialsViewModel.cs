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
    public class RawMaterialsViewModel : Abstract.BindableObject
    {
        readonly Context context;
        public ObservableCollection<RawMaterialWrapper> RawMaterials { get; set; } = new ObservableCollection<RawMaterialWrapper>();
        private RawMaterialWrapper m_raw = new RawMaterialWrapper(new RawMaterial());
        public RawMaterialWrapper EditerRawMaterial
        {
            get => m_raw;
            set
            {
                m_raw = value;
            }
        }
        public RawMaterialsViewModel()
        {
            context = new Context();
            ReFreshRawMaterialsAsync();
        }
        private async void ReFreshRawMaterialsAsync()
        {
            RawMaterials.Clear();
            var RawMaterialsList = await new Service.RawMaterialService(context).GetItemsAsync();
            RawMaterialsList.ToList().ForEach(x =>
            {
                var temp = new RawMaterialWrapper(x);
                temp.ItemSelected += RawMaterialItem_ItemSelectedAsync;
                RawMaterials.Add(temp);
            });
        }

        public ICommand AddRaw
        {
            get => new Command.ActionCommand(async (obj) => await AddRawMaterialsAsync());

        }
        private async Task AddRawMaterialsAsync()
        {
            ReFreshRawMaterialsAsync();
            if(!await new Service.RawMaterialService(context).AddOrUpdateItemAsync(EditerRawMaterial.GetMaterial))
                MessageBox.Show("Something went wrong during the Process. Please try again later...");
            EditerRawMaterial = new RawMaterialWrapper(new RawMaterial());
            OnPropertyChanged(nameof(EditerRawMaterial));

        }
        private async void RawMaterialItem_ItemSelectedAsync(object _sender, object _sendObject)
        {
            if (_sendObject.ToString() == "Remove")
            {
                if (!await new Service.RawMaterialService(context).DeleteItemAsync((_sender as RawMaterialWrapper).GetMaterial.ID))
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
