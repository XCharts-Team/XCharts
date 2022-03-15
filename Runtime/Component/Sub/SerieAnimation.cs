/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEngine;
using System;

namespace XCharts
{
    public delegate float CustomAnimationDelay(int dataIndex);
    public delegate float CustomAnimationDuration(int dataIndex);

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
        [SerializeField] private float m_FadeOutDelay = 0;
        [SerializeField] private bool m_DataChangeEnable = true;
        [SerializeField] private float m_DataChangeDuration = 500;
        [SerializeField] private float m_ActualDuration;
        [SerializeField] private bool m_AlongWithLinePath;
        /// <summary>
        /// 自定义渐入动画延时函数。返回ms值。
        /// </summary>
        public CustomAnimationDelay customFadeInDelay;
        /// <summary>
        /// 自定义渐入动画时长函数。返回ms值。
        /// </summary>
        public CustomAnimationDuration customFadeInDuration;
        /// <summary>
        /// 自定义渐出动画延时函数。返回ms值。
        /// </summary>
        public CustomAnimationDelay customFadeOutDelay;
        /// <summary>
        /// 自定义渐出动画时长函数。返回ms值。
        /// </summary>
        public CustomAnimationDuration customFadeOutDuration;

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
        /// 设定的渐入动画时长（毫秒）。如果要设置单个数据项的渐入时长，可以用代码定制：customFadeInDuration。
        /// </summary>
        public float fadeInDuration { get { return m_FadeInDuration; } set { m_FadeInDuration = value < 0 ? 0 : value; } }
        /// <summary>
        /// The milliseconds duration of the fadeOut animation.
        /// 设定的渐出动画时长（毫秒）。如果要设置单个数据项的渐出时长，可以用代码定制：customFadeOutDuration。
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
        /// 渐入动画延时（毫秒）。如果要设置单个数据项的延时，可以用代码定制：customFadeInDelay。
        /// </summary>
        public float fadeInDelay { get { return m_FadeInDelay; } set { m_FadeInDelay = value < 0 ? 0 : value; } }
        /// <summary>
        /// 渐出动画延时（毫秒）。如果要设置单个数据项的延时，可以用代码定制：customFadeOutDelay。
        /// </summary>
        public float fadeOutDelay { get { return m_FadeOutDelay; } set { m_FadeInDelay = value < 0 ? 0 : value; } }
        /// <summary>
        /// 是否开启数据变更动画。
        /// </summary>
        public bool dataChangeEnable { get { return m_DataChangeEnable; } set { m_DataChangeEnable = value; } }
        /// <summary>
        /// The milliseconds duration of the data change animation.
        /// 数据变更的动画时长（毫秒）。
        /// </summary>
        public float dataChangeDuration { get { return m_DataChangeDuration; } set { m_DataChangeDuration = value < 0 ? 0 : value; } }
        /// <summary>
        /// 是否沿着线的轨迹进行匀速动画。
        /// </summary>
        public bool alongWithLinePath { get { return m_AlongWithLinePath; } set { m_AlongWithLinePath = value; } }
        /// <summary>
        /// 渐入动画完成回调
        /// </summary>
        public Action fadeInFinishCallback { get; set; }
        /// <summary>
        /// 渐出动画完成回调
        /// </summary>
        public Action fadeOutFinishCallback { get; set; }
        private Dictionary<int, float> m_DataCurrProgress = new Dictionary<int, float>();
        private Dictionary<int, float> m_DataDestProgress = new Dictionary<int, float>();
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
        private Vector3 m_LinePathLastPos;
        private float m_LinePathCurrTotalDist = 0f;

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
            m_DataCurrProgress.Clear();
            m_DataDestProgress.Clear();
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
            m_DataCurrProgress.Clear();
            m_DataDestProgress.Clear();
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
            m_ActualDuration = (int)((Time.time - startTime) * 1000) - (m_FadeOut ? fadeOutDelay : fadeInDelay);
            m_CurrDataProgress = m_DestDataProgress + (m_FadeOut ? -1 : 1);
            m_IsEnd = true;
            m_IsInit = false;
            if (m_FadeIn)
            {
                m_FadeIn = false;
                if (fadeInFinishCallback != null)
                {
                    fadeInFinishCallback();
                }
            }
            if (m_FadeOut)
            {
                m_FadeOut = false;
                m_FadeOuted = true;
                if (fadeOutFinishCallback != null)
                {
                    fadeOutFinishCallback();
                }
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
            m_DataCurrProgress.Clear();
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

        private void SetDataCurrProgress(int index, float state)
        {
            m_DataCurrProgress[index] = state;
        }

        private float GetDataCurrProgress(int index, float initValue, float destValue, out bool isBarEnd)
        {
            if (IsInDelay())
            {
                isBarEnd = false;
                return initValue;
            }
            var c1 = !m_DataCurrProgress.ContainsKey(index);
            var c2 = !m_DataDestProgress.ContainsKey(index);
            if (c1 || c2)
            {
                if (c1) m_DataCurrProgress.Add(index, initValue);
                if (c2) m_DataDestProgress.Add(index, destValue);
                isBarEnd = false;
            }
            else
            {
                isBarEnd = m_DataCurrProgress[index] == m_DataDestProgress[index];
            }
            return m_DataCurrProgress[index];
        }

        public bool IsFinish()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return true;
#endif
            return !m_Enable || m_IsEnd || (m_CurrDataProgress > m_DestDataProgress && m_CurrDetailProgress > m_DestDetailProgress);
        }

        public bool IsInFadeOut()
        {
            return m_FadeOut;
        }

        public bool IsInDelay()
        {
            if (m_FadeOut) return (fadeOutDelay > 0 && Time.time - startTime < fadeOutDelay / 1000);
            return (fadeInDelay > 0 && Time.time - startTime < fadeInDelay / 1000);
        }

        public float GetDataDelay(int dataIndex)
        {
            if (m_FadeOut && customFadeOutDelay != null) return customFadeOutDelay(dataIndex);
            if (m_FadeIn && customFadeInDelay != null) return customFadeInDelay(dataIndex);
            return 0;
        }

        public bool IsInDataDelay(int dataIndex)
        {
            return Time.time - startTime < GetDataDelay(dataIndex) / 1000f;
        }

        public bool IsAllOutDelay(int dataCount)
        {
            var nowTime = Time.time - startTime;
            for (int i = 0; i < dataCount; i++)
            {
                if (nowTime < GetDataDelay(i) / 1000) return false;
            }
            return true;
        }

        public bool IsAllDataFinishProgress(int dataCount)
        {
            for (int i = 0; i < dataCount; i++)
            {
                if (m_DataDestProgress.ContainsKey(i) && m_DataCurrProgress.ContainsKey(i))
                {
                    if (m_DataCurrProgress[i] != m_DataDestProgress[i]) return false;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckDetailBreak(float detail)
        {
            return !IsFinish() && detail > m_CurrDetailProgress;
        }

        public void SetLinePathStartPos(Vector3 pos)
        {
            if (m_AlongWithLinePath)
            {
                m_LinePathLastPos = pos;
                m_LinePathCurrTotalDist = 0;
            }
        }

        public bool CheckDetailBreak(Vector3 pos, bool isYAxis)
        {
            if (IsFinish()) return false;
            if (m_AlongWithLinePath)
            {
                m_LinePathCurrTotalDist += Vector3.Distance(pos, m_LinePathLastPos);
                m_LinePathLastPos = pos;
                return CheckDetailBreak(m_LinePathCurrTotalDist);
            }
            else
            {
                if (isYAxis) return pos.y > m_CurrDetailProgress;
                else return pos.x > m_CurrDetailProgress;
            }
        }

        public bool NeedAnimation(int dataIndex)
        {
            if (!m_Enable || m_IsEnd) return true;
            if (IsInDelay()) return false;
            if (m_FadeOut) return dataIndex > 0;
            else return dataIndex <= m_CurrDataProgress;
        }

        public void CheckProgress(double total)
        {
            if (IsFinish()) return;
            if (!m_IsInit || m_IsPause || m_IsEnd) return;
            if (IsInDelay()) return;
            m_ActualDuration = (int)((Time.time - startTime) * 1000) - fadeInDelay;
            var duration = GetCurrAnimationDuration();
            var delta = (float)(total / duration * Time.deltaTime);
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

        public float GetCurrAnimationDuration(int dataIndex = -1)
        {
            if (dataIndex >= 0)
            {
                if (m_FadeOut && customFadeOutDuration != null) return customFadeOutDuration(dataIndex) / 1000f;
                if (m_FadeIn && customFadeInDuration != null) return customFadeInDuration(dataIndex) / 1000f;
            }
            if (m_FadeOut) return m_FadeOutDuration > 0 ? m_FadeOutDuration / 1000 : 1f;
            else return m_FadeInDuration > 0 ? m_FadeInDuration / 1000 : 1f;
        }

        public float CheckBarProgress(int dataIndex, float barHig, int dataCount, out bool isBarEnd)
        {
            isBarEnd = false;
            var initHig = m_FadeOut ? barHig : 0;
            var destHig = m_FadeOut ? 0 : barHig;
            var currHig = GetDataCurrProgress(dataIndex, initHig, destHig, out isBarEnd);
            if (isBarEnd || IsFinish())
            {
                return m_FadeOuted ? 0 : barHig;
            }
            else if (IsInDelay() || IsInDataDelay(dataIndex))
            {
                return m_FadeOut ? barHig : 0;
            }
            else if (m_IsPause)
            {
                return currHig;
            }
            else
            {
                var duration = GetCurrAnimationDuration(dataIndex);
                var delta = barHig / duration * Time.deltaTime;
                currHig = currHig + (m_FadeOut ? -delta : delta);
                if (m_FadeOut)
                {
                    if ((initHig > 0 && currHig <= 0) || (initHig < 0 && currHig >= 0))
                    {
                        currHig = 0;
                        isBarEnd = true;
                    }
                }
                else if (Mathf.Abs(currHig) >= Mathf.Abs(barHig))
                {
                    currHig = barHig;
                    isBarEnd = true;
                }
                SetDataCurrProgress(dataIndex, currHig);
                return currHig;
            }
        }

        public void AllBarEnd()
        {
            End();
        }

        public void CheckSymbol(float dest)
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