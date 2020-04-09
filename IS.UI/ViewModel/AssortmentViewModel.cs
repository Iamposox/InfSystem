using IS.Domain;
using IS.Domain.Model;
using IS.UI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IS.UI.ViewModel
{
    public class AssortmentViewModel : Abstract.BindableObject
    {
        readonly Context context;
        public ObservableCollection<AssortimentsWrapper> Assortments { get; set; } = new ObservableCollection<AssortimentsWrapper>();
        public AssortmentViewModel()
        {
            context = new Context();
            context.Assortments.ToList().ForEach(x => Assortments.Add(new AssortimentsWrapper(x)));
            foreach (var item in Assortments)
                item.ItemSelected += Item_Selected;
        }
        private Assortment m_Assortiment;
        public Assortment EditerAssortiments
        {
            get => m_Assortiment;
            set
            {
                m_Assortiment = value;
                Changed();
            }
        }
        public ICommand AddInAssortiment { get => new Command.ActionCommand((obj) =>
        {
            if (EditerAssortiments.Validate())
            {
                if (EditerAssortiments.ID == 0)
                    context.Add(EditerAssortiments);
                EditAssortiment();
            }
        }); }
        private void Item_Selected(object _sender, object _SendObject)
        {
            EditerAssortiments = (Assortment)_SendObject;
        }
        private void EditAssortiment()
        {
            context.SaveChanges();
            Assortments.Clear();
            context.Assortments.ToList().ForEach(x => Assortments.Add(new AssortimentsWrapper(x)));
            foreach (var item in Assortments)
                item.ItemSelected += Item_Selected;
            EditerAssortiments = new Assortment();
            context.SaveChanges();
        }
        private void Changed()
        {
            OnPropertyChanged(nameof(EditerAssortiments));
            OnPropertyChanged(nameof(EditerAssortiments.Product));
            OnPropertyChanged(nameof(EditerAssortiments.InAssortment));
        }
    }
}
