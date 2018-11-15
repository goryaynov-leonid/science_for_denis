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
    /// Логика взаимодействия для SimpleConditionalFrequencyPage.xaml
    /// </summary>
    public partial class SimpleConditionalFrequencyPage : Page
    {
        public SimpleConditionalFrequencyPage()
        {
            InitializeComponent();
        }

        public void AddButtonSaveAsCSV()
        {
            //Добавялем кнопку для экспорта в CSV
            Button buttonSaveAsCSV = new Button();
            buttonSaveAsCSV.Content = "Сохранить как CSV";
            buttonSaveAsCSV.AddHandler(Button.ClickEvent, new RoutedEventHandler(buttonSaveAsCSVClickHandler));

            Grid.SetRow(buttonSaveAsCSV, 0);
            Grid.SetColumn(buttonSaveAsCSV, 0);
            grid.Children.Add(buttonSaveAsCSV);
        }

        List<string> multyParamList = new List<string>();
        List<List<string>> multyParamElems = new List<List<string>>();
        public void AddNewMultyParamElem(int numberOfParam,StringBuilder stringBuilder)
        {
            if (numberOfParam == multyParamElems.Count - 1)
            {
                foreach(var item in multyParamElems[numberOfParam])
                {
                    string s =stringBuilder.ToString() + item;
                    multyParamList.Add(s);
                }
            }
            else
            {
                foreach (var item in multyParamElems[numberOfParam])
                {
                    stringBuilder.Insert(stringBuilder.Length, item);
                    AddNewMultyParamElem(numberOfParam + 1, stringBuilder.Insert(stringBuilder.Length,"|"));
                    stringBuilder.Remove(stringBuilder.Length - item.Length-1, item.Length+1);
                }
            }
        }

        public SimpleConditionalFrequencyPage(object data, bool m):this()
        {
            AddButtonSaveAsCSV();
            
            MultyConditionalFrequencyResults results = (MultyConditionalFrequencyResults)data;
            //Считаем сколько потребуется строк
            multyParamElems = results.Param1Values;
            AddNewMultyParamElem(0, new StringBuilder());

            grid.RowDefinitions.Add(new RowDefinition());
            foreach (var item in multyParamList)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            foreach (var item in results.Param2Values)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i=0; i<multyParamList.Count;i++)
            {
                Label label = new Label();
                label.Content = multyParamList[i];
                Grid.SetColumn(label, 0);
                Grid.SetRow(label, i + 1);
                grid.Children.Add(label);
            }
            for (int i=0; i<results.Param2Values.Count;i++)
            {
                Label label = new Label();
                label.Content = results.Param2Values[i];
                Grid.SetColumn(label, i+1);
                Grid.SetRow(label, 0);
                grid.Children.Add(label);
            }
            for (int i=0; i<multyParamList.Count;i++)
                for (int j=0; j<results.Param2Values.Count;j++)
                {
                    Label label = new Label();

                    if (results.data.ContainsKey((multyParamList[i], results.Param2Values[j])))
                    {
                        label.Content = Convert.ToString(results.data[((multyParamList[i], results.Param2Values[j]))]);
                    }
                    else
                        label.Content = "0";

                    Grid.SetColumn(label, j+1);
                    Grid.SetRow(label, i + 1);
                    grid.Children.Add(label);
                }

            InitializeComponent();
        }
        public SimpleConditionalFrequencyPage(object data) : this()
        {

            ConditionalFrequencyResults results = (ConditionalFrequencyResults)data;
            //Создаём сетку для отображения
            for (int i = 0; i <= results.Param2Values.Count + 1; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i <= results.Param1Values.Count + 1; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            //Отдельный ряд для кнопок
            grid.RowDefinitions.Add(new RowDefinition());

            //Выводим значения Второго параметра 0ая строка по столбцам(начиная с 1ого столбца)
            Label label = new Label();
            for (int i = 1; i <= results.Param2Values.Count; i++)
            {
                label = new Label();
                label.Content = results.Param2Values[i - 1];
                grid.Children.Add(label);
                Grid.SetColumn(label, i);
                Grid.SetRow(label, 0);
            }
            //Выводим значения Первого параметра 0ый столбец(начиная с 1ой строки)
            for (int i = 1; i <= results.Param1Values.Count; i++)
            {
                label = new Label();
                label.Content = results.Param1Values[i - 1];
                grid.Children.Add(label);
                Grid.SetColumn(label, 0);
                Grid.SetRow(label, i);
            }
            
            for (int i=0; i<results.Param1Values.Count;i++)
            {
                for (int j=0; j<results.Param2Values.Count;j++)
                {
                    label = new Label();
                    grid.Children.Add(label);
                    Grid.SetColumn(label, j+1);
                    Grid.SetRow(label, i+1);

                    if (results.data.ContainsKey((results.Param1Values[i], results.Param2Values[j])))
                    {
                        label.Content = results.data[(results.Param1Values[i], results.Param2Values[j])];
                    }
                    else
                        label.Content = 0;
                }
            }
            AddButtonSaveAsCSV();
            InitializeComponent();
        }


        public void buttonSaveAsCSVClickHandler(object sender, RoutedEventArgs e)
        {

        }
    }
}