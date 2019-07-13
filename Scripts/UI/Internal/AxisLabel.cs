using System;
using UnityEngine;

namespace XCharts
{
    [Serializable]
    public class AxisLabel
    {
        [SerializeField] private bool m_Show;
        [SerializeField] private int m_Interval;
        [SerializeField] private bool m_Inside;
        [SerializeField] private float m_Rotate;
        [SerializeField] private float m_Margin;
        [SerializeField] private Color m_Color;
        [SerializeField] private int m_FontSize;
        [SerializeField] private FontStyle m_FontStyle;

        public bool show { get { return m_Show; } set { m_Show = value; } }
        public int interval { get { return m_Interval; } set { m_Interval = value; } }
        public bool inside { get { return m_Inside; } set { m_Inside = value; } }
        public float rotate { get { return m_Rotate; } set { m_Rotate = value; } }
        public float margin { get { return m_Margin; } set { m_Margin = value; } }
        public Color color { get { return m_Color; } set { m_Color = value; } }
        public int fontSize { get { return m_FontSize; } set { m_FontSize = value; } }
        public FontStyle fontStyle { get { return m_FontStyle; } set { m_FontStyle = value; } }

        public static AxisLabel defaultAxisLabel
        {
            get
            {
                return new AxisLabel()
                {
                    m_Show = true,
                    m_Interval = 0,
                    m_Inside = false,
                    m_Rotate = 0,
                    m_Margin = 8,
                    m_Color = Color.clear,
                    m_FontSize = 18,
                    m_FontStyle = FontStyle.Normal
                };
            }
        }
        public void Copy(AxisLabel other)
        {
            m_Show = other.show;
            m_Interval = other.interval;
            m_Inside = other.inside;
            m_Rotate = other.rotate;
            m_Margin = other.margin;
            m_Color = other.color;
            m_FontSize = other.fontSize;
            m_FontStyle = other.fontStyle;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var other = (AxisLabel)obj;
            return m_Show == other.show &&
                m_Interval.Equals(other.interval) &&
                m_Inside == other.inside &&
                m_Rotate == other.rotate &&
                m_Margin == other.margin &&
                m_Color == other.color &&
                m_FontSize == other.fontSize &&
                m_FontStyle == other.fontStyle;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}