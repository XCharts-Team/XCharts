/*
/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

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
        [SerializeField] private TextStyle m_TextStyle = new TextStyle();

        /// <summary>
        /// Whether to show title.
        /// 是否显示标题。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// the color of text. 
        /// 文本的颜色。
        /// </summary>
        public TextStyle textStyle
        {
            get { return m_TextStyle; }
            set { if (PropertyUtil.SetClass(ref m_TextStyle, value, true)) SetComponentDirty(); }
        }

        public override bool componentDirty { get { return m_ComponentDirty || textStyle.componentDirty; } }

        public override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            textStyle.ClearComponentDirty();
        }

        public ChartText runtimeText { get; set; }

        public bool IsInited()
        {
            return runtimeText != null;
        }

        public void SetActive(bool active)
        {
            if (runtimeText != null)
            {
                runtimeText.SetActive(active);
            }
        }

        public void UpdatePosition(Vector3 pos)
        {
            if (runtimeText != null)
            {
                runtimeText.SetLocalPosition(pos + new Vector3(m_TextStyle.offset.x, m_TextStyle.offset.y));
            }
        }

        public void SetText(string text)
        {
            if (runtimeText == null) return;
            var oldText = runtimeText.GetText();
            if (oldText != null && !oldText.Equals(text))
            {
                if (!ChartHelper.IsClearColor(textStyle.color)) runtimeText.SetColor(textStyle.color);
                runtimeText.SetText(text);
            }
        }

        public void SetColor(Color color)
        {
            if (runtimeText != null)
            {
                runtimeText.SetColor(color);
            }
        }
    }
}