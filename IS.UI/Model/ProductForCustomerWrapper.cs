using IS.Domain.Model;
using IS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace IS.UI.Model
{
    public class ProductForCustomerWrapper
    {
        private readonly ProductForCustomer productForCustomer;
        public event SelectedItemDelegate ItemSelected;
        public ProductForCustomerWrapper(ProductForCustomer _product)
        {
            productForCustomer = _product;
        }
        public ProductForCustomer GetProductForCustomer { get => productForCustomer; }
        public bool Valid
        {
            get
            {
                if (string.IsNullOrEmpty(Name)) return false;
                if (Price == 0) return false;
                return true;
            }
        }
        public string Name { get => productForCustomer.Product.Product.Name; }
        public double Price
        {
            get => productForCustomer.Price;
            set
            {
                productForCustomer.Price = value;
            }
        }
        public int ID { get => productForCustomer.ID; }
        public ICommand Selected
        {
            get => new Command.ActionCommand((obj) =>
            {
                ItemSelected?.Invoke(this, productForCustomer);
            });
        }
    }
}
