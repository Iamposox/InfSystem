using IS.Domain;
using IS.Domain.Model;
using IS.UI.Interface;
using IS.UI.Model;
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
    public class AssortmentViewModel : Abstract.BindableObject
    {
        readonly Context context;
        readonly IDataStore<Assortment> dataStore;
        public ObservableCollection<AssortimentsWrapper> Assortments { get; set; } = new ObservableCollection<AssortimentsWrapper>();
        private AssortimentsWrapper m_Assortiment = new AssortimentsWrapper(new Assortment());
        public AssortimentsWrapper EditerAssortiments
        {
            get => m_Assortiment;
            set
            {
                m_Assortiment = value;
                OnPropertyChanged(nameof(EditerAssortiments));
            }
        }
        public ICommand AddInAssortiment { get => new Command.ActionCommand(async (obj) => await EditAssortiment()); }
        public AssortmentViewModel()
        {
            context = new Context();
            dataStore = new Service.AssortimentService(context);
            ReFreshAssortimentsAsync();
        }
        private async void ReFreshAssortimentsAsync()
        {
            Assortments.Clear();
            var AssortList = await dataStore.GetItemsAsync();
            AssortList.ToList().ForEach(x =>
            {
                var temp = new AssortimentsWrapper(x);
                temp.ItemSelected += AssortimentItem_SelectedAsync;
                Assortments.Add(temp);
            });
            OnPropertyChanged(nameof(EditerAssortiments));
        }
        private async void AssortimentItem_SelectedAsync(object _sender, object _SendObject)
        {
            if (_SendObject.ToString() == "Remove")
            {
                if (!await dataStore.DeleteItemAsync((_sender as AssortimentsWrapper).GetAssortment.ID))
                    MessageBox.Show("Ошибка");
                ReFreshAssortimentsAsync();
            }
            else
                EditerAssortiments = (AssortimentsWrapper)_sender;
        }
        private async Task EditAssortiment()
        {
            ReFreshAssortimentsAsync();
            if (!await dataStore.AddOrUpdateItemAsync(EditerAssortiments.GetAssortment))
                MessageBox.Show("Ошибка");
            EditerAssortiments = new AssortimentsWrapper(new Assortment());
            OnPropertyChanged(nameof(EditerAssortiments));
            OnPropertyChanged(nameof(Assortments));
        }
    }
}
