 using IS.Domain.Model;
using IS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace IS.UI.Model
{
    public class ProductWrapper
    {
        private readonly Product m_Product;
        public event SelectedItemDelegate ItemSelected;
        public ProductWrapper(Product _product) 
        {
            m_Product = _product;
        }
        public Product GetProduct { get => m_Product; }
        public string Name
        {
            get => m_Product.Name;
            set 
            {
                m_Product.Name = value;
            }
        }
        public ICommand Selected { get => new Command.ActionCommand((obj) =>
        {
            ItemSelected?.Invoke(this, m_Product);
        });
        }
    }
}
