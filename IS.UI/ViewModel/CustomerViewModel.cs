using IS.Domain;
using IS.Domain.Model;
using IS.UI.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;

namespace IS.UI.ViewModel
{
    public class CustomerViewModel : Abstract.BindableObject
    {
        readonly Context context;
        public ObservableCollection<Customer> Customers { get; set; }
        public ObservableCollection<ProductWrapper> Products { get; set; } = new ObservableCollection<ProductWrapper>();
        public CustomerViewModel()
        {
            context = new Context();
            Customers = new ObservableCollection<Customer>(context.Customers.ToList());
        }
        private Customer m_Customer = new Customer();
        public Customer SelectedCustomer
        {
            get => m_Customer;
            set
            {
                m_Customer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }
        
    }
}
