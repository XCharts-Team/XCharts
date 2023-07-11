
using System;
using UnityEngine;

namespace XCharts.Runtime
{
    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationInfo
    {
        [SerializeField][Since("v3.8.0")] private bool m_Enable = true;
        [SerializeField][Since("v3.8.0")] private bool m_Reverse = false;
        [SerializeField][Since("v3.8.0")] private float m_Delay = 0;
        [SerializeField][Since("v3.8.0")] private float m_Duration = 1000;
        public AnimationInfoContext context = new AnimationInfoContext();

        public bool enable { get { return m_Enable; } set { m_Enable = value; } }
        public bool reverse { get { return m_Reverse; } set { m_Reverse = value; } }
        public float delay { get { return m_Delay; } set { m_Delay = value; } }
        public float duration { get { return m_Duration; } set { m_Duration = value; } }

        public Action OnAnimationStart { get; set; }
        public Action OnAnimationEnd { get; set; }

        public AnimationDelayFunction delayFunction { get; set; }
        public AnimationDurationFunction durationFunction { get; set; }

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

        public void Pause()
        {
            if (!enable) return;
            if (!context.start || context.end) return;
            context.pause = true;
        }

        public void Resume()
        {
            if (!enable) return;
            if (!context.pause) return;
            context.pause = false;
        }

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

        public bool IsFinish()
        {
            if (!context.start) return true;
            if (context.end) return true;
            if (context.pause) return false;
            return context.currProgress == context.destProgress;
        }

        public bool IsInDelay()
        {
            if (!context.start)
                return false;
            else
                return (m_Delay > 0 && Time.time - context.startTime < m_Delay / 1000);
        }

        public bool IsInIndexDelay(int dataIndex)
        {
            if (context.start)
                return Time.time - context.startTime < GetIndexDelay(dataIndex) / 1000f;
            else
                return false;
        }

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
    public class AnimationChange : AnimationInfo
    {
    }

    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationAddition : AnimationInfo
    {
    }
}