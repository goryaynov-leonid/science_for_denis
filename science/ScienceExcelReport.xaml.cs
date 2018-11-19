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
    /// Логика взаимодействия для ScienceExcelReport.xaml
    /// </summary>
    public partial class ScienceExcelReport : Page
    {
        public ScienceExcelReport()
        {
            InitializeComponent();
        }

        private StatisticData data; // Разобраться с реестром объектов... Судя по всему какая-то хрень получается...

        public ScienceExcelReport(object statisticData):this()
        {
            this.data = (StatisticData)statisticData; //Преобразование объекта из класса Object к строгому классу


            //Всё что ниже придётся вывести в отдельный метод... 
            //На начало экрана добавить кнопки с выбором способа обработки... 
            //В зависимости от которого и перестраивать форму
            
            //Строим сетку из кода. Тренируюсь с возможностями
            GridMain.ColumnDefinitions.Add(new ColumnDefinition());
            GridMain.ColumnDefinitions.Add(new ColumnDefinition());
            GridMain.ColumnDefinitions.Add(new ColumnDefinition());
            GridMain.RowDefinitions.Add(new RowDefinition());
            GridMain.RowDefinitions.Add(new RowDefinition());

            //Отображаем два списка параметров
            ListBox listOfFirstParametr = new ListBox
            {
                ItemsSource = this.data.Header
            };
            listOfFirstParametr.SelectionMode = SelectionMode.Extended;
            ListBox listOfSecondParametr = new ListBox
            {
                ItemsSource = this.data.Header
            };


            GridMain.Children.Add(listOfFirstParametr);
            Grid.SetColumn(listOfFirstParametr, 0);
            Grid.SetRow(listOfFirstParametr, 1);
            GridMain.Children.Add(listOfSecondParametr);
            Grid.SetColumn(listOfSecondParametr, 1);
            Grid.SetRow(listOfSecondParametr, 1);


            //Добавялем кнопку для генерации таблицы условных частот
            Button buttonConditionalFrequency = new Button();
            buttonConditionalFrequency.Content = "Таблица условных частот";
            buttonConditionalFrequency.AddHandler(Button.ClickEvent, new RoutedEventHandler(buttonConditionalFrequencyHandler));

            Grid.SetRow(buttonConditionalFrequency, 0);
            Grid.SetColumn(buttonConditionalFrequency, 1);
            GridMain.Children.Add(buttonConditionalFrequency);

            //Добавялем кнопку для оценки Хи квадрат
            Button buttonHiSquare = new Button();
            buttonHiSquare.Content = "Хи квадрат";
            buttonHiSquare.AddHandler(Button.ClickEvent, new RoutedEventHandler(buttonHiSquareClickerHandler));

            Grid.SetRow(buttonHiSquare, 0);
            Grid.SetColumn(buttonHiSquare, 0);
            GridMain.Children.Add(buttonHiSquare);

            //Добавляем кнопку дял проверки однородности
            Button buttonCheckHomogeneity = new Button();
            buttonCheckHomogeneity.Content = "Проверка однородности";
            buttonCheckHomogeneity.AddHandler(Button.ClickEvent, new RoutedEventHandler(buttonCheckHomogeneityClickerHandler));

            Grid.SetRow(buttonCheckHomogeneity, 0);
            Grid.SetColumn(buttonCheckHomogeneity, 2);
            GridMain.Children.Add(buttonCheckHomogeneity);


            InitializeComponent();
        }

        public void buttonCheckHomogeneityClickerHandler(object snder, RoutedEventArgs e)
        {
            ListBox listFirstParameter = (ListBox)GridMain.Children[0];
            ListBox listSecondParameter = (ListBox)GridMain.Children[1];

            int firstParametr = listFirstParameter.SelectedIndex;
            int secondParametr = listSecondParameter.SelectedIndex;

            HomogeneityResult result = this.data.CountHomogeneityParam(firstParametr, secondParametr);
        }

        public void buttonHiSquareClickerHandler(object sender, RoutedEventArgs e)
        {
            ListBox listFirstParameter = (ListBox)GridMain.Children[0];
            ListBox listSecondParameter = (ListBox)GridMain.Children[1];

            //По логике выбираем один элемент из первого листа

            int firstParametr = listFirstParameter.SelectedIndex;
            int secondParametr = listSecondParameter.SelectedIndex;

        }

        public void buttonConditionalFrequencyHandler(object sender, RoutedEventArgs e)
        {
            //Преобразовываем элементы MainGrid
            ListBox listFirstParameter = (ListBox)GridMain.Children[0];
            ListBox listSecondParameter = (ListBox)GridMain.Children[1];


            if (listFirstParameter.SelectedItems.Count == 1)
            {

                //Определяем интересующие нас параметры
                int firstParametr = listFirstParameter.SelectedIndex;
                int secondParametr = listSecondParameter.SelectedIndex;

                //Получаем значения для матрицы
                ConditionalFrequencyResults results = this.data.CountCounditionalFrequency(firstParametr, secondParametr);

                //Вызываем страницу для 
                SimpleConditionalFrequencyPage simpleConditionalFrequencyPage = new SimpleConditionalFrequencyPage(results);
                this.NavigationService.Navigate(simpleConditionalFrequencyPage);
            }
            else
            {
                List<int> selectedParams = new List<int>();
                

                foreach (var item in listFirstParameter.SelectedItems)
                {
                    selectedParams.Add(this.data.Header.IndexOf(item.ToString()));
                }

                MultyConditionalFrequencyResults results = this.data.CountMultyConditionalFrequency(selectedParams, listSecondParameter.SelectedIndex);
                SimpleConditionalFrequencyPage simpleConditionalFrequencyPage = new SimpleConditionalFrequencyPage(results, true);
                this.NavigationService.Navigate(simpleConditionalFrequencyPage);
            }
        }  
    }
}
