/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class Arrow : SubComponent
    {
        [SerializeField] private float m_Width = 10;
        [SerializeField] private float m_Height = 15;
        [SerializeField] private float m_Offset = 0;
        [SerializeField] private float m_Dent = 3;
        [SerializeField] private Color32 m_Color = Color.clear;

        /// <summary>
        /// The widht of arrow.
        /// 箭头宽。
        /// </summary>
        public float width
        {
            get { return m_Width; }
            set { if (PropertyUtil.SetStruct(ref m_Width, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The height of arrow.
        /// 箭头高。
        /// </summary>
        public float height
        {
            get { return m_Height; }
            set { if (PropertyUtil.SetStruct(ref m_Height, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The offset of arrow.
        /// 箭头偏移。
        /// </summary>
        public float offset
        {
            get { return m_Offset; }
            set { if (PropertyUtil.SetStruct(ref m_Offset, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The dent of arrow.
        /// 箭头的凹度。
        /// </summary>
        public float dent
        {
            get { return m_Dent; }
            set { if (PropertyUtil.SetStruct(ref m_Dent, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// the color of arrow.
        /// 箭头颜色。
        /// </summary>
        public Color32 color
        {
            get { return m_Color; }
            set { if (PropertyUtil.SetColor(ref m_Color, value)) SetVerticesDirty(); }
        }

        public Arrow Clone()
        {
            var arrow = new Arrow();
            arrow.width = width;
            arrow.height = height;
            arrow.offset = offset;
            arrow.dent = dent;
            arrow.color = color;
            return arrow;
        }

        public void Copy(Arrow arrow)
        {
            width = arrow.width;
            height = arrow.height;
            offset = arrow.offset;
            dent = arrow.dent;
            color = arrow.color;
        }

        public Color32 GetColor(Color32 defaultColor)
        {
            if (ChartHelper.IsClearColor(color)) return defaultColor;
            else return color;
        }
    }
}