using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using IS.Domain.Model;
using IS.UI.ViewModel;

namespace IS.UI.Model
{
    public class AssortimentsWrapper
    {
        private Assortment m_Assortment = new Assortment();
        public event SelectedItemDelegate ItemSelected;
        public AssortimentsWrapper(Assortment _assortment)
        {
            m_Assortment = _assortment;
        }
        public Assortment GetAssortment { get => m_Assortment; }
        public double InAssortimnet 
        { 
            get => m_Assortment.InAssortment;
            set 
            {
                m_Assortment.InAssortment = value;
            }
        }
        public Product product { get => m_Assortment.Product; set => m_Assortment.Product = value; }
        //public string Name { get => m_Assortment.Product.Name; set => m_Assortment.Product.Name = value; }
        public ICommand Selected
        {
            get => new Command.ActionCommand((obj) =>
              {
                  ItemSelected?.Invoke(this, obj);
              });
        }
    }
}
