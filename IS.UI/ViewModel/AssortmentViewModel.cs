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
        public Product SelectedProd
        {
            get => m_SelectedProd;
            set
            {
                m_SelectedProd = value;
                Assortment = new Assortment { Product = value, InAssortment=m_Assortiment  };
            }
        }
        private int m_Assortiment;
        public int Assortiment
        {
            get => m_Assortiment;
            set
            {
                m_Assortiment = value;
            }
        }
        private Assortment m_Assortment = new Assortment();
        public Assortment Assortment
        {
            get => m_Assortment;
            set
            {
                m_Assortment = value;
            }
        }
        public ICommand AddInAssort { get => new Command.ActionCommand((obj) => AddAssort(obj)); }
        public void AddAssort(object obj) 
        {
            context.Assortments.Add(m_Assortment);
            context.SaveChanges();
        }
    }
}
