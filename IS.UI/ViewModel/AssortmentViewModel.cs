using IS.Domain;
using IS.Domain.Model;
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
        public AssortmentViewModel()
        {
            context = new Context();
            ReFreshAssortiments();
        }
        private async void ReFreshAssortiments()
        {
            Assortments.Clear();
            var AssortList = await new Service.AssortimentService(context).GetAssortments();
            AssortList.ToList().ForEach(x =>
            {
                var temp = new AssortimentsWrapper(x);
                temp.ItemSelected += AssortimentItem_Selected;
                Assortments.Add(temp);
            });
        }

        public ICommand AddInAssortiment { get => new Command.ActionCommand(async(obj) => await EditAssortiment()); }
        private void AssortimentItem_Selected(object _sender, object _SendObject)
        {
            EditerAssortiments = (AssortimentsWrapper)_sender;
        }
        private async Task EditAssortiment()
        {
            ReFreshAssortiments();
            if (!await new Service.AssortimentService(context).AddOrUpdateAssortment(EditerAssortiments.GetAssortment))
                MessageBox.Show("Ошибка");
            EditerAssortiments = new AssortimentsWrapper(new Assortment());
            OnPropertyChanged(nameof(EditerAssortiments));
            OnPropertyChanged(nameof(Assortments));
        }
    }
}
