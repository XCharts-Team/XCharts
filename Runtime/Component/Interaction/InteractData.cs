using UnityEngine;

namespace XCharts.Runtime
{
    public class InteractData
    {
        private float m_PreviousValue = 0;
        private float m_TargetValue = 0;
        private Color32 m_PreviousColor;
        private Color32 m_TargetColor;
        private Color32 m_PreviousToColor;
        private Color32 m_TargetToColor;
        private float m_UpdateTime = 0;
        private bool m_UpdateFlag = false;
        private bool m_ValueEnable = false;

        internal float targetVaue { get { return m_TargetValue; } }

        public void SetValue(ref bool needInteract, float size, bool highlight, float rate = 1.3f)
        {
            size = highlight ? size * rate : size;
            SetValue(ref needInteract, size);
        }

        public void SetValue(ref bool needInteract, float size)
        {
            if (m_TargetValue != size)
            {
                needInteract = true;
                m_UpdateFlag = true;
                m_ValueEnable = true;
                m_UpdateTime = Time.time;
                m_PreviousValue = m_TargetValue;
                m_TargetValue = size;
            }
        }

        public void SetColor(ref bool needInteract, Color32 color)
        {
            if (!ChartHelper.IsValueEqualsColor(color, m_TargetColor))
            {
                if (!ChartHelper.IsClearColor(m_TargetColor))
                {
                    needInteract = true;
                    m_UpdateFlag = true;
                    m_ValueEnable = true;
                    m_UpdateTime = Time.time;
                    m_PreviousColor = m_TargetColor;
                }
                m_TargetColor = color;
            }
        }
        public void SetColor(ref bool needInteract, Color32 color, Color32 toColor)
        {
            SetColor(ref needInteract, color);
            if (!ChartHelper.IsValueEqualsColor(toColor, m_TargetToColor))
            {
                if (!ChartHelper.IsClearColor(m_TargetToColor))
                {
                    needInteract = true;
                    m_UpdateFlag = true;
                    m_ValueEnable = true;
                    m_UpdateTime = Time.time;
                    m_PreviousToColor = m_TargetToColor;
                }
                m_TargetToColor = toColor;
            }
        }

        public void SetValueAndColor(ref bool needInteract, float value, Color32 color)
        {
            SetValue(ref needInteract, value);
            SetColor(ref needInteract, color);
        }

        public void SetValueAndColor(ref bool needInteract, float value, Color32 color, Color32 toColor)
        {
            SetValue(ref needInteract, value);
            SetColor(ref needInteract, color, toColor);
        }

        public bool TryGetValue(ref float value, ref bool interacting, float animationDuration = 250)
        {
            if (!IsValueEnable() || m_PreviousValue == 0)
                return false;
            if (m_UpdateFlag)
            {
                var time = Time.time - m_UpdateTime;
                var total = animationDuration / 1000;
                var rate = time / total;
                if (rate > 1) rate = 1;
                if (rate < 1)
                {
                    interacting = true;
                    value = Mathf.Lerp(m_PreviousValue, m_TargetValue, rate);
                    return true;
                }
                else
                {
                    m_UpdateFlag = false;
                }
            }
            value = m_TargetValue;
            return true;
        }

        public bool TryGetColor(ref Color32 color, ref bool interacting, float animationDuration = 250)
        {
            if (!IsValueEnable())
                return false;
            if (m_UpdateFlag)
            {
                var time = Time.time - m_UpdateTime;
                var total = animationDuration / 1000;
                var rate = time / total;
                if (rate > 1) rate = 1;
                if (rate < 1)
                {
                    interacting = true;
                    color = Color32.Lerp(m_PreviousColor, m_TargetColor, rate);
                    return true;
                }
                else
                {
                    m_UpdateFlag = false;
                }
            }
            color = m_TargetColor;
            return true;
        }

        public bool TryGetColor(ref Color32 color, ref Color32 toColor, ref bool interacting, float animationDuration = 250)
        {
            if (!IsValueEnable())
                return false;
            if (m_UpdateFlag)
            {
                var time = Time.time - m_UpdateTime;
                var total = animationDuration / 1000;
                var rate = time / total;
                if (rate > 1) rate = 1;
                if (rate < 1)
                {
                    interacting = true;
                    color = Color32.Lerp(m_PreviousColor, m_TargetColor, rate);
                    toColor = Color32.Lerp(m_PreviousToColor, m_TargetToColor, rate);
                    return true;
                }
                else
                {
                    m_UpdateFlag = false;
                }
            }
            color = m_TargetColor;
            toColor = m_TargetToColor;
            return true;
        }
        public bool TryGetValueAndColor(ref float value, ref Color32 color, ref Color32 toColor, ref bool interacting, float animationDuration = 250)
        {
            if (!IsValueEnable())
                return false;
            if (m_UpdateFlag)
            {
                var time = Time.time - m_UpdateTime;
                var total = animationDuration / 1000;
                var rate = time / total;
                if (rate > 1) rate = 1;
                if (rate < 1)
                {
                    interacting = true;
                    value = Mathf.Lerp(m_PreviousValue, m_TargetValue, rate);
                    color = Color32.Lerp(m_PreviousColor, m_TargetColor, rate);
                    toColor = Color32.Lerp(m_PreviousToColor, m_TargetToColor, rate);
                    return true;
                }
                else
                {
                    m_UpdateFlag = false;
                }
            }
            value = m_TargetValue;
            color = m_TargetColor;
            toColor = m_TargetToColor;
            return true;
        }

        public void Reset()
        {
            m_UpdateFlag = false;
            m_ValueEnable = false;
            m_PreviousValue = float.NaN;
            m_TargetColor = ColorUtil.clearColor32;
            m_TargetToColor = ColorUtil.clearColor32;
            m_PreviousColor = ColorUtil.clearColor32;
            m_PreviousToColor = ColorUtil.clearColor32;
        }

        private bool IsValueEnable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return false;
#endif
            return m_ValueEnable;
        }
    }
}