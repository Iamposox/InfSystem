using IS.Domain;
using IS.Domain.Model;
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
        readonly Context context;
        public ObservableCollection<CustomerWrapper> Customers { get; set; } = new ObservableCollection<CustomerWrapper>();
        public ObservableCollection<ProductWrapper> Products { get; set; } = new ObservableCollection<ProductWrapper>();
        private CustomerWrapper m_Customer = new CustomerWrapper(new Customer());
        public CustomerViewModel()
        {
            context = new Context();
            ReRecordCustomerList();
            new Service.ProductService(context).GetProducts().GetAwaiter().GetResult().ToList().ForEach(x =>
            {
                var temp = new ProductWrapper(x);
                temp.ItemSelected += product_ItemSelected;
                Products.Add(temp);
            });
        }
        private async void ReRecordCustomerList()
        {
            Customers.Clear();
            var CustomersList = await new Service.CustomerService(context).GetCustomers();
            CustomersList.ToList().ForEach(x =>
            {
                var temp = new CustomerWrapper(x);
                temp.ItemSelected += Item_Selected;
                Customers.Add(temp);
            });
            OnPropertyChanged(nameof(Customers));
        }
        public CustomerWrapper SelectedCustomer
        {
            get => m_Customer;
            set
            {
                m_Customer = value;
                foreach (var item in SelectedCustomer.Product)
                    item.ItemSelected += OutOrder_ItemSelected;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }
        private void ClearCustomer()
        {
            context.SaveChanges();
            Customers.Clear();
            ReRecordCustomerList();
            SelectedCustomer = new CustomerWrapper(new Customer());
        }
        public ICommand AddNewCustomer
        {
            get => new Command.ActionCommand(async (obj) => await AddCustomer(obj));
        }
        private async Task AddCustomer(object obj)
        {
            if (obj.ToString() == "AddToPurchases")
                SelectedCustomer.AddToPurchased();
            else
                SelectedCustomer.AddToOrders();
            if (!await new Service.CustomerService(context).AddOrUpdate(SelectedCustomer.GetCustomer))
                MessageBox.Show("Something went wrong during the Process. Please try again later...");
            ClearCustomer();
            OnPropertyChanged(nameof(SelectedCustomer));
            OnPropertyChanged(nameof(Customers));
            context.SaveChanges();

        }
        private async void Item_Selected(object _sender, object _sendObject)
        {
            if (_sendObject.ToString() == "Remove")
            {
                if (!await new Service.CustomerService(context).RemoveCustomers((_sender as CustomerWrapper).GetCustomer))
                    MessageBox.Show("Something went wrong during the Process. Please try again later...");
                ReRecordCustomerList();
            }
            else
                SelectedCustomer = (CustomerWrapper)_sender;

        }
        private void product_ItemSelected(object _sender, object _sendObject)
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
