using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public class SerieParams
    {
        public Type serieType;
        public int serieIndex;
        public string serieName;
        public string marker = "●";
        public string category;
        public int dimension;
        public SerieData serieData;
        public int dataCount;
        public double value;
        public double total;
        public Color32 color;
        public string itemFormatter;
        public string numericFormatter;
        public bool ignore;
        public List<string> columns = new List<string>();
    }
}