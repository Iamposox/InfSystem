using IS.Domain;
using IS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IS.UI.ViewModel
{
    public class AssortmentViewModel : Abstract.BindableObject
    {
        readonly Context context;
        public ObservableCollection<Assortment> Assortments { get; set; }
        public List<Product> Product { get; set; }
        public AssortmentViewModel()
        {
            context = new Context();
            Assortments = new ObservableCollection<Assortment>(context.Assortments.ToList());
            Product = new List<Product>(context.Products.ToList());
        }
        private Product m_SelectedProd = new Product();
        public Product SelectedProduct
        {
            get => m_SelectedProd;
            set
            {
                m_SelectedProd = value;
                Assortiments = new Assortment { Product = value, InAssortment=m_CountOfAssortiment  };
            }
        }
        private int m_CountOfAssortiment;
        public int CountAssortiment
        {
            get => m_CountOfAssortiment;
            set
            {
                m_CountOfAssortiment = value;
            }
        }
        private Assortment m_Assortiment;
        public Assortment Assortiments
        {
            get => m_Assortiment;
            set
            {
                m_Assortiment = value;
            }
        }
        public ICommand AddInAssortiment { get => new Command.ActionCommand((obj) => AddAssortiment(obj)); }
        public void AddAssortiment(object obj) 
        {
            context.Assortments.Add(m_Assortiment);
            context.SaveChanges();
        }
    }
}
