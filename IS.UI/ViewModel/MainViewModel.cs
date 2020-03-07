using IS.UI.Model;

namespace IS.UI.ViewModel
{
    public class MainViewModel : Abstract.BindableObject
    {
        public NavigationModel CurrentView { get; set; }
        
        public MainViewModel()
        {
            Manager.ApplicationManager.GetInstance.NewNavigationRequested += GetInstance_NewNavigationRequested;
        }

        private void GetInstance_NewNavigationRequested(object _sender, NavigationModel _navigateTo)
        {
            CurrentView = _navigateTo;
            OnPropertyChanged(nameof(CurrentView));
        }
    }
}
