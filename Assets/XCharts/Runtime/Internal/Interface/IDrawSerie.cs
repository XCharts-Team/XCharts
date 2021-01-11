/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts
{
    public interface IDrawSerie
    {
        void InitComponent();
        void CheckComponent();
        void Update();
        void DrawBase(VertexHelper vh);
        void DrawSerie(VertexHelper vh, Serie serie);
        void RefreshLabel();
        bool CheckTootipArea(Vector2 local);
        bool OnLegendButtonClick(int index, string legendName, bool show);
        bool OnLegendButtonEnter(int index, string legendName);
        bool OnLegendButtonExit(int index, string legendName);
        void OnPointerDown(PointerEventData eventData);
    }
}