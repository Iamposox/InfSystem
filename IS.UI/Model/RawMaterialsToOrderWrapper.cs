using IS.Domain.Model;
using IS.UI.ViewModel;
using System.Windows.Input;

namespace IS.UI.Model
{
    public class RawMaterialsToOrderWrapper
    {
        private readonly RawMaterialsToOrder rawMaterial;
        public event SelectedItemDelegate ItemSelected;
        public RawMaterialsToOrderWrapper(RawMaterialsToOrder _rawMaterial)
        {
            rawMaterial = _rawMaterial;
        }

        public RawMaterialsToOrder GetRawMaterialsToOrder { get => rawMaterial; }

        public string Name { get => rawMaterial.Material.Name; }
        public double Price
        {
            get => rawMaterial.Price;
            set
            {
                rawMaterial.Price = value;
            }
        }
        public ICommand Selected
        {
            get => new Command.ActionCommand((obj) =>
            {
                ItemSelected?.Invoke(this, rawMaterial);
            });
        }
    }
}
