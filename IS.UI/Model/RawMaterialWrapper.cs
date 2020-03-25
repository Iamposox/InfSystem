using IS.Domain.Model;
using IS.UI.ViewModel;
using System.Windows.Input;

namespace IS.UI.Model
{
    public class RawMaterialWrapper
    {
        private RawMaterial rawMaterial;
        public event SelectedItemDelegate ItemSelected;
        public RawMaterialWrapper(RawMaterial _rawMaterial)
        {
            rawMaterial = _rawMaterial;
        }
        public RawMaterial GetMaterial { get => rawMaterial; }
        public double Amount { get => rawMaterial.Amount; }
        public string Name { get => rawMaterial.Name; }
        public ICommand Selected
        {
            get => new Command.ActionCommand((obj) =>
            {
                ItemSelected?.Invoke(this, rawMaterial);
            });
        }
    }
}
