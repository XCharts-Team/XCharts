/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XUGL;

namespace XCharts
{
    internal class DrawSerieTemplate : IDrawSerie
    {
        public BaseChart chart;

        public DrawSerieTemplate(BaseChart chart)
        {
            this.chart = chart;
        }

        public void InitComponent()
        {
        }
        public void CheckComponent()
        {
        }

        public void Update()
        {
        }

        public void DrawBase(VertexHelper vh)
        {
        }

        public void DrawSerie(VertexHelper vh, Serie serie)
        {
        }

        public void RefreshLabel()
        {
        }

        public bool CheckTootipArea(Vector2 local)
        {
            return false;
        }

        public bool OnLegendButtonClick(int index, string legendName, bool show)
        {
            return false;
        }

        public bool OnLegendButtonEnter(int index, string legendName)
        {
            return false;
        }

        public bool OnLegendButtonExit(int index, string legendName)
        {
            return false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }
    }
}