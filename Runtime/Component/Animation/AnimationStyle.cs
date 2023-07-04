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
        /// |沿着路径播放动画。当折线图从左到右无序或有折返时，可以使用该模式。
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

    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationInfo
    {
        [SerializeField][Since("v3.8.0")] private bool m_Enable = true;
        [SerializeField][Since("v3.8.0")] private float m_Delay = 0;
        [SerializeField][Since("v3.8.0")] private float m_Duration = 1000;

        public bool enable { get { return m_Enable; } set { m_Enable = value; } }
        public float delay { get { return m_Delay; } set { m_Delay = value; } }
        public float duration { get { return m_Duration; } set { m_Duration = value; } }

        public Action OnAnimationStart { get; set; }
        public Action OnAnimationEnd { get; set; }

        public AnimationDelayFunction delayFunction { get; set; }
        public AnimationDurationFunction durationFunction { get; set; }

        internal bool start { get; set; }
        internal bool end { get; set; }
    }

    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationFadeIn : AnimationInfo
    {
    }

    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationFadeOut : AnimationInfo
    {
    }

    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationUpdated : AnimationInfo
    {
    }

    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationAdded : AnimationInfo
    {
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
        [SerializeField][Since("v3.4.0")] private bool m_UnscaledTime;
        [SerializeField][Since("v3.8.0")] private AnimationFadeIn m_FadeIn = new AnimationFadeIn();
        [SerializeField][Since("v3.8.0")] private AnimationFadeOut m_FadeOut = new AnimationFadeOut();
        [SerializeField][Since("v3.8.0")] private AnimationUpdated m_Updated = new AnimationUpdated() { duration = 500 };
        [SerializeField][Since("v3.8.0")] private AnimationAdded m_Added = new AnimationAdded() { duration = 500 };

        [Obsolete("Use animation.fadeIn.delayFunction instead.", true)]
        public AnimationDelayFunction fadeInDelayFunction;
        [Obsolete("Use animation.fadeIn.durationFunction instead.", true)]
        public AnimationDurationFunction fadeInDurationFunction;
        [Obsolete("Use animation.fadeOut.delayFunction instead.", true)]
        public AnimationDelayFunction fadeOutDelayFunction;
        [Obsolete("Use animation.fadeOut.durationFunction instead.", true)]
        public AnimationDurationFunction fadeOutDurationFunction;
        [Obsolete("Use animation.fadeIn.OnAnimationEnd() instead.", true)]
        public Action fadeInFinishCallback { get; set; }
        [Obsolete("Use animation.fadeOut.OnAnimationEnd() instead.", true)]
        public Action fadeOutFinishCallback { get; set; }
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
        /// Whether to set graphic number threshold to animation. Animation will be disabled when graphic number is larger than threshold.
        /// |是否开启动画的阈值，当单个系列显示的图形数量大于这个阈值时会关闭动画。
        /// </summary>
        public int threshold { get { return m_Threshold; } set { m_Threshold = value; } }
        /// <summary>
        /// Animation updates independently of Time.timeScale.
        /// |动画是否受TimeScaled的影响。默认为 false 受TimeScaled的影响。
        /// </summary>
        public bool unscaledTime { get { return m_UnscaledTime; } set { m_UnscaledTime = value; } }
        /// <summary>
        /// Fade in animation configuration.
        /// |渐入动画配置。
        /// </summary>
        public AnimationFadeIn fadeIn { get { return m_FadeIn; } }
        /// <summary>
        /// Fade out animation configuration.
        /// |渐出动画配置。
        /// </summary>
        public AnimationFadeOut fadeOut { get { return m_FadeOut; } }
        /// <summary>
        /// Update data animation configuration.
        /// |更新数据动画配置。
        /// </summary>
        public AnimationUpdated updated { get { return m_Updated; } }
        /// <summary>
        /// Add data animation configuration.
        /// |添加数据动画配置。
        /// </summary>
        public AnimationAdded added { get { return m_Added; } }

        private Dictionary<int, float> m_ItemCurrProgress = new Dictionary<int, float>();
        private Dictionary<int, float> m_ItemDestProgress = new Dictionary<int, float>();
        private bool m_IsEnd = true;
        private bool m_IsPause = false;
        private bool m_IsInit = false;

        private float startTime { get; set; }
        private float m_CurrDetailProgress;
        private float m_DestDetailProgress;
        private float m_TotalDetailProgress;
        private float m_CurrSymbolProgress;
        private Vector3 m_LinePathLastPos;

        public void FadeIn()
        {
            if (m_FadeOut.start)
                return;

            if (m_IsPause)
            {
                m_IsPause = false;
                return;
            }

            if (m_FadeIn.start)
                return;

            startTime = Time.time;
            m_FadeIn.start = true;
            m_IsEnd = false;
            m_IsInit = false;
            m_IsPause = false;
            m_FadeOut.end = false;
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

            m_FadeOut.start = true;
            startTime = Time.time;
            m_FadeIn.start = true;
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

            m_IsEnd = true;
            m_IsInit = false;

            if (m_FadeIn.start)
            {
                m_FadeIn.start = false;
                if (m_FadeIn.OnAnimationEnd != null)
                {
                    m_FadeIn.OnAnimationEnd();
                }
            }
            if (m_FadeOut.start)
            {
                m_FadeOut.start = false;
                m_FadeOut.end = true;
                if (m_FadeOut.OnAnimationEnd != null)
                {
                    m_FadeOut.OnAnimationEnd();
                }
            }
        }

        public void Reset()
        {
            m_FadeIn.start = false;
            m_IsEnd = true;
            m_IsInit = false;
            m_IsPause = false;
            m_FadeOut.start = false;
            m_FadeOut.end = false;
            m_ItemCurrProgress.Clear();
        }

        public void InitProgress(float curr, float dest)
        {
            if (m_IsInit || m_IsEnd)
                return;

            m_IsInit = true;
            m_TotalDetailProgress = dest - curr;

            if (m_FadeOut.start)
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
                if (m_FadeOut.start) return m_CurrDetailProgress <= m_DestDetailProgress;
                else return m_CurrDetailProgress > m_DestDetailProgress;
            }
            if (IsItemAnimation())
                return false;
            return true;
        }

        public bool IsInFadeOut()
        {
            return m_FadeOut.start;
        }

        public bool IsInDelay()
        {
            if (m_FadeOut.start)
                return (m_FadeOut.delay > 0 && Time.time - startTime < m_FadeOut.delay / 1000);
            else
                return (m_FadeIn.delay > 0 && Time.time - startTime < m_FadeIn.delay / 1000);
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
            if (m_FadeOut.start && m_FadeOut.delayFunction != null)
                return m_FadeOut.delayFunction(dataIndex);
            else if (m_FadeIn.start && m_FadeIn.delayFunction != null)
                return m_FadeIn.delayFunction(dataIndex);
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

            var duration = GetCurrAnimationDuration();
            var delta = (float)(total / duration * (m_UnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime));
            if (m_FadeOut.start)
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
                if (m_FadeOut.start && m_FadeOut.durationFunction != null)
                    return m_FadeOut.durationFunction(dataIndex) / 1000f;
                if (m_FadeIn.start && m_FadeIn.durationFunction != null)
                    return m_FadeIn.durationFunction(dataIndex) / 1000f;
            }

            if (m_FadeOut.start)
                return m_FadeOut.delay > 0 ? m_FadeOut.delay / 1000 : 1f;
            else
                return m_FadeIn.delay > 0 ? m_FadeIn.delay / 1000 : 1f;
        }

        internal float CheckItemProgress(int dataIndex, float destProgress, ref bool isEnd, float startProgress = 0)
        {
            isEnd = false;
            var initHig = m_FadeOut.start ? destProgress : startProgress;
            var destHig = m_FadeOut.start ? startProgress : destProgress;
            var currHig = GetDataCurrProgress(dataIndex, initHig, destHig, ref isEnd);
            if (isEnd || IsFinish())
            {
                return m_FadeOut.end ? startProgress : destProgress;
            }
            else if (IsInDelay() || IsInIndexDelay(dataIndex))
            {
                return m_FadeOut.start ? destProgress : startProgress;
            }
            else if (m_IsPause)
            {
                return currHig;
            }
            else
            {
                var duration = GetCurrAnimationDuration(dataIndex);
                var delta = (destProgress - startProgress) / duration * (m_UnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
                currHig = currHig + (m_FadeOut.start ? -delta : delta);
                if (m_FadeOut.start)
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
            var delta = dest / duration * (m_UnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
            if (m_FadeOut.start)
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
                return m_FadeOut.start ? 0 : dest;

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
            return (int)m_CurrDetailProgress;
        }

        public float GetDataChangeDuration()
        {
            if (m_Enable && m_Updated.enable)
                return m_Updated.duration;
            else
                return 0;
        }

        public float GetDataAddDuration()
        {
            if (m_Enable && m_Added.enable)
                return m_Added.duration;
            else
                return 0;
        }

        public bool HasFadeOut()
        {
            return enable && m_FadeOut.end && m_IsEnd;
        }
    }
}