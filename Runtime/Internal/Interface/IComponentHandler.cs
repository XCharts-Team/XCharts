/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts
{
    public interface IComponentHandler
    {
        void Init();
        void Update();
        void DrawBase(VertexHelper vh);
        void DrawTop(VertexHelper vh);
        void OnDrag(PointerEventData eventData);
        void OnBeginDrag(PointerEventData eventData);
        void OnEndDrag(PointerEventData eventData);
        void OnPointerDown(PointerEventData eventData);
        void OnScroll(PointerEventData eventData);

    }
}