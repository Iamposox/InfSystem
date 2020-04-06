using IS.Domain;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace IS.UI.ViewModel
{
    public class DashboardViewModel : Abstract.BindableObject
    {
        private Visibility m_LoginVisibility = Visibility.Visible;
        public Visibility LoginVisibility 
        { 
            get => m_LoginVisibility;
            set
            {
                m_LoginVisibility = value;
                OnPropertyChanged(nameof(LoginVisibility));
                OnPropertyChanged(nameof(DashboardInfo));
            }
        }

        public SeriesCollection PieData { get;private set; }

        public Visibility DashboardInfo { get => m_LoginVisibility == Visibility.Visible ? Visibility.Collapsed:Visibility.Visible; }

        public ICommand LogoutCommand { get => new Command.ActionCommand((obj) => { Manager.ApplicationManager.GetInstance.LogOut();}); }
      
        public DashboardViewModel()
        {
            Manager.ApplicationManager.GetInstance.ValuesChangedNotification += GetInstance_ValuesChangedNotification;
            PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            SetData();
        }

        private void SetData()
        {
            PieData = new SeriesCollection();
            Context context = new Context();
            var leastInAssortiment = context
                .Assortments
                .Include(x=>x.Product)
                .OrderBy(x => x.InAssortment).Take(5).ToList();
            foreach (var item in leastInAssortiment)
            {
                PieData.Add(new PieSeries()
                {
                    Title = item.Product.Name,
                    Values = new ChartValues<double> { item.InAssortment },
                    DataLabels = true
                });
            }
        }

        public Func<ChartPoint, string> PointLabel { get; set; }

        private void GetInstance_ValuesChangedNotification(object _sender)
        {
            if (Manager.ApplicationManager.GetInstance.CurrentUser != null)
                LoginVisibility = Visibility.Collapsed;
            else
                LoginVisibility = Visibility.Visible;
        }
    }
}
