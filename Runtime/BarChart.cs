/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

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
        protected Action<PointerEventData, int> m_OnPointerClickBar;

        protected override void Awake()
        {
            base.Awake();
            raycastTarget = false;
        }
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Title.text = "BarChart";
            m_Tooltip.type = Tooltip.Type.Shadow;
            RemoveData();
            AddSerie(SerieType.Bar, "serie1");
            for (int i = 0; i < 5; i++)
            {
                AddXAxisData("x" + (i + 1));
                AddData(0, UnityEngine.Random.Range(10, 90));
            }
        }
#endif

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (m_OnPointerClickBar == null) return;
            if (pointerPos == Vector2.zero) return;
            UpdateTooltipValue(pointerPos);
            var dataIndex = m_Tooltip.runtimeDataIndex[0];
            if (dataIndex >= 0)
            {
                m_OnPointerClickBar(eventData, dataIndex);
            }
        }
    }
}
