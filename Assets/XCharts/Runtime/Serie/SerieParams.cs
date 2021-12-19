/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts
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
        public double value;
        public double total;
        public Color32 color;
        public string itemFormatter;
        public string numericFormatter;
        public List<string> columns = new List<string>();
    }
}