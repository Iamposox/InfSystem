using IS.Domain;
using IS.UI.Manager;
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
        private Visibility m_LoginVisibility = ApplicationManager.GetInstance.CurrentUser == null ? Visibility.Visible :Visibility.Collapsed;
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
        public SeriesCollection PieDataTwo { get; private set; }

        public Visibility DashboardInfo { get => m_LoginVisibility == Visibility.Visible ? Visibility.Collapsed:Visibility.Visible; }

        public ICommand LogoutCommand { get => new Command.ActionCommand((obj) => { Manager.ApplicationManager.GetInstance.LogOut();}); }
      
        public DashboardViewModel()
        {
            Manager.ApplicationManager.GetInstance.ValuesChangedNotification += GetInstance_ValuesChangedNotification;
            PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

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
                .OrderByDescending(x => x.InAssortment).Take(5).ToList();
            foreach (var item in leastInAssortiment)
            {
                PieData.Add(new PieSeries()
                {
                    Title = item.Product.Name,
                    Values = new ChartValues<double> { item.InAssortment },
                    DataLabels = true
                });
            }
            PieDataTwo = new SeriesCollection();
            var leastInRawMaterials = context.RawMaterials.OrderByDescending(x => x.Amount).Take(5).ToList();
            foreach (var item in leastInRawMaterials)
            {
                PieDataTwo.Add(new PieSeries()
                {
                    Title = item.Name,
                    Values = new ChartValues<double> { item.Amount },
                    DataLabels = true
                });
            }
        }
        
        public Func<ChartPoint, string> PointLabel { get; set; }
        public Func<ChartPoint, string> PointLabelTwo { get; set; }

        private void GetInstance_ValuesChangedNotification(object _sender)
        {
            if (Manager.ApplicationManager.GetInstance.CurrentUser != null)
                LoginVisibility = Visibility.Collapsed;
            else
                LoginVisibility = Visibility.Visible;
        }
    }
}
