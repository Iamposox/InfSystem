using IS.Domain.Model;
using IS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IS.UI.Model
{
    public class CustomerWrapper:Abstract.BindableObject
    {
        private Customer customer;
        public event SelectedItemDelegate ItemSelected;
        public CustomerWrapper(Customer _customer)
        {
            customer = _customer;
            Orders.ForEach(x => Product.Add(new ProductForCustomerWrapper(x)));
            Purchased.ForEach(x => Product.Add(new ProductForCustomerWrapper(x)));
            //foreach (var item in )
        }
        public Customer GetCustomer { get => customer; }
        public string Name { get => customer.Name; set => customer.Name = value; }
        public string Contact { get => customer.Contact; set => customer.Contact = value; }
        public List<ProductForCustomer> Purchased { get => customer.Purchased; }
        public List<ProductForCustomer> Orders { get => customer.Orders; }
        public ObservableCollection<ProductForCustomerWrapper> Product { get; set; } = new ObservableCollection<ProductForCustomerWrapper>();
        public bool Valid
        {
            get
            {
                if (string.IsNullOrEmpty(Name)) return false;
                if (string.IsNullOrEmpty(Contact)) return false;
                return true;
            }
        }
        public void AddToProduct(ProductForCustomer product)
        {
            if (Product.Any(x => x.ID == product.Product.Product.ID))
                return;
            Product.Add(new ProductForCustomerWrapper(product));
            OnPropertyChanged(nameof(Product));
        }
        public void RemoveProduct(ProductForCustomerWrapper product)
        {
            Product.Remove(product);
            OnPropertyChanged(nameof(Product));
        }
        public void AddToPurchased()
        {
            Purchased.Clear();
            foreach (var item in Product)
                Purchased.Add(item.GetProductForCustomer);
        }
        public void AddToOrders()
        {
            Orders.Clear();
            foreach (var item in Product)
                Orders.Add(item.GetProductForCustomer);
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
