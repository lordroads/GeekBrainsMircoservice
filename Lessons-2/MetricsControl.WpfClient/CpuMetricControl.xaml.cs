using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace MetricsControl.WpfClient
{
    /// <summary>
    /// Логика взаимодействия для MetricControl.xaml
    /// </summary>
    public partial class CpuMetricControl : UserControl, INotifyPropertyChanged
    {
        private SeriesCollection _columnSeriesValue;
        private MetricsControlClient _metricsControlClient;

        public event PropertyChangedEventHandler PropertyChanged;

        public SeriesCollection ColumnSeriesValues
        {
            get { return _columnSeriesValue; }
            set
            {
                _columnSeriesValue = value;
                OnPropertyChanged("ColumnSeriesValues");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public CpuMetricControl()
        {
            InitializeComponent();

            DataContext = this;
        }

        

        private async void UpdateOnСlick(object sender, RoutedEventArgs e)
        {
            if (_metricsControlClient is null)
            {
                _metricsControlClient = new MetricsControlClient("http://localhost:5004", new HttpClient());
            }

            try
            {
                TimeSpan toTime = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                TimeSpan fromTime = toTime - TimeSpan.FromSeconds(60);

                AllCpuMetricsApiResponse response = await _metricsControlClient.GetAllByIdAsync(
                    1,
                    fromTime.ToString("dd\\.hh\\:mm\\:ss"),
                    toTime.ToString("dd\\.hh\\:mm\\:ss")
                );

                if (response.Metrics.Count > 0)
                {

                    PercentDescriptionTextBlock.Text = $"За последние {TimeSpan.FromSeconds(response.Metrics.ToArray()[response.Metrics.Count - 1].Time - response.Metrics.ToArray()[0].Time)}\n средняя загрузка";

                    PercentTextBlock.Text = $"{response.Metrics.Where(x => x != null).Select(x => x.Value).ToArray().Sum(x => x) / response.Metrics.Count:F2}";
                }

                ColumnSeriesValues = new SeriesCollection
                {
                     new LineSeries
                    {
                        Fill = new SolidColorBrush(Color.FromArgb(125, 255, 0,0)),
                        Stroke = new SolidColorBrush(Color.FromRgb(255, 0,0)),
                        AreaLimit = 0,
                        Values = new ChartValues<ObservableValue>(response.Metrics.Where(x => x != null).Select(x => new ObservableValue(x.Value)).ToArray())
                    },
                     
                };

                TimePowerChart.Update(true);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }

    
}
