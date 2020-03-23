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
    public class RawMaterialsViewModel: Abstract.BindableObject
    {
        readonly Context context;
        public ObservableCollection<RawMaterial> rawMaterials { get; set; }
        private RawMaterial m_raw = new RawMaterial();
        private RawMaterial f_raw = new RawMaterial();
        public RawMaterialsViewModel() 
        {
            context = new Context();
            rawMaterials = new ObservableCollection<RawMaterial>(context.RawMaterials.ToList());
        }
        public RawMaterial Raw
        {
            get => m_raw;
            set 
            {
                m_raw = value;
            }
        }
        public ICommand AddRaw { get => new Command.ActionCommand((obj) => AddRawMate(obj)); }
        public void AddRawMate(object obj) 
        {
            context.RawMaterials.Add(m_raw);
            context.SaveChanges();
        }
    }
}
