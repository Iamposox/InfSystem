using IS.Domain;
using IS.Domain.Model;
using IS.UI.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IS.UI.ViewModel
{
    public class RawMaterialsViewModel : Abstract.BindableObject
    {
        readonly Context context;
        public ObservableCollection<RawMaterialWrapper> rawMaterials { get; set; } = new ObservableCollection<RawMaterialWrapper>();
        private RawMaterial m_raw;
        public RawMaterialsViewModel()
        {
            context = new Context();
            context.RawMaterials.ToList().ForEach(x => rawMaterials.Add(new RawMaterialWrapper(x)));
            foreach (var item in rawMaterials)
                item.ItemSelected += Item_ItemSelected;
        }
        public RawMaterial EditerRawMaterial
        {
            get => m_raw;
            set
            {
                m_raw = value;
                Changed();
            }
        }
        private void Changed()
        {
            OnPropertyChanged(nameof(EditerRawMaterial));
            OnPropertyChanged(nameof(EditerRawMaterial.Name));
            OnPropertyChanged(nameof(EditerRawMaterial.Amount));
            OnPropertyChanged(nameof(rawMaterials));
        }
        public ICommand AddRaw
        {
            get => new Command.ActionCommand((obj) =>
            {
                if (EditerRawMaterial.Validate())
                {
                    if (EditerRawMaterial.ID == 0)
                        context.Add(EditerRawMaterial);
                    context.SaveChanges();
                    rawMaterials.Clear();
                    context.RawMaterials.ToList().ForEach(x => rawMaterials.Add(new RawMaterialWrapper(x)));
                    foreach (var item in rawMaterials)
                        item.ItemSelected += Item_ItemSelected;
                    EditerRawMaterial = new RawMaterial();
                    Changed();
                    context.SaveChanges();
                }
            });
        }
        private void Item_ItemSelected(object _sender, object _sendObject)
        {
            EditerRawMaterial = (RawMaterial)_sendObject;
        }
    }
}
