using IS.Domain.Model;
using IS.UI.ViewModel;
using System.Windows.Input;

namespace IS.UI.Model
{
    public class SupplierWrapper
    {
        private readonly Supplier m_Model;
        public event SelectedItemDelegate ItemSelected;
        public SupplierWrapper(Supplier _model)
        {
            m_Model = _model;
        }

        public Supplier GetModel { get => m_Model; }

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
                ItemSelected?.Invoke(this, m_Model);
            });
        }
    }
}
