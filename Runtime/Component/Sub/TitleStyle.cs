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

namespace XCharts
{
    /// <summary>
    /// the title of serie.
    /// 标题相关设置。
    /// </summary>
    [Serializable]
    public class TitleStyle : SubComponent, IEquatable<TitleStyle>
    {
        [SerializeField] private bool m_Show;
        [SerializeField] private TextStyle m_textStyle = new TextStyle(18);

        /// <summary>
        /// Whether to show title.
        /// 是否显示标题。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }

        /// <summary>
        /// the color of text. 
        /// 文本的颜色。
        /// </summary>
        public TextStyle textStyle { get { return m_textStyle; } set { m_textStyle = value; } }
        /// <summary>
        /// 
        /// </summary>
        public Text runtimeText { get; set; }

        public TitleStyle Clone()
        {
            var title = new TitleStyle();
            title.show = show;
            title.textStyle = textStyle.Clone();
            return title;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            else if (obj is TitleStyle)
            {
                return Equals((TitleStyle)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(TitleStyle other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            return textStyle.Equals(other.textStyle);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

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
                runtimeText.transform.localPosition = pos + new Vector3(m_textStyle.offset.x, m_textStyle.offset.y);
            }
        }

        public void SetText(string text)
        {
            if (runtimeText)
            {
                if (textStyle.color != Color.clear) runtimeText.color = textStyle.color;
                runtimeText.text = text;
            }
        }
    }
}