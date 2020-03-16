using IS.Domain;
using IS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace IS.UI.ViewModel
{
    public class AssortmentViewModel:Abstract.BindableObject
    {
        readonly Context context;
        public ObservableCollection<Assortment> Assortments { get; set; }
        public List<Product >Product { get; set; }
        public AssortmentViewModel() 
        {
            context = new Context();
            Assortments = new ObservableCollection<Assortment>(context.Assortments.ToList());
            Product = new List<Product>(context.Products.ToList());
        }
        private Product m_SelectedProd;
        public Product SelectedProd 
        {
            get => m_SelectedProd;
            set 
            {
                m_SelectedProd = value;
            }
        }
    }
}
