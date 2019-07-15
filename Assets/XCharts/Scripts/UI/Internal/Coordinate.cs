using System;
using UnityEngine;

namespace XCharts
{
    [Serializable]
    public class Coordinate : IEquatable<Coordinate>
    {
        [SerializeField] private float m_Left;
        [SerializeField] private float m_Right;
        [SerializeField] private float m_Top;
        [SerializeField] private float m_Bottom;
        [SerializeField] private float m_Tickness;
        [SerializeField] private int m_FontSize;

        public float left { get { return m_Left; } set { m_Left = value; } }
        public float right { get { return m_Right; } set { m_Right = value; } }
        public float top { get { return m_Top; } set { m_Top = value; } }
        public float bottom { get { return m_Bottom; } set { m_Bottom = value; } }
        public float tickness { get { return m_Tickness; } set { m_Tickness = value; } }
        public int fontSize { get { return m_FontSize; } set { m_FontSize = value; } }

        public static Coordinate defaultCoordinate
        {
            get
            {
                var coordinate = new Coordinate
                {
                    m_Left = 50,
                    m_Right = 30,
                    m_Top = 50,
                    m_Bottom = 30,
                    m_Tickness = 0.6f,
                    m_FontSize = 16,
                };
                return coordinate;
            }
        }
        public void Copy(Coordinate other)
        {
            m_Left = other.left;
            m_Right = other.right;
            m_Top = other.top;
            m_Bottom = other.bottom;
            m_Tickness = other.tickness;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            else if (obj is Coordinate)
            {
                return Equals((Coordinate)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Coordinate other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            return m_Left == other.left &&
                m_Right == other.right &&
                m_Top == other.top &&
                m_Bottom == other.bottom &&
                m_Tickness == other.tickness &&
                m_FontSize == other.fontSize;
        }

        public static bool operator ==(Coordinate left, Coordinate right)
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

        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}