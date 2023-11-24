using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// animation style of axis.
    /// ||坐标轴动画配置。
    /// </summary>
    [System.Serializable]
    [Since("v3.9.0")]
    public class AxisAnimation : ChildComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private float m_Duration;
        [SerializeField] private bool m_UnscaledTime;

        /// <summary>
        /// whether to enable animation.
        /// ||是否开启动画。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the duration of animation (ms). When it is set to 0, the animation duration will be automatically calculated according to the serie.
        /// ||动画时长(ms)。 默认设置为0时，会自动获取serie的动画时长。
        /// </summary>
        public float duration
        {
            get { return m_Duration; }
            set { if (PropertyUtil.SetStruct(ref m_Duration, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Animation updates independently of Time.timeScale.
        /// ||动画是否受TimeScaled的影响。默认为 false 受TimeScaled的影响。
        /// </summary>
        public bool unscaledTime
        {
            get { return m_UnscaledTime; }
            set { if (PropertyUtil.SetStruct(ref m_UnscaledTime, value)) SetComponentDirty(); }
        }

        public AxisAnimation Clone()
        {
            var animation = new AxisAnimation
            {
                show = show,
                duration = duration,
                unscaledTime = unscaledTime
            };
            return animation;
        }

        public void Copy(AxisAnimation animation)
        {
            show = animation.show;
            duration = animation.duration;
            unscaledTime = animation.unscaledTime;
        }
    }
}