using UnityEngine;

namespace XCharts.Runtime
{
    public class InteractData
    {
        private float m_PreviousValue = 0;
        private float m_CurrentValue = float.NaN;
        private float m_TargetValue = float.NaN;
        private Vector3 m_PreviousPosition = Vector3.one;
        private Vector3 m_TargetPosition = Vector3.one;
        private Color32 m_PreviousColor = ColorUtil.clearColor32;
        private Color32 m_TargetColor = ColorUtil.clearColor32;
        private Color32 m_PreviousToColor = ColorUtil.clearColor32;
        private Color32 m_TargetToColor = ColorUtil.clearColor32;
        private float m_UpdateTime = 0;
        private bool m_UpdateFlag = false;
        private bool m_ValueEnable = false;

        internal float targetVaue { get { return m_TargetValue; } }
        internal float previousValue { get { return m_PreviousValue; } }
        internal bool valueEnable { get { return m_ValueEnable; } }
        internal bool updateFlag { get { return m_UpdateFlag; } }

        public override string ToString()
        {
            return string.Format("m_PreviousValue:{0},m_TargetValue:{1},m_UpdateTime:{2},m_UpdateFlag:{3},m_ValueEnable:{4},m_PreviousPosition:{5},m_TargetPosition:{6}",
            m_PreviousValue, m_TargetValue, m_UpdateTime, m_UpdateFlag, m_ValueEnable, m_PreviousPosition, m_TargetPosition);
        }

        public void SetValue(ref bool needInteract, float value, bool highlight, float rate = 1.3f)
        {
            value = highlight && rate != 0 ? value * rate : value;
            SetValue(ref needInteract, value);
        }

        public void SetValue(ref bool needInteract, float value, bool previousValueZero = false)
        {
            if (m_TargetValue != value)
            {
                needInteract = true;
                if (!m_ValueEnable)
                    m_PreviousValue = previousValueZero ? 0 : value;
                else
                    m_PreviousValue = m_CurrentValue;
                UpdateStart();
                m_TargetValue = value;
            }
            else if (m_UpdateFlag)
            {
                needInteract = true;
            }
        }

        public void SetPosition(ref bool needInteract, Vector3 pos)
        {
            if (m_TargetPosition != pos)
            {
                needInteract = true;
                UpdateStart();
                m_PreviousPosition = m_TargetPosition == Vector3.one ? pos : m_TargetPosition;
                m_TargetPosition = pos;
            }
        }

        public void SetColor(ref bool needInteract, Color32 color)
        {
            if (!ChartHelper.IsValueEqualsColor(color, m_TargetColor))
            {
                needInteract = true;
                UpdateStart();
                m_PreviousColor = ChartHelper.IsClearColor(m_TargetColor) ? color : m_TargetColor;
                m_TargetColor = color;
            }
            else if (m_UpdateFlag)
            {
                needInteract = true;
            }
        }
        public void SetColor(ref bool needInteract, Color32 color, Color32 toColor)
        {
            SetColor(ref needInteract, color);
            if (!ChartHelper.IsValueEqualsColor(toColor, m_TargetToColor))
            {
                needInteract = true;
                UpdateStart();
                m_PreviousToColor = ChartHelper.IsClearColor(m_TargetToColor) ? color : m_TargetToColor;
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
            if (!IsValueEnable() || animationDuration == 0)
                return false;
            if (float.IsNaN(m_TargetValue))
                return false;
            if (m_UpdateFlag && !float.IsNaN(m_PreviousValue))
            {
                var rate = GetRate(animationDuration);
                if (rate < 1)
                {
                    interacting = true;
                    value = Mathf.Lerp(m_PreviousValue, m_TargetValue, rate);
                    m_CurrentValue = value;
                    return true;
                }
                else
                {
                    UpdateEnd();
                }
            }
            value = m_TargetValue;
            return true;
        }

        public bool TryGetPosition(ref Vector3 pos, ref bool interacting, float animationDuration = 250)
        {
            if (!IsValueEnable() || animationDuration == 0)
                return false;
            if (m_TargetPosition == Vector3.one)
            {
                return false;
            }
            if (m_UpdateFlag && m_PreviousPosition != Vector3.one)
            {
                var rate = GetRate(animationDuration);
                if (rate < 1)
                {
                    interacting = true;
                    pos = Vector3.Lerp(m_PreviousPosition, m_TargetPosition, rate);
                    return true;
                }
                else
                {
                    UpdateEnd();
                }
            }
            pos = m_TargetPosition;
            return true;
        }

        public bool TryGetColor(ref Color32 color, ref bool interacting, float animationDuration = 250)
        {
            if (!IsValueEnable() || animationDuration == 0)
                return false;
            if (m_UpdateFlag)
            {
                var rate = GetRate(animationDuration);
                if (rate < 1)
                {
                    interacting = true;
                    color = Color32.Lerp(m_PreviousColor, m_TargetColor, rate);
                    return true;
                }
                else
                {
                    UpdateEnd();
                }
            }
            color = m_TargetColor;
            return true;
        }

        public bool TryGetColor(ref Color32 color, ref Color32 toColor, ref bool interacting, float animationDuration = 250)
        {
            if (!IsValueEnable() || animationDuration == 0)
                return false;
            if (m_UpdateFlag)
            {
                var rate = GetRate(animationDuration);
                if (rate < 1)
                {
                    interacting = true;
                    color = Color32.Lerp(m_PreviousColor, m_TargetColor, rate);
                    toColor = Color32.Lerp(m_PreviousToColor, m_TargetToColor, rate);
                    return true;
                }
                else
                {
                    UpdateEnd();
                }
            }
            color = m_TargetColor;
            toColor = m_TargetToColor;
            return true;
        }
        public bool TryGetValueAndColor(ref float value, ref Color32 color, ref Color32 toColor, ref bool interacting, float animationDuration = 250)
        {
            if (!IsValueEnable() || animationDuration == 0)
                return false;
            if (float.IsNaN(m_TargetValue))
                return false;
            if (m_UpdateFlag && !float.IsNaN(m_PreviousValue))
            {
                var rate = GetRate(animationDuration);
                if (rate < 1)
                {
                    interacting = true;
                    value = Mathf.Lerp(m_PreviousValue, m_TargetValue, rate);
                    color = Color32.Lerp(m_PreviousColor, m_TargetColor, rate);
                    toColor = Color32.Lerp(m_PreviousToColor, m_TargetToColor, rate);
                    m_CurrentValue = value;
                    return true;
                }
                else
                {
                    UpdateEnd();
                }
            }
            value = m_TargetValue;
            color = m_TargetColor;
            toColor = m_TargetToColor;
            return true;
        }

        private float GetRate(float animationDuration)
        {
            var time = Time.time - m_UpdateTime;
            var total = animationDuration / 1000;
            var rate = time / total;
            if (rate > 1) rate = 1;
            return rate;
        }

        private void UpdateStart()
        {
            m_ValueEnable = true;
            m_UpdateFlag = true;
            m_UpdateTime = Time.time;
        }

        private void UpdateEnd()
        {
            if (!m_UpdateFlag) return;
            m_UpdateFlag = false;
            m_PreviousColor = m_TargetColor;
            m_PreviousToColor = m_TargetToColor;
            m_PreviousValue = m_TargetValue;
            m_CurrentValue = m_TargetValue;
            m_PreviousPosition = m_TargetPosition;
        }

        public bool TryGetValueAndColor(ref float value, ref Vector3 pos, ref Color32 color, ref Color32 toColor, ref bool interacting, float animationDuration = 250)
        {
            var flag = TryGetValueAndColor(ref value, ref color, ref toColor, ref interacting, animationDuration);
            flag |= TryGetPosition(ref pos, ref interacting, animationDuration);
            return flag;
        }

        public bool TryGetValueAndColor(ref float value, ref Vector3 pos, ref bool interacting, float animationDuration = 250)
        {
            var flag = TryGetValue(ref value, ref interacting, animationDuration);
            flag |= TryGetPosition(ref pos, ref interacting, animationDuration);
            return flag;
        }

        public void Reset()
        {
            m_UpdateFlag = false;
            m_ValueEnable = false;
            m_TargetValue = float.NaN;
            m_PreviousValue = float.NaN;
            m_CurrentValue = float.NaN;
            m_PreviousPosition = Vector3.one;
            m_TargetPosition = Vector3.one;
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