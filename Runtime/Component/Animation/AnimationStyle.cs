using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public enum AnimationType
    {
        /// <summary>
        /// he default. An animation playback mode will be selected according to the actual situation.
        /// |默认。内部会根据实际情况选择一种动画播放方式。
        /// </summary>
        Default,
        /// <summary>
        /// Play the animation from left to right.
        /// |从左往右播放动画。
        /// </summary>
        LeftToRight,
        /// <summary>
        /// Play the animation from bottom to top.
        /// |从下往上播放动画。
        /// </summary>
        BottomToTop,
        /// <summary>
        /// Play animations from the inside out.
        /// |由内到外播放动画。
        /// </summary>
        InsideOut,
        /// <summary>
        /// Play the animation along the path.
        /// |沿着路径播放动画。
        /// </summary>
        AlongPath,
        /// <summary>
        /// Play the animation clockwise.
        /// |顺时针播放动画。
        /// </summary>
        Clockwise,
    }

    public enum AnimationEasing
    {
        Linear,
    }

    /// <summary>
    /// the animation of serie.
    /// |动画表现。
    /// </summary>
    [System.Serializable]
    public class AnimationStyle : ChildComponent
    {
        [SerializeField] private bool m_Enable = true;
        [SerializeField] private AnimationType m_Type;
        [SerializeField] private AnimationEasing m_Easting;
        [SerializeField] private int m_Threshold = 2000;
        [SerializeField] private float m_FadeInDuration = 1000;
        [SerializeField] private float m_FadeInDelay = 0;
        [SerializeField] private float m_FadeOutDuration = 1000f;
        [SerializeField] private float m_FadeOutDelay = 0;
        [SerializeField] private bool m_DataChangeEnable = true;
        [SerializeField] private float m_DataChangeDuration = 500;
        [SerializeField] private float m_ActualDuration;
        /// <summary>
        /// 自定义渐入动画延时函数。返回ms值。
        /// </summary>
        public AnimationDelayFunction fadeInDelayFunction;
        /// <summary>
        /// 自定义渐入动画时长函数。返回ms值。
        /// </summary>
        public AnimationDurationFunction fadeInDurationFunction;
        /// <summary>
        /// 自定义渐出动画延时函数。返回ms值。
        /// </summary>
        public AnimationDelayFunction fadeOutDelayFunction;
        /// <summary>
        /// 自定义渐出动画时长函数。返回ms值。
        /// </summary>
        public AnimationDurationFunction fadeOutDurationFunction;
        public AnimationStyleContext context = new AnimationStyleContext();

        /// <summary>
        /// Whether to enable animation.
        /// |是否开启动画效果。
        /// </summary>
        public bool enable { get { return m_Enable; } set { m_Enable = value; } }
        /// <summary>
        /// The type of animation.
        /// |动画类型。
        /// </summary>
        public AnimationType type { get { return m_Type; } set { m_Type = value; } }
        /// <summary>
        /// Easing method used for the first animation.
        /// |动画的缓动效果。
        /// </summary>
        //public Easing easting { get { return m_Easting; } set { m_Easting = value; } }
        /// <summary>
        /// The milliseconds duration of the fadeIn animation.
        /// |设定的渐入动画时长（毫秒）。如果要设置单个数据项的渐入时长，可以用代码定制：customFadeInDuration。
        /// </summary>
        public float fadeInDuration { get { return m_FadeInDuration; } set { m_FadeInDuration = value < 0 ? 0 : value; } }
        /// <summary>
        /// The milliseconds duration of the fadeOut animation.
        /// |设定的渐出动画时长（毫秒）。如果要设置单个数据项的渐出时长，可以用代码定制：customFadeOutDuration。
        /// </summary>
        public float fadeOutDuration { get { return m_FadeOutDuration; } set { m_FadeOutDuration = value < 0 ? 0 : value; } }
        /// <summary>
        /// The milliseconds actual duration of the first animation.
        /// |实际的动画时长（毫秒）。
        /// </summary>
        public float actualDuration { get { return m_ActualDuration; } }
        /// <summary>
        /// Whether to set graphic number threshold to animation. Animation will be disabled when graphic number is larger than threshold.
        /// |是否开启动画的阈值，当单个系列显示的图形数量大于这个阈值时会关闭动画。
        /// </summary>
        public int threshold { get { return m_Threshold; } set { m_Threshold = value; } }
        /// <summary>
        /// The milliseconds delay before updating the first animation.
        /// |渐入动画延时（毫秒）。如果要设置单个数据项的延时，可以用代码定制：customFadeInDelay。
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
        /// |数据变更的动画时长（毫秒）。
        /// </summary>
        public float dataChangeDuration { get { return m_DataChangeDuration; } set { m_DataChangeDuration = value < 0 ? 0 : value; } }
        /// <summary>
        /// 渐入动画完成回调
        /// </summary>
        public Action fadeInFinishCallback { get; set; }
        /// <summary>
        /// 渐出动画完成回调
        /// </summary>
        public Action fadeOutFinishCallback { get; set; }
        private Dictionary<int, float> m_ItemCurrProgress = new Dictionary<int, float>();
        private Dictionary<int, float> m_ItemDestProgress = new Dictionary<int, float>();
        private bool m_FadeIn = false;
        private bool m_IsEnd = true;
        private bool m_IsPause = false;
        private bool m_FadeOut = false;
        private bool m_FadeOuted = false;
        private bool m_IsInit = false;

        private float startTime { get; set; }
        private float m_CurrDetailProgress;
        private float m_DestDetailProgress;
        private float m_TotalDetailProgress;
        private float m_CurrSymbolProgress;
        private Vector3 m_LinePathLastPos;

        public void FadeIn()
        {
            if (m_FadeOut)
                return;

            if (m_IsPause)
            {
                m_IsPause = false;
                return;
            }

            if (m_FadeIn)
                return;

            startTime = Time.time;
            m_FadeIn = true;
            m_IsEnd = false;
            m_IsInit = false;
            m_IsPause = false;
            m_FadeOuted = false;
            m_CurrDetailProgress = 0;
            m_DestDetailProgress = 1;
            m_CurrSymbolProgress = 0;
            m_ItemCurrProgress.Clear();
            m_ItemDestProgress.Clear();
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
            m_CurrDetailProgress = 0;
            m_DestDetailProgress = 1;
            m_CurrSymbolProgress = 0;
            m_ItemCurrProgress.Clear();
            m_ItemDestProgress.Clear();
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
            if (m_IsEnd)
                return;

            m_ActualDuration = (int) ((Time.time - startTime) * 1000) - (m_FadeOut ? fadeOutDelay : fadeInDelay);
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
            m_ItemCurrProgress.Clear();
        }

        public void InitProgress(float curr, float dest)
        {
            if (m_IsInit || m_IsEnd)
                return;
            if (curr > dest)
                return;

            m_IsInit = true;
            m_TotalDetailProgress = dest - curr;

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

        public void InitProgress(List<Vector3> paths, bool isY)
        {
            if (paths.Count < 1) return;
            var sp = paths[0];
            var ep = paths[paths.Count - 1];
            var currDetailProgress = isY ? sp.y : sp.x;
            var totalDetailProgress = isY ? ep.y : ep.x;
            if (context.type == AnimationType.AlongPath)
            {
                currDetailProgress = 0;
                totalDetailProgress = 0;
                var lp = sp;
                for (int i = 1; i < paths.Count; i++)
                {
                    var np = paths[i];
                    totalDetailProgress += Vector3.Distance(np, lp);
                    lp = np;
                }
                m_LinePathLastPos = sp;
                context.currentPathDistance = 0;
            }
            InitProgress(currDetailProgress, totalDetailProgress);
        }

        private void SetDataCurrProgress(int index, float state)
        {
            m_ItemCurrProgress[index] = state;
        }

        private float GetDataCurrProgress(int index, float initValue, float destValue, ref bool isBarEnd)
        {
            if (IsInDelay())
            {
                isBarEnd = false;
                return initValue;
            }
            var c1 = !m_ItemCurrProgress.ContainsKey(index);
            var c2 = !m_ItemDestProgress.ContainsKey(index);
            if (c1 || c2)
            {
                if (c1)
                    m_ItemCurrProgress.Add(index, initValue);

                if (c2)
                    m_ItemDestProgress.Add(index, destValue);

                isBarEnd = false;
            }
            else
            {
                isBarEnd = m_ItemCurrProgress[index] == m_ItemDestProgress[index];
            }
            return m_ItemCurrProgress[index];
        }

        public bool IsFinish()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return true;
#endif
            if (!m_Enable || m_IsEnd)
                return true;
            if (IsIndexAnimation())
            {
                if (m_FadeOut) return m_CurrDetailProgress <= m_DestDetailProgress;
                else return m_CurrDetailProgress > m_DestDetailProgress;
            }
            if (IsItemAnimation())
                return false;
            return true;
        }

        public bool IsInFadeOut()
        {
            return m_FadeOut;
        }

        public bool IsInDelay()
        {
            if (m_FadeOut)
                return (fadeOutDelay > 0 && Time.time - startTime < fadeOutDelay / 1000);
            else
                return (fadeInDelay > 0 && Time.time - startTime < fadeInDelay / 1000);
        }

        public bool IsItemAnimation()
        {
            return context.type == AnimationType.BottomToTop || context.type == AnimationType.InsideOut;
        }

        public bool IsIndexAnimation()
        {
            return context.type == AnimationType.LeftToRight ||
                context.type == AnimationType.Clockwise ||
                context.type == AnimationType.AlongPath;
        }

        public float GetIndexDelay(int dataIndex)
        {
            if (m_FadeOut && fadeOutDelayFunction != null)
                return fadeOutDelayFunction(dataIndex);
            else if (m_FadeIn && fadeInDelayFunction != null)
                return fadeInDelayFunction(dataIndex);
            else
                return 0;
        }

        public bool IsInIndexDelay(int dataIndex)
        {
            return Time.time - startTime < GetIndexDelay(dataIndex) / 1000f;
        }

        public bool IsAllOutDelay(int dataCount)
        {
            var nowTime = Time.time - startTime;
            for (int i = 0; i < dataCount; i++)
            {
                if (nowTime < GetIndexDelay(i) / 1000)
                    return false;
            }
            return true;
        }

        public bool CheckDetailBreak(float detail)
        {
            if (!IsIndexAnimation())
                return false;
            return !IsFinish() && detail > m_CurrDetailProgress;
        }

        public bool CheckDetailBreak(Vector3 pos, bool isYAxis)
        {
            if (!IsIndexAnimation())
                return false;

            if (IsFinish())
                return false;

            if (context.type == AnimationType.AlongPath)
            {
                context.currentPathDistance += Vector3.Distance(pos, m_LinePathLastPos);
                m_LinePathLastPos = pos;
                return CheckDetailBreak(context.currentPathDistance);
            }
            else
            {
                if (isYAxis)
                    return pos.y > m_CurrDetailProgress;
                else
                    return pos.x > m_CurrDetailProgress;
            }
        }

        public void CheckProgress()
        {
            if (IsItemAnimation() && context.isAllItemAnimationEnd)
            {
                End();
                return;
            }
            CheckProgress(m_TotalDetailProgress);
        }

        public void CheckProgress(double total)
        {
            if (IsFinish())
                return;

            if (!m_IsInit || m_IsPause || m_IsEnd)
                return;

            if (IsInDelay())
                return;

            m_ActualDuration = (int) ((Time.time - startTime) * 1000) - fadeInDelay;
            var duration = GetCurrAnimationDuration();
            var delta = (float) (total / duration * Time.deltaTime);
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

        internal float GetCurrAnimationDuration(int dataIndex = -1)
        {
            if (dataIndex >= 0)
            {
                if (m_FadeOut && fadeOutDurationFunction != null)
                    return fadeOutDurationFunction(dataIndex) / 1000f;
                if (m_FadeIn && fadeInDurationFunction != null)
                    return fadeInDurationFunction(dataIndex) / 1000f;
            }

            if (m_FadeOut)
                return m_FadeOutDuration > 0 ? m_FadeOutDuration / 1000 : 1f;
            else
                return m_FadeInDuration > 0 ? m_FadeInDuration / 1000 : 1f;
        }

        internal float CheckItemProgress(int dataIndex, float destProgress, ref bool isEnd, float startProgress = 0)
        {
            isEnd = false;
            var initHig = m_FadeOut ? destProgress : startProgress;
            var destHig = m_FadeOut ? startProgress : destProgress;
            var currHig = GetDataCurrProgress(dataIndex, initHig, destHig, ref isEnd);
            if (isEnd || IsFinish())
            {
                return m_FadeOuted ? startProgress : destProgress;
            }
            else if (IsInDelay() || IsInIndexDelay(dataIndex))
            {
                return m_FadeOut ? destProgress : startProgress;
            }
            else if (m_IsPause)
            {
                return currHig;
            }
            else
            {
                var duration = GetCurrAnimationDuration(dataIndex);
                var delta = (destProgress - startProgress) / duration * Time.deltaTime;
                currHig = currHig + (m_FadeOut ? -delta : delta);
                if (m_FadeOut)
                {
                    if ((initHig > 0 && currHig <= 0) || (initHig < 0 && currHig >= 0))
                    {
                        currHig = 0;
                        isEnd = true;
                    }
                }
                else
                {
                    if ((destProgress - startProgress > 0 && currHig > destProgress) ||
                        (destProgress - startProgress < 0 && currHig < destProgress))
                    {
                        currHig = destProgress;
                        isEnd = true;
                    }
                }
                SetDataCurrProgress(dataIndex, currHig);
                return currHig;
            }
        }

        public void CheckSymbol(float dest)
        {
            if (!enable || m_IsEnd || m_IsPause || !m_IsInit)
                return;

            if (IsInDelay())
                return;

            var duration = GetCurrAnimationDuration();
            var delta = dest / duration * Time.deltaTime;
            if (m_FadeOut)
            {
                m_CurrSymbolProgress -= delta;
                if (m_CurrSymbolProgress < 0)
                    m_CurrSymbolProgress = 0;
            }
            else
            {
                m_CurrSymbolProgress += delta;
                if (m_CurrSymbolProgress > dest)
                    m_CurrSymbolProgress = dest;
            }
        }

        public float GetSysmbolSize(float dest)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return dest;
#endif
            if (!enable)
                return dest;

            if (m_IsEnd)
                return m_FadeOut ? 0 : dest;

            return m_CurrSymbolProgress;
        }

        public float GetCurrDetail()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return m_DestDetailProgress;
#endif
            return m_CurrDetailProgress;
        }

        public float GetCurrRate()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return 1;
#endif
            if (!enable || m_IsEnd)
                return 1;
            return m_CurrDetailProgress;
        }

        public int GetCurrIndex()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return -1;
#endif
            if (!enable || m_IsEnd)
                return -1;
            return (int) m_CurrDetailProgress;
        }

        public float GetUpdateAnimationDuration()
        {
            if (m_Enable && m_DataChangeEnable && IsFinish())
                return m_DataChangeDuration;
            else
                return 0;
        }

        public bool HasFadeOut()
        {
            return enable && m_FadeOuted && m_IsEnd;
        }
    }
}