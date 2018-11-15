using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Science
{
    /// <summary>
    /// Логика взаимодействия для ScienceHome.xaml
    /// </summary>
    public partial class ScienceHome : Page
    {
        InputHandler inputHandler = new InputHandler();
        public ScienceHome()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Открыть Excel Файл и распарсить его
            inputHandler = new InputHandler(); 
            StatisticData statisticData = inputHandler.OpenFile();

            //Отправить Данные на работу
            ScienceExcelReport scienceExcelReport = new ScienceExcelReport(statisticData);
            this.NavigationService.Navigate(scienceExcelReport);
        }
    }
}
