using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Science
{
    class StatisticData
    {
        public List<string> Header { get; set; }
        public List<List<string>> Data { get; set; }
        public List<List<string>> NewData { get; set; }

        public StatisticData(List<string> header, List<List<string>> data)
        {
            this.Header = header;
            this.Data = data;
            List<List<string>> TempData = new List<List<string>>();
            for (int i = 0; i < this.Data[0].Count; i++)
            {
                TempData.Add(new List<string>());
                for (int j = 0; j < this.Data.Count; j++)
                {
                    TempData[i].Add(this.Data[j][i]);
                }
            }
            this.NewData = TempData;
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
                firstParamValue.Add(data.Select(x => x[item]).Distinct().OrderBy(x => x).ToList());
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
            res.ParamProbability = new Dictionary<double, double>[2];
            res.ParamProbability[0] = new Dictionary<double, double>();
            res.ParamProbability[1] = new Dictionary<double, double>();
            List<List<double>> SortedData = new List<List<double>>();
            SortedData.Add(new List<double>());
            SortedData.Add(new List<double>());
            
            //Выберем нужные два столбца
            SortedData[0].AddRange(this.NewData[index1].Select(x => Convert.ToDouble(x)));
            SortedData[1].AddRange(this.NewData[index2].Select(x => Convert.ToDouble(x)));

            int Count1 = 0;
            int Count2 = 0;

            //Посчитаем вероятности для первого столбца
            foreach (var group in SortedData[0].GroupBy(x => x).OrderBy(x => x.Key))
            {
                Count1 += group.Count();
                res.ParamProbability[0].Add(group.Key, (double)Count1 / (double)SortedData[0].Count);
            }

            //Посчитаем вероятности для второго столбца
            foreach (var group in SortedData[1].GroupBy(x => x).OrderBy(x => x.Key))
            {
                Count2 += group.Count();
                res.ParamProbability[1].Add(group.Key, (double)Count2 / (double)SortedData[0].Count);
            }

            double[] previousProbability = { 0, 0 };
            res.Result = 0;
            List<(double, int)> allData = new List<(double, int)>();
            allData.AddRange(SortedData[0].Distinct().Select(x => (x, 0)));
            allData.AddRange(SortedData[1].Distinct().Select(x => (x, 1)));
            allData.Sort();

            foreach (var tmpData in allData)
            {
                double tmp;
                if (res.ParamProbability[(tmpData.Item2 == 1 ? 0 : 1)].TryGetValue(tmpData.Item1, out tmp))
                {
                    res.Result = Math.Max(res.Result, Math.Abs(res.ParamProbability[0][tmpData.Item1] - res.ParamProbability[1][tmpData.Item1]));
                    previousProbability[0] = res.ParamProbability[0][tmpData.Item1];
                    previousProbability[1] = res.ParamProbability[1][tmpData.Item1];
                }
                else
                {
                    res.Result = Math.Max(res.Result, Math.Abs(res.ParamProbability[tmpData.Item2][tmpData.Item1] - previousProbability[(tmpData.Item2 == 1 ? 0 : 1)]));
                    previousProbability[tmpData.Item2] = res.ParamProbability[tmpData.Item2][tmpData.Item1];
                }
            }

            return res;
        }
    }
}