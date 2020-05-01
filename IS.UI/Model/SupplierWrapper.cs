using IS.Domain.Model;
using IS.UI.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace IS.UI.Model
{
    public class SupplierWrapper:Abstract.BindableObject
    {
        private readonly Supplier m_Model;
        public event SelectedItemDelegate ItemSelected;
        public ObservableCollection<RawMaterialsToOrderWrapper> RawMaterials{ get; set; } = new ObservableCollection<RawMaterialsToOrderWrapper>();
        public List<RawMaterialsToOrder> RawMaterialsToOrders
        {
            get => m_Model.RawMaterials;
        }
        public SupplierWrapper(Supplier _model)
        {
            m_Model = _model;
            RawMaterialsToOrders.ForEach(x => RawMaterials.Add(new RawMaterialsToOrderWrapper(x)));
        }
        public Supplier GetModel { get => m_Model; }

        public void AddMaterialToOrder(RawMaterialsToOrderWrapper _materialToORder)
        {
            if (RawMaterials.Any(x => x.GetRawMaterialsToOrder.ID == _materialToORder.GetRawMaterialsToOrder.ID))
                return;
            RawMaterials.Add(_materialToORder);
            OnPropertyChanged(nameof(RawMaterials));
        }
        public void RemoveMaterialToORder(RawMaterialsToOrderWrapper item)
        {
            RawMaterials.Remove(item);
            OnPropertyChanged(nameof(RawMaterials));
        }
        public void AddToList()
        {
            RawMaterialsToOrders.Clear();
            foreach (var item in RawMaterials)
                RawMaterialsToOrders.Add(item.GetRawMaterialsToOrder);
        }

        public bool Valid
        {
            get
            {
                if (string.IsNullOrEmpty(Name)) return false;
                if (string.IsNullOrEmpty(Transport)) return false;
                if (string.IsNullOrEmpty(Contact)) return false;
                return true;
            }
        }
        public string Name 
        { 
            get => m_Model.Name;
            set
            {
                m_Model.Name = value;
            }
        }
        public string Transport 
        {
            get => m_Model.Transport;
            set
            {
                m_Model.Transport = value;
            }
        }

        public string Contact 
        {
            get => m_Model.Contact;
            set
            {
                m_Model.Contact = value;
            }
        }

        public ICommand Selected
        {
            get => new Command.ActionCommand((obj) =>
            {
                ItemSelected?.Invoke(this, obj);
            });
        }
    }
}
