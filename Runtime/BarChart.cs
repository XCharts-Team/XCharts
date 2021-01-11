/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XCharts
{
    [AddComponentMenu("XCharts/BarChart", 14)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public partial class BarChart : CoordinateChart
    {
        protected override void Awake()
        {
            base.Awake();
            raycastTarget = false;
        }
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            title.text = "BarChart";
            tooltip.type = Tooltip.Type.Shadow;
            RemoveData();
            SerieTemplate.AddDefaultBarSerie(this, "serie1");
            for (int i = 0; i < 5; i++)
            {
                AddXAxisData("x" + (i + 1));
            }
        }
#endif

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (m_OnPointerClickBar == null) return;
            if (pointerPos == Vector2.zero) return;
            UpdateTooltipValue(pointerPos);
            var dataIndex = tooltip.runtimeDataIndex[0];
            if (dataIndex >= 0)
            {
                m_OnPointerClickBar(eventData, dataIndex);
            }
        }
    }
}
