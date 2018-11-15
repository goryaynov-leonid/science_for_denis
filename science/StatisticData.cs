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
    }
}