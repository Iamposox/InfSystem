using IS.Domain;
using IS.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;

namespace IS.UI.ViewModel
{
    public class CustomerViewModel : Abstract.BindableObject
    {
        readonly Context context;
        public ObservableCollection<Customer> Customers { get; set; }

        public CustomerViewModel()
        {
            context = new Context();
            Customers = new ObservableCollection<Customer>(context.Customers.ToList());
        }


    }
}
