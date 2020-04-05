using IS.Domain;
using IS.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IS.UI.Model
{
    public class CustomerDecoratorModel : Abstract.BindableObject
    {
        private Customer customer;
        public CustomerDecoratorModel(Customer _customer)
        {
            customer = _customer;
        }

        public string Name
        {
            get => customer.Name;
            set
            {
                customer.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Contact
        {
            get => customer.Contact;
            set
            {
                customer.Contact = value;
                OnPropertyChanged(nameof(Contact));
            }
        }
        public List<ProductForCustomer> Purchased
        {
            get => new Context()
                .Customers
                .Where(x => x.ID == customer.ID)
                .SelectMany(x => x.Purchased)
                .ToList();
        }
        public List<ProductForCustomer> Orders
        {
            get => new Context()
                .Customers
                .Where(x => x.ID == customer.ID)
                .SelectMany(x => x.Orders)
                .ToList();
        }
    }
}
