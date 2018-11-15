using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic.FileIO;

namespace Science
{
    class InputHandler
    {
        public StatisticData OpenFile()
        {
            //Открытие файла на чтение
            String fileName = "";
            OpenFileDialog openFileDialog = new OpenFileDialog(); //TODO вставить фильтр на excel
            //openFileDialog.InitialDirectory = "C\\Users\\Denis\\OneDrive\\Документы\\Аспирантура";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                { 
                    fileName = openFileDialog.FileName;
                    return ParseCSV(fileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Problems with File" + ex.Message);
                }
            }
            //проверим csv
            // return ParseExcel(fileName);
            return ParseCSV(fileName);
        }

        private StatisticData ParseCSV (string CSVFileName)
        {
            TextFieldParser parser = new TextFieldParser(CSVFileName);

            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(";");

            string[] fields = parser.ReadFields();

            List<string> headers = new List<string>(fields);
            List<List<string>> data = new List<List<string>>();

            while (!parser.EndOfData)
            {
                fields = parser.ReadFields();
                data.Add(new List<string>(fields));

            }

            StatisticData statisticData = new StatisticData(headers,data);
            return statisticData;
        }

        private StatisticData ParseExcel(String ExcelFileName)
        {
            

            //Открытие Excel файла
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(ExcelFileName);
            Excel._Worksheet xlWorkSheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorkSheet.UsedRange;

            //Границы страницы

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;


            //Тут парсим данные создаём объект класса data на который и возвращаем ссылку дальше
            StatisticData statisticData;

            List<String> headers = new List<string>();
            List<List<string>> data = new List<List<string>>();
            for (int i = 1; i <= colCount; i++)
                headers.Add(Convert.ToString(xlRange[1, i].Value2));

            for (int i = 2; i <= rowCount; i++)
            {
                List<string> s = new List<string>();
                for (int j=1; j < colCount; j++)
                {
                    s.Add(Convert.ToString(xlRange[i, j].Value2));
                }
                data.Add(s);
            }

            statisticData = new StatisticData(headers, data);
            //Убиваем excel процесс
            GC.Collect();
            GC.WaitForPendingFinalizers();
            xlWorkbook.Close();
            xlApp.Quit();


            return statisticData;
        }
    }
}