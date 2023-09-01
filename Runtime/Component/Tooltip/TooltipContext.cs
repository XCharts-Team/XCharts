using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    public class TooltipData
    {
        public string title;
        public List<SerieParams> param = new List<SerieParams>();
    }

    public class TooltipContext
    {
        public Vector2 pointer;
        public float width;
        public float height;
        public float angle;
        public Tooltip.Type type;
        public Tooltip.Trigger trigger;
        public TooltipData data = new TooltipData();
    }
}