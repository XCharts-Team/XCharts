using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// the animation of serie.
    /// 动画表现。
    /// </summary>
    [System.Serializable]
    public class Animation
    {
        public enum Easing
        {
            Linear,
        }
        [SerializeField] private bool m_Enable = true;
        [SerializeField] private Easing m_Easting;
        [SerializeField] private int m_Duration = 1000;
        [SerializeField] private int m_Threshold = 2000;
        [SerializeField] private int m_Delay = 0;
        [SerializeField] private int m_ActualDuration;

        /// <summary>
        /// Whether to enable animation.
        /// 是否开启动画效果。
        /// </summary>
        public bool enable { get { return m_Enable; } set { m_Enable = value; } }
        /// <summary>
        /// Easing method used for the first animation. 
        /// 动画的缓动效果。
        /// </summary>
        /// <value></value>
        public Easing easing { get { return m_Easting; } set { m_Easting = value; } }
        /// <summary>
        /// The milliseconds duration of the first animation.
        /// 设定的动画时长（毫秒）。
        /// </summary>
        /// <value></value>
        public int duration { get { return m_Duration; } set { m_Duration = value; } }
        /// <summary>
        /// The milliseconds actual duration of the first animation.
        /// 实际的动画时长（毫秒）。
        /// </summary>
        /// <value></value>
        public int actualDuration { get { return m_ActualDuration; } }
        /// <summary>
        /// Whether to set graphic number threshold to animation. Animation will be disabled when graphic number is larger than threshold.
        /// 是否开启动画的阈值，当单个系列显示的图形数量大于这个阈值时会关闭动画。
        /// </summary>
        /// <value></value>
        public int threshold { get { return m_Threshold; } set { m_Threshold = value; } }
        /// <summary>
        /// The milliseconds delay before updating the first animation.
        /// 动画延时。
        /// </summary>
        /// <value></value>
        public int delay { get { return m_Delay; } set { m_Delay = value; if (m_Delay < 0) m_Delay = 0; } }

        private Dictionary<int, float> m_DataAnimationState = new Dictionary<int, float>();
        private bool m_IsStart = false;
        private bool m_IsEnd = true;
        private bool m_Inited = false;

        private float startTime { get; set; }
        private int m_CurrDataProgress { get; set; }
        private int m_DestDataProgress { get; set; }
        [SerializeField] private float m_CurrDetailProgress;
        [SerializeField] private float m_DestDetailProgress;
        private float m_CurrSymbolProgress;

        public void Start()
        {
            if (m_IsStart) return;
            startTime = Time.time;
            m_IsStart = true;
            m_IsEnd = false;
            m_Inited = false;
            m_CurrDataProgress = 1;
            m_DestDataProgress = 1;
            m_CurrDetailProgress = 0;
            m_DestDetailProgress = 1;
            m_CurrSymbolProgress = 0;
            m_DataAnimationState.Clear();
        }

        public void Stop()
        {
            m_IsStart = false;
            m_IsEnd = true;
            m_Inited = false;
            m_DataAnimationState.Clear();
        }

        public void End()
        {
            if (m_IsEnd) return;
            m_ActualDuration = (int)((Time.time - startTime) * 1000) - delay;
            m_CurrDataProgress = m_DestDataProgress + 1;
            m_IsEnd = true;
        }

        public void InitProgress(int data, float curr, float dest)
        {
            if (!m_Inited)
            {
                m_Inited = true;
                m_DestDataProgress = data;
                m_CurrDetailProgress = curr;
                m_DestDetailProgress = dest;
            }
        }

        public void SetDataFinish(int dataIndex)
        {
            if (!m_IsEnd)
            {
                m_CurrDataProgress = dataIndex + 1;
            }
        }

        public void SetDataState(int index, float state)
        {
            m_DataAnimationState[index] = state;
        }

        public float GetDataState(int index)
        {
            if (IsInDelay()) return 0;
            if (!m_DataAnimationState.ContainsKey(index))
            {
                m_DataAnimationState.Add(index, 0);
            }
            return m_DataAnimationState[index];
        }

        public bool IsFinish()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return true;
#endif
            return !enable || (m_CurrDataProgress > m_DestDataProgress && m_CurrDetailProgress > m_DestDetailProgress);
        }

        public bool IsInDelay()
        {
            return (delay > 0 && Time.time - startTime < delay / 1000);
        }

        public bool CheckDetailBreak(int dataIndex, float detail)
        {
            return !IsFinish() && detail > m_CurrDetailProgress;
        }

        public bool CheckDetailBreak(Vector3 pos, bool isYAxis)
        {
            if (IsFinish()) return false;
            if (isYAxis) return pos.y > m_CurrDetailProgress;
            else return pos.x > m_CurrDetailProgress;
        }

        public bool NeedAnimation(int dataIndex)
        {
            if (!m_Enable || m_IsEnd) return true;
            if (IsInDelay()) return false;
            return dataIndex <= m_CurrDataProgress;
        }

        public void CheckProgress(float delta)
        {
            if (!enable) return;
            if (IsInDelay()) return;
            if (m_IsEnd) return;
            if (m_CurrDetailProgress > m_DestDetailProgress)
            {
                End();
            }
            else
            {
                m_ActualDuration = (int)((Time.time - startTime) * 1000) - delay;
                m_CurrDetailProgress += delta;
            }
        }

        public void CheckSymbol(float delta, float dest)
        {
            m_CurrSymbolProgress += delta;
            if (m_CurrSymbolProgress > dest) m_CurrSymbolProgress = dest;
        }

        public float GetSysmbolSize(float dest)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return dest;
#endif
            if (!enable || m_IsEnd) return dest;
            return m_CurrSymbolProgress;
        }

        public float GetCurrDetail()
        {
            return m_CurrDetailProgress;
        }

        public float GetCurrData()
        {
            return m_CurrDataProgress;
        }
    }
}