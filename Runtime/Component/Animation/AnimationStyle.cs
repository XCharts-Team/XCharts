using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public enum AnimationType
    {
        /// <summary>
        /// he default. An animation playback mode will be selected according to the actual situation.
        /// ||默认。内部会根据实际情况选择一种动画播放方式。
        /// </summary>
        Default,
        /// <summary>
        /// Play the animation from left to right.
        /// ||从左往右播放动画。
        /// </summary>
        LeftToRight,
        /// <summary>
        /// Play the animation from bottom to top.
        /// ||从下往上播放动画。
        /// </summary>
        BottomToTop,
        /// <summary>
        /// Play animations from the inside out.
        /// ||由内到外播放动画。
        /// </summary>
        InsideOut,
        /// <summary>
        /// Play the animation along the path.
        /// ||沿着路径播放动画。当折线图从左到右无序或有折返时，可以使用该模式。
        /// </summary>
        AlongPath,
        /// <summary>
        /// Play the animation clockwise.
        /// ||顺时针播放动画。
        /// </summary>
        Clockwise,
    }

    public enum AnimationEasing
    {
        Linear,
    }

    /// <summary>
    /// the animation of serie. support animation type: fadeIn, fadeOut, change, addition.
    /// ||动画组件，用于控制图表的动画播放。支持配置五种动画表现：FadeIn（渐入动画），FadeOut（渐出动画），Change（变更动画），Addition（新增动画），Interaction（交互动画）。
    /// 按作用的对象可以分为两类：SerieAnimation（系列动画）和DataAnimation（数据动画）。
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
        [SerializeField][Since("v3.8.0")] private AnimationFadeOut m_FadeOut = new AnimationFadeOut() { reverse = true };
        [SerializeField][Since("v3.8.0")] private AnimationChange m_Change = new AnimationChange() { duration = 500 };
        [SerializeField][Since("v3.8.0")] private AnimationAddition m_Addition = new AnimationAddition() { duration = 500 };
        [SerializeField][Since("v3.8.0")] private AnimationHiding m_Hiding = new AnimationHiding() { duration = 500 };
        [SerializeField][Since("v3.8.0")] private AnimationInteraction m_Interaction = new AnimationInteraction() { duration = 250 };

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
        /// ||是否开启动画效果。
        /// </summary>
        public bool enable { get { return m_Enable; } set { m_Enable = value; } }
        /// <summary>
        /// The type of animation.
        /// ||动画类型。
        /// </summary>
        public AnimationType type { get { return m_Type; } set { m_Type = value; } }
        /// <summary>
        /// Whether to set graphic number threshold to animation. Animation will be disabled when graphic number is larger than threshold.
        /// ||是否开启动画的阈值，当单个系列显示的图形数量大于这个阈值时会关闭动画。
        /// </summary>
        public int threshold { get { return m_Threshold; } set { m_Threshold = value; } }
        /// <summary>
        /// Animation updates independently of Time.timeScale.
        /// ||动画是否受TimeScaled的影响。默认为 false 受TimeScaled的影响。
        /// </summary>
        public bool unscaledTime { get { return m_UnscaledTime; } set { m_UnscaledTime = value; } }
        /// <summary>
        /// Fade in animation configuration.
        /// ||渐入动画配置。
        /// </summary>
        public AnimationFadeIn fadeIn { get { return m_FadeIn; } }
        /// <summary>
        /// Fade out animation configuration.
        /// ||渐出动画配置。
        /// </summary>
        public AnimationFadeOut fadeOut { get { return m_FadeOut; } }
        /// <summary>
        /// Update data animation configuration.
        /// ||数据变更动画配置。
        /// </summary>
        public AnimationChange change { get { return m_Change; } }
        /// <summary>
        /// Add data animation configuration.
        /// ||数据新增动画配置。
        /// </summary>
        public AnimationAddition addition { get { return m_Addition; } }
        /// <summary>
        /// Data hiding animation configuration.
        /// ||数据隐藏动画配置。
        /// </summary>
        public AnimationHiding hiding { get { return m_Hiding; } }
        /// <summary>
        /// Interaction animation configuration.
        /// ||交互动画配置。
        /// </summary>
        public AnimationInteraction interaction { get { return m_Interaction; } }

        private Vector3 m_LinePathLastPos;
        private List<AnimationInfo> m_Animations;
        private List<AnimationInfo> animations
        {
            get
            {
                if (m_Animations == null)
                {
                    m_Animations = new List<AnimationInfo>();
                    m_Animations.Add(m_FadeIn);
                    m_Animations.Add(m_FadeOut);
                    m_Animations.Add(m_Change);
                    m_Animations.Add(m_Addition);
                    m_Animations.Add(m_Hiding);
                }
                return m_Animations;
            }
        }

        /// <summary>
        /// The actived animation.
        /// ||当前激活的动画。
        /// </summary>
        public AnimationInfo activedAnimation
        {
            get
            {
                foreach (var anim in animations)
                {
                    if (anim.context.start) return anim;
                }
                return null;
            }
        }

        /// <summary>
        /// Start fadein animation.
        /// ||开始渐入动画。
        /// </summary>
        public void FadeIn()
        {
            if (m_FadeOut.context.start) return;
            m_FadeIn.Start();
        }

        /// <summary>
        /// Restart the actived animation.
        /// ||重启当前激活的动画。
        /// </summary>
        public void Restart()
        {
            var anim = activedAnimation;
            Reset();
            if (anim != null)
            {
                anim.Start();
            }
        }

        /// <summary>
        /// Start fadeout animation.
        /// ||开始渐出动画。
        /// </summary>
        public void FadeOut()
        {
            m_FadeOut.Start();
        }

        /// <summary>
        /// Start additon animation.
        /// ||开始数据新增动画。
        /// </summary>
        public void Addition()
        {
            if (!enable) return;
            if (!m_FadeIn.context.start && !m_FadeOut.context.start)
            {
                m_Addition.Start(false);
            }
        }

        /// <summary>
        /// Pause all animations.
        /// ||暂停所有动画。
        /// </summary>
        public void Pause()
        {
            foreach (var anim in animations)
            {
                anim.Pause();
            }
        }

        /// <summary>
        /// Resume all animations.
        /// ||恢复所有动画。
        /// </summary>
        public void Resume()
        {
            foreach (var anim in animations)
            {
                anim.Resume();
            }
        }

        /// <summary>
        /// Reset all animations.
        /// </summary>
        public void Reset()
        {
            foreach (var anim in animations)
            {
                anim.Reset();
            }
        }

        /// <summary>
        /// Initialize animation configuration.
        /// ||初始化动画配置。
        /// </summary>
        /// <param name="curr">当前进度</param>
        /// <param name="dest">目标进度</param>
        public void InitProgress(float curr, float dest)
        {
            var anim = activedAnimation;
            if (anim == null) return;
            var isAddedAnim = anim is AnimationAddition;
            if (IsSerieAnimation())
            {
                if (isAddedAnim)
                {
                    anim.Init(anim.context.currPointIndex, dest, (int)dest - 1);
                }
                else
                {
                    m_Addition.context.currPointIndex = (int)dest - 1;
                    anim.Init(curr, dest, (int)dest - 1);
                }
            }
            else
            {
                anim.Init(curr, dest, 0);
            }
        }

        /// <summary>
        /// Initialize animation configuration.
        /// ||初始化动画配置。
        /// </summary>
        /// <param name="paths">路径坐标点列表</param>
        /// <param name="isY">是Y轴还是X轴</param>
        public void InitProgress(List<Vector3> paths, bool isY)
        {
            if (paths.Count < 1) return;
            var anim = activedAnimation;
            if (anim == null)
            {
                m_Addition.context.currPointIndex = paths.Count - 1;
                return;
            }
            var isAddedAnim = anim is AnimationAddition;
            var startIndex = 0;
            if (isAddedAnim)
            {
                startIndex = anim.context.currPointIndex == paths.Count - 1 ?
                    paths.Count - 2 :
                    anim.context.currPointIndex;
                if (startIndex < 0 || startIndex > paths.Count - 2) startIndex = 0;
            }
            else
            {
                m_Addition.context.currPointIndex = paths.Count - 1;
            }
            var sp = paths[startIndex];
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
                    if (startIndex > 0 && i == startIndex)
                        currDetailProgress = totalDetailProgress;
                }
                m_LinePathLastPos = sp;
                context.currentPathDistance = 0;
            }
            if (sp == anim.context.currPoint && ep == anim.context.destPoint)
            {
                return;
            }
            anim.context.currPoint = sp;
            anim.context.destPoint = ep;
            anim.Init(currDetailProgress, totalDetailProgress, paths.Count - 1);
        }

        public bool IsEnd()
        {
            foreach (var animation in animations)
            {
                if (animation.context.start)
                    return animation.context.end;
            }
            return m_FadeIn.context.end;
        }


        public bool IsFinish()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return true;
#endif
            if (!m_Enable)
                return true;
            var animation = activedAnimation;
            if (animation != null && animation.context.end)
                return true;
            if (IsSerieAnimation())
            {
                if (m_FadeOut.context.start) return m_FadeOut.context.currProgress <= m_FadeOut.context.destProgress;
                else if (m_Addition.context.start) return m_Addition.context.currProgress >= m_Addition.context.destProgress;
                else return m_FadeIn.context.currProgress >= m_FadeIn.context.destProgress;
            }
            else if (IsDataAnimation())
            {
                if (animation == null) return true;
                else return animation.context.end;
            }
            return true;
        }

        public bool IsInDelay()
        {
            var anim = activedAnimation;
            if (anim != null)
                return anim.IsInDelay();
            return false;
        }

        /// <summary>
        /// whther animaiton is data animation. BottomToTop and InsideOut are data animation.
        /// ||是否为数据动画。BottomToTop和InsideOut类型的为数据动画。
        /// </summary>
        public bool IsDataAnimation()
        {
            return context.type == AnimationType.BottomToTop || context.type == AnimationType.InsideOut;
        }

        /// <summary>
        /// whther animaiton is serie animation. LeftToRight, AlongPath and Clockwise are serie animation.
        /// ||是否为系列动画。LeftToRight、AlongPath和Clockwise类型的为系列动画。
        /// </summary>
        public bool IsSerieAnimation()
        {
            return context.type == AnimationType.LeftToRight ||
                context.type == AnimationType.AlongPath || context.type == AnimationType.Clockwise;
        }

        public bool CheckDetailBreak(float detail)
        {
            if (!IsSerieAnimation())
                return false;
            foreach (var animation in animations)
            {
                if (animation.context.start)
                    return !IsFinish() && detail > animation.context.currProgress;
            }
            return false;
        }

        public bool CheckDetailBreak(Vector3 pos, bool isYAxis)
        {
            if (!IsSerieAnimation())
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
                    return pos.y > GetCurrDetail();
                else
                    return pos.x > GetCurrDetail();
            }
        }

        public void CheckProgress()
        {
            if (IsDataAnimation() && context.isAllItemAnimationEnd)
            {
                foreach (var animation in animations)
                {
                    animation.End();
                }
                return;
            }
            foreach (var animation in animations)
            {
                animation.CheckProgress(animation.context.totalProgress, m_UnscaledTime);
            }
        }

        public void CheckProgress(double total)
        {
            if (IsFinish())
                return;
            foreach (var animation in animations)
            {
                animation.CheckProgress(total, m_UnscaledTime);
            }
        }

        internal float CheckItemProgress(int dataIndex, float destProgress, ref bool isEnd, float startProgress = 0)
        {
            isEnd = false;
            var anim = activedAnimation;
            if (anim == null)
            {
                isEnd = true;
                return destProgress;
            }
            return anim.CheckItemProgress(dataIndex, destProgress, ref isEnd, startProgress, m_UnscaledTime);
        }

        public void CheckSymbol(float dest)
        {
            m_FadeIn.CheckSymbol(dest, m_UnscaledTime);
            m_FadeOut.CheckSymbol(dest, m_UnscaledTime);
        }

        public float GetSysmbolSize(float dest)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return dest;
#endif
            if (!enable)
                return dest;

            if (IsEnd())
                return m_FadeOut.context.start ? 0 : dest;

            return m_FadeOut.context.start ? m_FadeOut.context.sizeProgress : m_FadeIn.context.sizeProgress;
        }

        public float GetCurrDetail()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                foreach (var animation in animations)
                {
                    if (animation.context.start)
                        return animation.context.destProgress;
                }
            }
#endif
            foreach (var animation in animations)
            {
                if (animation.context.start)
                    return animation.context.currProgress;
            }
            return m_FadeIn.context.currProgress;
        }

        public float GetCurrRate()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return 1;
#endif
            if (!enable || IsEnd())
                return 1;
            return m_FadeOut.context.start ? m_FadeOut.context.currProgress : m_FadeIn.context.currProgress;
        }

        public int GetCurrIndex()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return -1;
#endif
            if (!enable)
                return -1;
            var anim = activedAnimation;
            if (anim == null)
                return -1;
            return (int)anim.context.currProgress;
        }

        public float GetChangeDuration()
        {
            if (m_Enable && m_Change.enable)
                return m_Change.duration;
            else
                return 0;
        }

        public float GetAdditionDuration()
        {
            if (m_Enable && m_Addition.enable)
                return m_Addition.duration;
            else
                return 0;
        }

        public float GetInteractionDuration()
        {
            if (m_Enable && m_Interaction.enable)
                return m_Interaction.duration;
            else
                return 0;
        }

        public float GetInteractionRadius(float radius)
        {
            if (m_Enable && m_Interaction.enable)
                return m_Interaction.GetRadius(radius);
            else
                return radius;
        }

        public bool HasFadeOut()
        {
            return enable && m_FadeOut.context.end;
        }

        public bool IsFadeIn()
        {
            return enable && m_FadeIn.context.start;
        }

        public bool IsFadeOut()
        {
            return enable && m_FadeOut.context.start;
        }

        public bool CanCheckInteract()
        {
            return enable && interaction.enable
                && !IsFadeIn() && !IsFadeOut();
        }
    }
}