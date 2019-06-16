using System;
using UnityEngine;

namespace XCharts
{
    [Serializable]
    public class Location : IPropertyChanged, IEquatable<Location>
    {
        public enum Align
        {
            TopLeft,
            TopRight,
            TopCenter,
            BottomLeft,
            BottomRight,
            BottomCenter,
            Center,
            CenterLeft,
            CenterRight
        }

        [SerializeField] private Align m_Align = Align.TopCenter;
        [SerializeField] private float m_Left;
        [SerializeField] private float m_Right;
        [SerializeField] private float m_Top;
        [SerializeField] private float m_Bottom;

        private TextAnchor m_TextAnchor;
        private Vector2 m_AnchorMin;
        private Vector2 m_AnchorMax;
        private Vector2 m_Pivot;

        public Align align { get { return m_Align; } set { m_Align = value; UpdateAlign(); } }
        public float left { get { return m_Left; } set { m_Left = value; UpdateAlign(); } }
        public float right { get { return m_Right; } set { m_Right = value; UpdateAlign(); } }
        public float top { get { return m_Top; } set { m_Top = value; UpdateAlign(); } }
        public float bottom { get { return m_Bottom; } set { m_Bottom = value; UpdateAlign(); } }

        public TextAnchor textAnchor { get { return m_TextAnchor; } }
        public Vector2 anchorMin { get { return m_AnchorMin; } }
        public Vector2 anchorMax { get { return m_AnchorMax; } }
        public Vector2 pivot { get { return m_Pivot; } }

        public static Location defaultLeft
        {
            get
            {
                return new Location()
                {
                    align = Align.CenterRight,
                    left = 5,
                    right = 0,
                    top = 0,
                    bottom = 0
                };
            }
        }

        public static Location defaultRight
        {
            get
            {
                return new Location()
                {
                    align = Align.CenterRight,
                    left = 0,
                    right = 5,
                    top = 0,
                    bottom = 0
                };
            }
        }

        public static Location defaultTop
        {
            get
            {
                return new Location()
                {
                    align = Align.TopCenter,
                    left = 0,
                    right = 0,
                    top = 5,
                    bottom = 0
                };
            }
        }

        public static Location defaultBottom
        {
            get
            {
                return new Location()
                {
                    align = Align.BottomCenter,
                    left = 0,
                    right = 0,
                    top = 0,
                    bottom = 5
                };
            }
        }

        private void UpdateAlign()
        {
            switch (m_Align)
            {
                case Align.BottomCenter:
                    m_TextAnchor = TextAnchor.LowerCenter;
                    m_AnchorMin = new Vector2(0.5f, 0);
                    m_AnchorMax = new Vector2(0.5f, 0);
                    m_Pivot = new Vector2(0.5f, 0);
                    break;
                case Align.BottomLeft:
                    m_TextAnchor = TextAnchor.LowerLeft;
                    m_AnchorMin = new Vector2(0, 0);
                    m_AnchorMax = new Vector2(0, 0);
                    m_Pivot = new Vector2(0, 0);
                    break;
                case Align.BottomRight:
                    m_TextAnchor = TextAnchor.LowerRight;
                    m_AnchorMin = new Vector2(1, 0);
                    m_AnchorMax = new Vector2(1, 0);
                    m_Pivot = new Vector2(1, 0);
                    break;
                case Align.Center:
                    m_TextAnchor = TextAnchor.MiddleCenter;
                    m_AnchorMin = new Vector2(0.5f, 0.5f);
                    m_AnchorMax = new Vector2(0.5f, 0.5f);
                    m_Pivot = new Vector2(0.5f, 0.5f);
                    break;
                case Align.CenterLeft:
                    m_TextAnchor = TextAnchor.MiddleLeft;
                    m_AnchorMin = new Vector2(0, 0.5f);
                    m_AnchorMax = new Vector2(0, 0.5f);
                    m_Pivot = new Vector2(0, 0.5f);
                    break;
                case Align.CenterRight:
                    m_TextAnchor = TextAnchor.MiddleRight;
                    m_AnchorMin = new Vector2(1, 0.5f);
                    m_AnchorMax = new Vector2(1, 0.5f);
                    m_Pivot = new Vector2(1, 0.5f);
                    break;
                case Align.TopCenter:
                    m_TextAnchor = TextAnchor.UpperCenter;
                    m_AnchorMin = new Vector2(0.5f, 1);
                    m_AnchorMax = new Vector2(0.5f, 1);
                    m_Pivot = new Vector2(0.5f, 1);
                    break;
                case Align.TopLeft:
                    m_TextAnchor = TextAnchor.UpperLeft;
                    m_AnchorMin = new Vector2(0, 1);
                    m_AnchorMax = new Vector2(0, 1);
                    m_Pivot = new Vector2(0, 1);
                    break;
                case Align.TopRight:
                    m_TextAnchor = TextAnchor.UpperRight;
                    m_AnchorMin = new Vector2(1, 1);
                    m_AnchorMax = new Vector2(1, 1);
                    m_Pivot = new Vector2(1, 1);
                    break;
                default:
                    break;
            }
        }

        public Vector2 GetPosition(float chartWidht, float chartHeight)
        {
            switch (align)
            {
                case Align.BottomCenter:
                    return new Vector2(chartWidht / 2, bottom);
                case Align.BottomLeft:
                    return new Vector2(left, bottom);
                case Align.BottomRight:
                    return new Vector2(chartWidht - right, bottom);
                case Align.Center:
                    return new Vector2(chartWidht / 2, chartHeight / 2);
                case Align.CenterLeft:
                    return new Vector2(left, chartHeight / 2);
                case Align.CenterRight:
                    return new Vector2(chartWidht - right, chartHeight / 2);
                case Align.TopCenter:
                    return new Vector2(chartWidht / 2, chartHeight - top);
                case Align.TopLeft:
                    return new Vector2(left, chartHeight - top);
                case Align.TopRight:
                    return new Vector2(chartWidht - right, chartHeight - top);
                default:
                    return Vector2.zero;
            }
        }

        public void Copy(Location location)
        {
            m_Align = location.align;
            m_Left = location.left;
            m_Right = location.right;
            m_Top = location.top;
            m_Bottom = location.bottom;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            else if (obj is Location)
            {
                return Equals((Location)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Location other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            return align == other.align &&
                left == other.left &&
                right == other.right &&
                top == other.top &&
                bottom == other.bottom;
        }

        public static bool operator ==(Location left, Location right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            {
                return true;
            }
            else if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }
            return Equals(left, right);
        }

        public static bool operator !=(Location left, Location right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void OnChanged()
        {
            UpdateAlign();
        }
    }
}
