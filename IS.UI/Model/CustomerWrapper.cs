using IS.Domain.Model;
using IS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace IS.UI.Model
{
    public class CustomerWrapper
    {
        private Customer customer;
        public event SelectedItemDelegate ItemSelected;
        public CustomerWrapper(Customer _customer)
        {
            customer = _customer;
        }
        public Customer GetCustomer { get => customer; }
        public string Name { get => customer.Name; }
        public string Contact { get => customer.Contact; }
        public bool Valid
        {
            get
            {
                if (string.IsNullOrEmpty(Name)) return false;
                if (string.IsNullOrEmpty(Contact)) return false;
                return true;
            }
        }
        public ICommand Selected
        {
            get => new Command.ActionCommand((obj) =>
            {
                ItemSelected?.Invoke(this, customer);
            });
        }
    }
}
