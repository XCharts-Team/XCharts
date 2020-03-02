using System.Threading;
/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// the animation of serie.
    /// 动画表现。
    /// </summary>
    [System.Serializable]
    public class SerieAnimation : SubComponent
    {
        public enum Easing
        {
            Linear,
        }
        [SerializeField] private bool m_Enable = true;
        [SerializeField] private Easing m_Easting;
        [SerializeField] private int m_Threshold = 2000;
        [SerializeField] private float m_FadeInDuration = 1000;
        [SerializeField] private float m_FadeInDelay = 0;
        [SerializeField] private float m_FadeOutDuration = 1000f;
        [SerializeField] private bool m_DataChangeEnable = true;
        [SerializeField] private float m_DataChangeDuration = 500;
        [SerializeField] private float m_ActualDuration;

        /// <summary>
        /// Whether to enable animation.
        /// 是否开启动画效果。
        /// </summary>
        public bool enable { get { return m_Enable; } set { m_Enable = value; } }
        /// <summary>
        /// Easing method used for the first animation. 
        /// 动画的缓动效果。
        /// </summary>
        //public Easing easing { get { return m_Easting; } set { m_Easting = value; } }
        /// <summary>
        /// The milliseconds duration of the fadeIn animation.
        /// 设定的渐入动画时长（毫秒）。
        /// </summary>
        public float fadeInDuration { get { return m_FadeInDuration; } set { m_FadeInDuration = value < 0 ? 0 : value; } }
        /// <summary>
        /// The milliseconds duration of the fadeOut animation.
        /// 设定的渐出动画时长（毫秒）。
        /// </summary>
        public float fadeOutDuration { get { return m_FadeOutDuration; } set { m_FadeOutDuration = value < 0 ? 0 : value; } }
        /// <summary>
        /// The milliseconds actual duration of the first animation.
        /// 实际的动画时长（毫秒）。
        /// </summary>
        public float actualDuration { get { return m_ActualDuration; } }
        /// <summary>
        /// Whether to set graphic number threshold to animation. Animation will be disabled when graphic number is larger than threshold.
        /// 是否开启动画的阈值，当单个系列显示的图形数量大于这个阈值时会关闭动画。
        /// </summary>
        public int threshold { get { return m_Threshold; } set { m_Threshold = value; } }
        /// <summary>
        /// The milliseconds delay before updating the first animation.
        /// 动画延时（毫秒）。
        /// </summary>
        public float delay { get { return m_FadeInDelay; } set { m_FadeInDelay = value < 0 ? 0 : value; } }
        /// <summary>
        /// 是否开启数据变更动画。
        /// </summary>
        public bool dataChangeEnable { get { return m_DataChangeEnable; } set { m_DataChangeEnable = value; } }
        /// <summary>
        /// The milliseconds duration of the data change animation.
        /// 数据变更的动画时长（毫秒）。
        /// </summary>
        public float dataChangeDuration { get { return m_DataChangeDuration; } set { m_DataChangeDuration = value < 0 ? 0 : value; } }

        private Dictionary<int, float> m_DataAnimationState = new Dictionary<int, float>();
        private bool m_FadeIn = false;
        private bool m_IsEnd = true;
        private bool m_IsPause = false;
        private bool m_FadeOut = false;
        private bool m_FadeOuted = false;
        private bool m_IsInit = false;

        private float startTime { get; set; }
        private int m_CurrDataProgress { get; set; }
        private int m_DestDataProgress { get; set; }
        [SerializeField] private float m_CurrDetailProgress;
        [SerializeField] private float m_DestDetailProgress;
        private float m_CurrSymbolProgress;

        public void FadeIn()
        {
            if (m_FadeOut) return;
            if (m_IsPause)
            {
                m_IsPause = false;
                return;
            }
            if (m_FadeIn) return;
            startTime = Time.time;
            m_FadeIn = true;
            m_IsEnd = false;
            m_IsInit = false;
            m_IsPause = false;
            m_FadeOuted = false;
            m_CurrDataProgress = 1;
            m_DestDataProgress = 1;
            m_CurrDetailProgress = 0;
            m_DestDetailProgress = 1;
            m_CurrSymbolProgress = 0;
            m_DataAnimationState.Clear();
        }

        public void Restart()
        {
            Reset();
            FadeIn();
        }

        public void FadeOut()
        {
            if (m_IsPause)
            {
                m_IsPause = false;
                return;
            }
            m_FadeOut = true;
            startTime = Time.time;
            m_FadeIn = true;
            m_IsEnd = false;
            m_IsInit = false;
            m_IsPause = false;
            m_CurrDataProgress = 0;
            m_DestDataProgress = 0;
            m_CurrDetailProgress = 0;
            m_DestDetailProgress = 1;
            m_CurrSymbolProgress = 0;
            m_DataAnimationState.Clear();
        }

        public void Pause()
        {
            if (!m_IsPause)
            {
                m_IsPause = true;
            }
        }

        public void Resume()
        {
            if (m_IsPause)
            {
                m_IsPause = false;
            }
        }

        private void End()
        {
            if (m_IsEnd) return;
            m_ActualDuration = (int)((Time.time - startTime) * 1000) - delay;
            m_CurrDataProgress = m_DestDataProgress + (m_FadeOut ? -1 : 1);
            m_FadeIn = false;
            m_IsEnd = true;
            m_IsInit = false;
            if (m_FadeOut)
            {
                m_FadeOut = false;
                m_FadeOuted = true;
            }
        }

        public void Reset()
        {
            m_FadeIn = false;
            m_IsEnd = true;
            m_IsInit = false;
            m_IsPause = false;
            m_FadeOut = false;
            m_FadeOuted = false;
            m_DataAnimationState.Clear();
        }

        public void InitProgress(int data, float curr, float dest)
        {
            if (m_IsInit || m_IsEnd) return;
            if (curr > dest) return;
            m_IsInit = true;
            m_DestDataProgress = data;

            if (m_FadeOut)
            {
                m_CurrDetailProgress = dest;
                m_DestDetailProgress = curr;
            }
            else
            {
                m_CurrDetailProgress = curr;
                m_DestDetailProgress = dest;
            }
        }

        public void SetDataFinish(int dataIndex)
        {
            if (m_IsEnd) return;
            m_CurrDataProgress = dataIndex + (m_FadeOut ? -1 : 1);
        }

        private void SetDataState(int index, float state)
        {
            m_DataAnimationState[index] = state;
        }

        private float GetDataState(int index, float dest)
        {
            if (IsInDelay()) return dest;
            if (!m_DataAnimationState.ContainsKey(index))
            {
                m_DataAnimationState.Add(index, dest);
            }
            return m_DataAnimationState[index];
        }

        public bool IsFinish()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return true;
#endif
            return !enable || m_IsEnd || (m_CurrDataProgress > m_DestDataProgress && m_CurrDetailProgress > m_DestDetailProgress);
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
            if (m_FadeOut) return dataIndex > 0;
            else return dataIndex <= m_CurrDataProgress;
        }

        internal void CheckProgress(float total)
        {
            if (IsFinish()) return;
            if (!m_IsInit || m_IsPause || m_IsEnd) return;
            if (IsInDelay()) return;
            m_ActualDuration = (int)((Time.time - startTime) * 1000) - delay;
            var duration = GetCurrAnimationDuration();
            var delta = total / duration * Time.deltaTime;
            if (m_FadeOut)
            {
                m_CurrDetailProgress -= delta;
                if (m_CurrDetailProgress <= m_DestDetailProgress)
                {
                    m_CurrDetailProgress = m_DestDetailProgress;
                    End();
                }
            }
            else
            {
                m_CurrDetailProgress += delta;
                if (m_CurrDetailProgress >= m_DestDetailProgress)
                {
                    m_CurrDetailProgress = m_DestDetailProgress;
                    End();
                }
            }
        }

        internal float GetCurrAnimationDuration()
        {
            if (m_FadeOut) return m_FadeOutDuration > 0 ? m_FadeOutDuration / 1000 : 1;
            else return m_FadeInDuration > 0 ? m_FadeInDuration / 1000 : 1;
        }

        internal float CheckBarProgress(int dataIndex, float barHig)
        {
            //if (!m_IsInit) return barHig;
            var destHig = m_FadeOut ? barHig : 0;
            if (IsInDelay() || IsFinish() || m_IsEnd)
            {
                return m_FadeOuted ? 0 : barHig;
            }
            else if (m_IsPause)
            {
                return GetDataState(dataIndex, destHig);
            }
            else
            {
                var duration = GetCurrAnimationDuration();
                var delta = barHig / duration * Time.deltaTime;
                var currHig = GetDataState(dataIndex, destHig) + (m_FadeOut ? -delta : delta);
                SetDataState(dataIndex, currHig);
                if (m_FadeOut)
                {
                    if (currHig <= 0)
                    {
                        End();
                        currHig = 0;
                    }
                }
                else if (Mathf.Abs(currHig) >= Mathf.Abs(barHig))
                {
                    End();
                    currHig = barHig;
                }
                return currHig;
            }
        }

        internal void CheckSymbol(float dest)
        {
            if (!enable || m_IsEnd || m_IsPause || !m_IsInit) return;
            if (IsInDelay()) return;
            var duration = GetCurrAnimationDuration();
            var delta = dest / duration * Time.deltaTime;
            if (m_FadeOut)
            {
                m_CurrSymbolProgress -= delta;
                if (m_CurrSymbolProgress < 0) m_CurrSymbolProgress = 0;
            }
            else
            {
                m_CurrSymbolProgress += delta;
                if (m_CurrSymbolProgress > dest) m_CurrSymbolProgress = dest;
            }
        }

        public float GetSysmbolSize(float dest)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return dest;
#endif
            if (!enable) return dest;
            if (m_IsEnd) return m_FadeOut ? 0 : dest;
            return m_CurrSymbolProgress;
        }

        public float GetCurrDetail()
        {
            return m_CurrDetailProgress;
        }

        public float GetCurrRate()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return 1;
#endif
            if (!enable || m_IsEnd) return 1;
            return m_CurrDetailProgress;
        }

        public int GetCurrIndex()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return -1;
#endif
            if (!enable || m_IsEnd) return -1;
            return (int)m_CurrDetailProgress;
        }

        public float GetCurrData()
        {
            return m_CurrDataProgress;
        }

        public float GetUpdateAnimationDuration()
        {
            if (m_Enable && m_DataChangeEnable && IsFinish()) return m_DataChangeDuration;
            else return 0;
        }

        public bool HasFadeOut()
        {
            return enable && m_FadeOuted && m_IsEnd;
        }
    }
}