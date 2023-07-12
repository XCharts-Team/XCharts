
using System;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// the animation info.
    /// |动画配置参数。
    /// </summary>
    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationInfo
    {
        [SerializeField][Since("v3.8.0")] private bool m_Enable = true;
        [SerializeField][Since("v3.8.0")] private bool m_Reverse = false;
        [SerializeField][Since("v3.8.0")] private float m_Delay = 0;
        [SerializeField][Since("v3.8.0")] private float m_Duration = 1000;
        public AnimationInfoContext context = new AnimationInfoContext();

        /// <summary>
        /// whether enable animation.
        /// |是否开启动画效果。
        /// </summary>
        public bool enable { get { return m_Enable; } set { m_Enable = value; } }
        /// <summary>
        /// whether enable reverse animation.
        /// |是否开启反向动画效果。
        /// </summary>
        public bool reverse { get { return m_Reverse; } set { m_Reverse = value; } }
        /// <summary>
        /// the delay time before animation start.
        /// |动画开始前的延迟时间。
        /// </summary>
        public float delay { get { return m_Delay; } set { m_Delay = value; } }
        /// <summary>
        /// the duration of animation.
        /// |动画的时长。
        /// </summary>
        public float duration { get { return m_Duration; } set { m_Duration = value; } }

        /// <summary>
        /// the callback function of animation start.
        /// |动画开始的回调。
        /// </summary>
        public Action OnAnimationStart { get; set; }
        /// <summary>
        /// the callback function of animation end.
        /// |动画结束的回调。
        /// </summary>
        public Action OnAnimationEnd { get; set; }

        /// <summary>
        /// the delegate function of animation delay.
        /// |动画延迟的委托函数。
        /// </summary>
        public AnimationDelayFunction delayFunction { get; set; }
        /// <summary>
        /// the delegate function of animation duration.
        /// |动画时长的委托函数。
        /// </summary>
        public AnimationDurationFunction durationFunction { get; set; }

        /// <summary>
        /// Reset animation.
        /// |重置动画。
        /// </summary>
        public void Reset()
        {
            if (!enable) return;
            context.init = false;
            context.start = false;
            context.pause = false;
            context.end = false;
            context.startTime = 0;
            context.itemCurrProgress.Clear();
        }

        /// <summary>
        /// Start animation.
        /// |开始动画。
        /// </summary>
        /// <param name="reset">是否重置上一次的参数</param>
        public void Start(bool reset = true)
        {
            if (!enable) return;
            if (context.start) return;
            if (context.pause)
            {
                context.pause = false;
                return;
            }
            context.init = false;
            context.start = true;
            context.end = false;
            context.pause = false;
            context.startTime = Time.time;
            if (reset)
            {
                context.currProgress = 0;
                context.destProgress = 1;
                context.totalProgress = 0;
                context.sizeProgress = 0;
                context.itemCurrProgress.Clear();
                context.itemDestProgress.Clear();
            }
            if (OnAnimationStart != null)
            {
                OnAnimationStart();
            }
        }

        /// <summary>
        /// Pause animation.
        /// |暂停动画。
        /// </summary>
        public void Pause()
        {
            if (!enable) return;
            if (!context.start || context.end) return;
            context.pause = true;
        }

        /// <summary>
        /// Resume animation.
        /// |恢复动画。
        /// </summary>
        public void Resume()
        {
            if (!enable) return;
            if (!context.pause) return;
            context.pause = false;
        }

        /// <summary>
        /// End animation.
        /// |结束动画。
        /// </summary>
        public void End()
        {
            if (!enable) return;
            if (!context.start || context.end) return;
            context.start = false;
            context.end = true;
            context.currPointIndex = context.destPointIndex;
            context.startTime = Time.time;
            if (OnAnimationEnd != null)
            {
                OnAnimationEnd();
            }
        }

        /// <summary>
        /// Initialize animation.
        /// |初始化动画。
        /// </summary>
        /// <param name="curr">当前进度</param>
        /// <param name="dest">目标进度</param>
        /// <param name="totalPointIndex">目标索引</param>
        /// <returns></returns>
        public bool Init(float curr, float dest, int totalPointIndex)
        {
            if (!enable || !context.start) return false;
            if (context.init || context.end) return false;
            context.init = true;
            context.totalProgress = dest - curr;
            context.destPointIndex = totalPointIndex;
            if (reverse)
            {
                context.currProgress = dest;
                context.destProgress = curr;
            }
            else
            {
                context.currProgress = curr;
                context.destProgress = dest;
            }
            return true;
        }

        /// <summary>
        /// Whether animation is finish.
        /// |动画是否结束。
        /// </summary>
        public bool IsFinish()
        {
            if (!context.start) return true;
            if (context.end) return true;
            if (context.pause) return false;
            return context.currProgress == context.destProgress;
        }

        /// <summary>
        /// Whether animation is in delay.
        /// |动画是否在延迟中。
        /// </summary>
        public bool IsInDelay()
        {
            if (!context.start)
                return false;
            else
                return (m_Delay > 0 && Time.time - context.startTime < m_Delay / 1000);
        }

        /// <summary>
        /// Whether animation is in index delay.
        /// |动画是否在索引延迟中。
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        public bool IsInIndexDelay(int dataIndex)
        {
            if (context.start)
                return Time.time - context.startTime < GetIndexDelay(dataIndex) / 1000f;
            else
                return false;
        }

        /// <summary>
        /// Get animation delay.
        /// |获取动画延迟。
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        public float GetIndexDelay(int dataIndex)
        {
            if (!context.start) return 0;
            if (delayFunction != null)
                return delayFunction(dataIndex);
            return delay;
        }

        internal float GetCurrAnimationDuration(int dataIndex = -1)
        {
            if (dataIndex >= 0)
            {
                if (context.start && durationFunction != null)
                    return durationFunction(dataIndex) / 1000f;
            }
            return m_Duration > 0 ? m_Duration / 1000 : 1f;
        }

        internal void SetDataCurrProgress(int index, float state)
        {
            context.itemCurrProgress[index] = state;
        }


        internal float GetDataCurrProgress(int index, float initValue, float destValue, ref bool isBarEnd)
        {
            if (IsInDelay())
            {
                isBarEnd = false;
                return initValue;
            }
            var c1 = !context.itemCurrProgress.ContainsKey(index);
            var c2 = !context.itemDestProgress.ContainsKey(index);
            if (c1 || c2)
            {
                if (c1)
                    context.itemCurrProgress.Add(index, initValue);

                if (c2)
                    context.itemDestProgress.Add(index, destValue);

                isBarEnd = false;
            }
            else
            {
                isBarEnd = context.itemCurrProgress[index] == context.itemDestProgress[index];
            }
            return context.itemCurrProgress[index];
        }

        internal void CheckProgress(double total, bool m_UnscaledTime)
        {
            if (!context.start || !context.init || context.pause) return;
            if (IsInDelay()) return;
            var duration = GetCurrAnimationDuration();
            var delta = (float)(total / duration * (m_UnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime));
            if (reverse)
            {
                context.currProgress -= delta;
                if (context.currProgress <= context.destProgress)
                {
                    context.currProgress = context.destProgress;
                    End();
                }
            }
            else
            {
                context.currProgress += delta;
                if (context.currProgress >= context.destProgress)
                {
                    context.currProgress = context.destProgress;
                    End();
                }
            }
        }

        internal float CheckItemProgress(int dataIndex, float destProgress, ref bool isEnd, float startProgress, bool m_UnscaledTime)
        {
            var currHig = GetDataCurrProgress(dataIndex, startProgress, destProgress, ref isEnd);
            if (IsFinish())
            {
                return reverse ? startProgress : destProgress;
            }
            else if (IsInDelay() || IsInIndexDelay(dataIndex))
            {
                return reverse ? destProgress : startProgress;
            }
            else if (context.pause)
            {
                return currHig;
            }
            else
            {
                var duration = GetCurrAnimationDuration(dataIndex);
                var delta = (destProgress - startProgress) / duration * (m_UnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
                currHig = currHig + (reverse ? -delta : delta);
                if (reverse)
                {
                    if ((destProgress > 0 && currHig <= 0) || (destProgress < 0 && currHig >= 0))
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

        internal void CheckSymbol(float dest, bool m_UnscaledTime)
        {
            if (!context.start || !context.init || context.pause) return;

            if (IsInDelay())
                return;

            var duration = GetCurrAnimationDuration();
            var delta = dest / duration * (m_UnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
            if (reverse)
            {
                context.sizeProgress -= delta;
                if (context.sizeProgress < 0)
                    context.sizeProgress = 0;
            }
            else
            {
                context.sizeProgress += delta;
                if (context.sizeProgress > dest)
                    context.sizeProgress = dest;
            }
        }
    }

    /// <summary>
    /// Fade in animation.
    /// |淡入动画。
    /// </summary>
    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationFadeIn : AnimationInfo
    {
    }

    /// <summary>
    /// Fade out animation.
    /// |淡出动画。
    /// </summary>
    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationFadeOut : AnimationInfo
    {
    }

    /// <summary>
    /// Data change animation.
    /// |数据变更动画。
    /// </summary>
    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationChange : AnimationInfo
    {
    }

    /// <summary>
    /// Data addition animation.
    /// |数据新增动画。
    /// </summary>
    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationAddition : AnimationInfo
    {
    }
}