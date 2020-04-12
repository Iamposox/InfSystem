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
        public List<RawMaterialsToOrder> rawMaterialsToOrder 
        {
            get => m_Model.RawMaterials; 
        }
            
        public SupplierWrapper(Supplier _model)
        {
            m_Model = _model;
        }
        public Supplier GetModel { get => m_Model; }

        public void AddMaterialToOrder(RawMaterialsToOrder _materialToORder)
        {
            if (rawMaterialsToOrder.Any(x => x.Material.ID == _materialToORder.Material.ID))
                return;
            rawMaterialsToOrder.Add(_materialToORder);
            OnPropertyChanged(nameof(rawMaterialsToOrder));
        }
        public void RemoveMaterialToORder(int ID)
        {
            rawMaterialsToOrder.Remove(rawMaterialsToOrder.Single(x => x.ID == ID));
            OnPropertyChanged(nameof(rawMaterialsToOrder));
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
