using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// 多样式数值。
    /// </summary>
    [Since("v3.8.0")]
    [System.Serializable]
    public class MLValue : ChildComponent
    {
        public enum Type
        {
            /// <summary>
            /// Percent form.
            /// |百分比形式。
            /// </summary>
            Percent,
            /// <summary>
            /// Absolute form.
            /// |绝对值形式。
            /// </summary>
            Absolute,
            /// <summary>
            /// Extra form.
            /// |额外形式。
            /// </summary>
            Extra
        }
        [SerializeField] private Type m_Type;
        [SerializeField] private float m_Value;

        public Type type { get { return m_Type; } set { m_Type = value; } }
        public float value { get { return m_Value; } set { m_Value = value; } }

        public MLValue(Type type, float value)
        {
            m_Type = type;
            m_Value = value;
        }

        public MLValue(float value)
        {
            m_Type = Type.Percent;
            m_Value = value;
        }

        public float GetValue(float total)
        {
            switch (m_Type)
            {
                case Type.Percent:
                    return m_Value * total;
                case Type.Absolute:
                    return m_Value;
                case Type.Extra:
                    return total + m_Value;
                default: return 0;
            }
        }
    }
}