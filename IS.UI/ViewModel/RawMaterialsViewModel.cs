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
    public class RawMaterialsViewModel: Abstract.BindableObject
    {
        readonly Context context;
        public ObservableCollection<RawMaterialWrapper> rawMaterials { get; set; } = new ObservableCollection<RawMaterialWrapper>();
        private RawMaterial m_raw = new RawMaterial();
        public RawMaterialsViewModel() 
        {
            context = new Context();
            context.RawMaterials.ToList().ForEach(x => rawMaterials.Add(new RawMaterialWrapper(x)));
        }
        public RawMaterial EditerRawMaterial
        {
            get => m_raw;
            set 
            {
                m_raw = value;
                OnPropertyChanged(nameof(EditerRawMaterial));
                OnPropertyChanged(nameof(EditerRawMaterial.Name));
                OnPropertyChanged(nameof(EditerRawMaterial.Amount));
            }
        }
        public ICommand AddRaw { get => new Command.ActionCommand((obj) => AddRawMate(obj)); }
        public void AddRawMate(object obj) 
        {
            context.RawMaterials.Add(m_raw);
            context.SaveChanges();
        }
        private void Item_ItemSelected(object _sender, object _sendObject)
        {
            EditerRawMaterial = (RawMaterial)_sendObject;
        }
    }
}
