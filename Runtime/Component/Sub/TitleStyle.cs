/*
/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace XCharts
{
    /// <summary>
    /// the title of serie.
    /// 标题相关设置。
    /// </summary>
    [Serializable]
    public class TitleStyle : SubComponent
    {
        [SerializeField] private bool m_Show;
        [FormerlySerializedAs("m_textStyle")]
        [SerializeField] private TextStyle m_TextStyle = new TextStyle(18);

        /// <summary>
        /// Whether to show title.
        /// 是否显示标题。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtility.SetStruct(ref m_Show, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// the color of text. 
        /// 文本的颜色。
        /// </summary>
        public TextStyle textStyle
        {
            get { return m_TextStyle; }
            set { if (PropertyUtility.SetClass(ref m_TextStyle, value, true)) SetComponentDirty(); }
        }

        public override bool componentDirty { get { return m_ComponentDirty || textStyle.componentDirty; } }

        internal override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            textStyle.ClearComponentDirty();
        }

        public Text runtimeText { get; set; }

        public bool IsInited()
        {
            return runtimeText != null;
        }

        public void SetActive(bool active)
        {
            if (runtimeText)
            {
                ChartHelper.SetActive(runtimeText, active);
            }
        }

        public void UpdatePosition(Vector3 pos)
        {
            if (runtimeText)
            {
                runtimeText.transform.localPosition = pos + new Vector3(m_TextStyle.offset.x, m_TextStyle.offset.y);
            }
        }

        public void SetText(string text)
        {
            if (runtimeText && !runtimeText.text.Equals(text))
            {
                if (!ChartHelper.IsClearColor(textStyle.color)) runtimeText.color = textStyle.color;
                runtimeText.text = text;
            }
        }
    }
}