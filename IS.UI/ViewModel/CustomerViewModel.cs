﻿using IS.Domain;
using IS.Domain.Model;
using IS.UI.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace IS.UI.ViewModel
{
    public class CustomerViewModel : Abstract.BindableObject
    {
        readonly Context context = new Context();
        public ObservableCollection<CustomerWrapper> Customers { get; set; } = new ObservableCollection<CustomerWrapper>();
        public ObservableCollection<ProductWrapper> Products { get; set; } = new ObservableCollection<ProductWrapper>();
        public ObservableCollection<ProductForCustomerWrapper> ProductForCustomer { get; set; } = new ObservableCollection<ProductForCustomerWrapper>();
        public CustomerViewModel()
        {
            context.Customers.ToList().ForEach(x => Customers.Add(new CustomerWrapper(x)));
            context.Products.ToList().ForEach(x => Products.Add(new ProductWrapper(x)));
            foreach (var item in Customers)
                item.ItemSelected += Item_Selected;
            Products.ToList().ForEach(x => x.ItemSelected += product_ItemSelected);
            ProductForCustomer.ToList().ForEach(x => x.ItemSelected += product_ItemSelected);
        }
        private Customer m_Customer = new Customer();
        public Customer SelectedCustomer
        {
            get => m_Customer;
            set
            {
                ProductForCustomer.ToList().ForEach(x => x.ItemSelected -= OutOrder_ItemSelected);
                ProductForCustomer.Clear();
                m_Customer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
                OnPropertyChanged(nameof(SelectedCustomer.Name));
                OnPropertyChanged(nameof(SelectedCustomer.Contact));
                SelectedCustomer.Orders.ForEach(x =>
                {
                    var Order = new ProductForCustomerWrapper(x);
                    Order.ItemSelected += product_ItemSelected;
                    ProductForCustomer.Add(Order);
                });
                SelectedCustomer.Purchased.ForEach(x =>
                {
                    var Order = new ProductForCustomerWrapper(x);
                    Order.ItemSelected += product_ItemSelected;
                    ProductForCustomer.Add(Order);
                });
            }
        }
        public ICommand AddNewCustomerOrder
        {
            get => new Command.ActionCommand((obj) =>
            {
                if (SelectedCustomer.Validate())
                {
                    ProductForCustomer.ToList().ForEach(x =>
                    {
                        SelectedCustomer.Orders.Add(x.GetProductForCustomer);
                        x.ItemSelected -= OutOrder_ItemSelected;
                    });
                    if (SelectedCustomer.ID == 0)
                        context.Add(SelectedCustomer);
                    SelectedCustomer.Orders.Clear();
                    context.SaveChanges();
                    foreach (var item in ProductForCustomer)
                        SelectedCustomer.Orders.Add(item.GetProductForCustomer);
                    context.SaveChanges();
                    Customers.Clear();
                    context.Customers
                                .ToList()
                                .ForEach(x => Customers.Add(new CustomerWrapper(x)));
                    foreach (var item in Customers)
                        item.ItemSelected += Item_Selected;
                    SelectedCustomer = new Customer();
                    ProductForCustomer.Clear();
                    OnPropertyChanged(nameof(SelectedCustomer));
                    OnPropertyChanged(nameof(Customers));
                    context.SaveChanges();
                }
            });
        }
        public ICommand AddNewCustomerPurchases
        {
            get => new Command.ActionCommand((obj) =>
            {
                if (SelectedCustomer.Validate())
                {
                    ProductForCustomer.ToList().ForEach(x =>
                    {
                        SelectedCustomer.Purchased.Add(x.GetProductForCustomer);
                        x.ItemSelected -= OutOrder_ItemSelected;
                    });
                    if (SelectedCustomer.ID == 0)
                        context.Add(SelectedCustomer);
                    SelectedCustomer.Purchased.Clear();
                    context.SaveChanges();
                    foreach (var item in ProductForCustomer)
                        SelectedCustomer.Purchased.Add(item.GetProductForCustomer);
                    context.SaveChanges();
                    Customers.Clear();
                    context.Customers
                                .ToList()
                                .ForEach(x => Customers.Add(new CustomerWrapper(x)));
                    foreach (var item in Customers)
                        item.ItemSelected += Item_Selected;
                    SelectedCustomer = new Customer();
                    ProductForCustomer.Clear();
                    OnPropertyChanged(nameof(SelectedCustomer));
                    OnPropertyChanged(nameof(Customers));
                    context.SaveChanges();
                }
            });
        }
        private void Item_Selected(object _sender, object _sendObject)
        {
            SelectedCustomer = (Customer)_sendObject;
        }
        private void product_ItemSelected(object _sender, object _sendObject) 
        {
            ProductForCustomer product = new ProductForCustomer();
            Assortment assortment = new Assortment();
            assortment.InAssortment = 1;
            assortment.Product = (Product)_sendObject;
            product.Product = assortment;
            var Order = new ProductForCustomerWrapper(product);
            Order.ItemSelected += OutOrder_ItemSelected;
            ProductForCustomer.Add(Order);
        }
        private void OutOrder_ItemSelected(object _sender, object _sendObject)
        {
            var Order = (ProductForCustomerWrapper)_sender;
            Order.ItemSelected -= OutOrder_ItemSelected;
            ProductForCustomer.Remove(Order);
        }
    }
}
