using IS.Domain;
using IS.Domain.Model;
using IS.UI.Interface;
using IS.UI.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IS.UI.ViewModel
{
    public class CustomerViewModel : Abstract.BindableObject
    {
        private readonly Context context;
        readonly IDataStore<Customer> dataStore;
        public ObservableCollection<CustomerWrapper> Customers { get; set; } = new ObservableCollection<CustomerWrapper>();
        public ObservableCollection<ProductWrapper> Products { get; set; } = new ObservableCollection<ProductWrapper>();
        private CustomerWrapper m_Customer = new CustomerWrapper(new Customer());
        public CustomerWrapper SelectedCustomer
        {
            get => m_Customer;
            set
            {
                m_Customer = new CustomerWrapper((Customer)value.GetCustomer.Clone());
                foreach (var item in SelectedCustomer.Product)
                    item.ItemSelected += OutOrder_ItemSelected;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }
        public ICommand AddNewCustomer
        {
            get => new Command.ActionCommand(async (obj) => await AddCustomerAsync(obj));
        }
        public ICommand CancelCommand { get => new Command.ActionCommand((obj) => ResetEditableCustomer(obj)); }
        public CustomerViewModel()
        {
            context = new Context();
            dataStore = new Service.CustomerService(context);
            ReFreshCustomerListAsync();
            new Service.ProductService(context).GetProducts().GetAwaiter().GetResult().ToList().ForEach(x =>
            {
                var temp = new ProductWrapper(x);
                temp.ItemSelected += Product_ItemSelected;
                Products.Add(temp);
            });
        }
        private async void ReFreshCustomerListAsync()
        {
            Customers.Clear();
            var CustomersList = await dataStore.GetItemsAsync();
            CustomersList.ToList().ForEach(x =>
            {
                var temp = new CustomerWrapper(x);
                temp.ItemSelected += CustomerItem_SelectedAsync;
                Customers.Add(temp);
            });
            OnPropertyChanged(nameof(Customers));
        }
        private void ResetEditableCustomer(object para)
        {
            m_Customer = new CustomerWrapper(new Customer());
            OnPropertyChanged(nameof(SelectedCustomer));
        }
        private async Task AddCustomerAsync(object obj)
        {
            if (obj.ToString() == "AddToPurchases")
                SelectedCustomer.AddToPurchased();
            else
                SelectedCustomer.AddToOrders();
            if (!await dataStore.AddOrUpdateItemAsync(SelectedCustomer.GetCustomer))
                MessageBox.Show("Something went wrong during the Process. Please try again later...");
            Customers.Clear();
            ReFreshCustomerListAsync();
            SelectedCustomer = new CustomerWrapper(new Customer());
            OnPropertyChanged(nameof(SelectedCustomer));
            OnPropertyChanged(nameof(Customers));
        }
        private async void CustomerItem_SelectedAsync(object _sender, object _sendObject)
        {
            if (_sendObject.ToString() == "Remove")
            {
                if (!await dataStore.DeleteItemAsync((_sender as CustomerWrapper).GetCustomer.ID))
                    MessageBox.Show("Something went wrong during the Process. Please try again later...");
                ReFreshCustomerListAsync();
            }
            else
                SelectedCustomer = (CustomerWrapper)_sender;

        }
        private void Product_ItemSelected(object _sender, object _sendObject)
        {
            ProductForCustomer product = new ProductForCustomer();
            Assortment assortment = new Assortment();
            assortment.Product = (Product)_sendObject;
            product.Product = assortment;
            var Order = new ProductForCustomerWrapper(product);
            SelectedCustomer.AddToProduct(Order.GetProductForCustomer);
            foreach (var item in SelectedCustomer.Product)
                item.ItemSelected += OutOrder_ItemSelected;
            OnPropertyChanged(nameof(SelectedCustomer));
        }
        public void OutOrder_ItemSelected(object _sender, object _sendObject)
        {
            var Order = (ProductForCustomerWrapper)_sender;
            Order.ItemSelected -= OutOrder_ItemSelected;
            SelectedCustomer.RemoveProduct(Order);
            OnPropertyChanged(nameof(SelectedCustomer));
        }
    }
}
