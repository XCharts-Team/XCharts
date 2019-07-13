using System;
using UnityEngine;

namespace XCharts
{
    [Serializable]
    public class AxisName
    {
        [Serializable]
        public enum Location
        {
            Start,
            Middle,
            End
        }
        [SerializeField] private bool m_Show;
        [SerializeField] private string m_Name;
        [SerializeField] private Location m_Location;
        [SerializeField] private float m_Gap;
        [SerializeField] private float m_Rotate;
        [SerializeField] private Color m_Color;
        [SerializeField] private int m_FontSize;
        [SerializeField] private FontStyle m_FontStyle;

        public bool show { get { return m_Show; } set { m_Show = value; } }
        public string name { get { return m_Name; } set { m_Name = value; } }
        public Location location { get { return m_Location; } set { m_Location = value; } }
        public float gap { get { return m_Gap; } set { m_Gap = value; } }
        public float rotate { get { return m_Rotate; } set { m_Rotate = value; } }
        public Color color { get { return m_Color; } set { m_Color = value; } }
        public int fontSize { get { return m_FontSize; } set { m_FontSize = value; } }
        public FontStyle fontStyle { get { return m_FontStyle; } set { m_FontStyle = value; } }

        public static AxisName defaultAxisName
        {
            get
            {
                return new AxisName()
                {
                    m_Show = false,
                    m_Name = "axisName",
                    m_Location = Location.End,
                    m_Gap = 5,
                    m_Rotate = 0,
                    m_Color = Color.clear,
                    m_FontSize = 18,
                    m_FontStyle = FontStyle.Normal
                };
            }
        }

        public void Copy(AxisName other)
        {
            m_Show = other.show;
            m_Name = other.name;
            m_Location = other.location;
            m_Gap = other.gap;
            m_Rotate = other.rotate;
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
            var other = (AxisName)obj;
            return m_Show == other.show &&
                m_Name.Equals(other.name) &&
                m_Location == other.location &&
                m_Gap == other.gap &&
                m_Rotate == other.rotate &&
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