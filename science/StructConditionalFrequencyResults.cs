using System;
using System.Collections.Generic;

namespace Science
{
    struct ConditionalFrequencyResults
    {
        public Dictionary<(string, string), int> data;
        public List<string> Param1Values;
        public List<string> Param2Values;
    }
    struct MultyConditionalFrequencyResults
    {
        public Dictionary<(string, string), int> data;
        public List<List<string>> Param1Values;
        public List<string> Param2Values;
    }
    struct HiSquare
    {
        public Dictionary<string, (int, int)> data;
        public double LevelOfConfidence;
    }

    struct HomogeneityResult
    {
        public double Result;
        public Dictionary<double, double> Param1Probability;
        public Dictionary<double, double> Param2Probability;
    }
}