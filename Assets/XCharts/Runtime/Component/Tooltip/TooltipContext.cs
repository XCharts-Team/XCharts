/******************************************/
/*                                        */
/*     Copyright (c) 2021 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
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
        public TooltipData data = new TooltipData();
    }
}