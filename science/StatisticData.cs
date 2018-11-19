using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Science
{
    class StatisticData
    {
        public List<String> Header { get; set; }
        public List<List<String>> Data { get; set; }

        public StatisticData(List<String> header, List<List<String>> data)
        {
            this.Header = header;
            this.Data = data;
        }

        public ConditionalFrequencyResults CountCounditionalFrequency(int index1, int index2)
        {
            // Используем последовательные обработки данных
            //Каждый последовательно вызываемый метод использует предыдущую коллекцию или группу
            //в качестве входных данных и применяет к ним следующее действие
            var counts = this.Data
                .GroupBy(row => (row[index1], row[index2]))
                .ToDictionary(x => x.Key, x => x.Count());

            //Выберем уникальные значения первого параметра
            var firstParamValue = (from x in this.Data
                                   orderby x[index1]
                                   select x[index1]).Distinct();
            
            //А теперь второго
            var secondParamValue = (from x in this.Data
                                    orderby x[index2]
                                    select x[index2]).Distinct();

            ConditionalFrequencyResults res;
            res.data = counts;
            res.Param1Values = firstParamValue.ToList<string>();
            res.Param2Values = secondParamValue.ToList<string>();

            return res;
        }

        public MultyConditionalFrequencyResults CountMultyConditionalFrequency(List<int> indexes, int index2)
        {

            List<List<string>> data = new List<List<string>>(this.Data);
            foreach (List<string> item in data)
            {
                item.Add(String.Join("|", indexes.Select(i => item[i])));
            }

            var counts = data
                .GroupBy(row => (row[data[0].Count - 1], row[index2]))
                .ToDictionary(x => x.Key, x => x.Count());

            List<List<string>> firstParamValue = new List<List<string>>();

            //Выберем уникальные значения первого параметра
            foreach (int item in indexes)
            {
                firstParamValue.Add(data.Select(x => x[item]).Distinct().OrderBy(x=>x).ToList());
            }

            //А теперь второго
            var secondParamValue = (from x in data
                                    orderby x[index2]
                                    select x[index2]).Distinct();

            MultyConditionalFrequencyResults res;
            res.data = counts;
            res.Param1Values = firstParamValue;
            res.Param2Values = secondParamValue.ToList<string>();

            return res;
        }

        public HomogeneityResult CountHomogeneityParam(int index1, int index2)
        {
            HomogeneityResult res = new HomogeneityResult();
            res.Param1Probability = new Dictionary<double, double>();
            res.Param2Probability = new Dictionary<double, double>();
            List<List<double>> SortedData = new List<List<double>>(2);
            SortedData.Add(new List<double>());
            SortedData.Add(new List<double>());
            for (int i = 0; i < Data.Count; i++)
            {
                SortedData[0].Add(Convert.ToDouble(this.Data[i][index1]));
            }

            for (int i = 0; i < Data.Count; i++)
            {
                SortedData[1].Add(Convert.ToDouble(this.Data[i][index2]));
            }

            SortedData[0].Sort((x, y) => x.CompareTo(y));
            SortedData[1].Sort((x, y) => x.CompareTo(y));
            int Count1 = 0;
            for (int i = 0; i < SortedData[0].Count; i++)
            {
                while (i < SortedData[0].Count - 1 && SortedData[0][i] == SortedData[0][i + 1])
                {
                    i++;
                    Count1++;
                }
                Count1++;
                res.Param1Probability.Add(SortedData[0][i], (double)Count1 / (double)SortedData[0].Count);
            }
            int Count2 = 0;
            for (int i = 0; i < SortedData[1].Count; i++)
            {
                while (i < SortedData[1].Count - 1 && SortedData[1][i] == SortedData[1][i + 1])
                {
                    i++;
                    Count2++;
                }
                Count2++;
                res.Param2Probability.Add(SortedData[1][i], (double)Count2 / (double)SortedData[1].Count);
            }

            

            return res;
        }
    }
}