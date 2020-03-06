using IS.Domain;
using IS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace IS.UI.ViewModel
{
    public class RawMaterialsViewModel: Abstract.BindableObject
    {
        readonly Context context;
        public ObservableCollection<RawMaterial> rawMaterials { get; set; }
        public RawMaterialsViewModel() 
        {
            context = new Context();
            rawMaterials = new ObservableCollection<RawMaterial>(context.RawMaterials.ToList());
        }
    }
}
