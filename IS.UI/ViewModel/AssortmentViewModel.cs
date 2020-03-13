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
        public ObservableCollection<Assortment> assortment { get; set; }
        public ObservableCollection<Product> products{ get; set; }
        public List<Product> product;
        public AssortmentViewModel() 
        {
            context = new Context();
            assortment = new ObservableCollection<Assortment>(context.Assortments.ToList());
            products = new ObservableCollection<Product>(context.Products.ToList());
        }
    }
}
