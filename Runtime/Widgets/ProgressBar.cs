using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class ProgressBar : BaseChart
    {
        [SerializeField][Range(0f, 1f)] private float m_Value = 0.5f;
        [SerializeField] private Color m_BackgroundColor = new Color32(255, 233, 233, 255);
        [SerializeField] private Color m_StartColor = Color.blue;
        [SerializeField] private Color m_EndColor = Color.red;
        [SerializeField] private float[] m_CornerRadius = new float[] { 0, 0, 0, 0 };

        public float value { get { return m_Value; } set { m_Value = value; SetVerticesDirty(); } }
        public Color32 backgroundColor { get { return m_BackgroundColor; } set { m_BackgroundColor = value; SetVerticesDirty(); } }
        public Color32 startColor { get { return m_StartColor; } set { m_StartColor = value; SetVerticesDirty(); } }
        public Color32 endColor { get { return m_EndColor; } set { m_EndColor = value; SetVerticesDirty(); } }
        public float[] cornerRadius { get { return m_CornerRadius; } set { m_CornerRadius = value; SetVerticesDirty(); } }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            var title = GetOrAddChartComponent<Title>();
            title.text = "ProgressBar";
            title.show = false;
            SetSize(580, 4);
            RemoveData();
        }
#endif

        protected override void OnDrawPainterBase(VertexHelper vh, Painter painter)
        {
            vh.Clear();
            var centerPos = new Vector3(chartPosition.x + m_ChartWidth / 2, chartPosition.y + m_ChartHeight / 2);
            UGL.DrawRoundRectangle(vh, centerPos, m_ChartWidth, m_ChartHeight, m_BackgroundColor, m_BackgroundColor,
                0, m_CornerRadius, true);

            var valueWidth = m_Value * m_ChartWidth;
            var valuePos = new Vector3(chartPosition.x + valueWidth / 2, centerPos.y);
            var endColor = Color.Lerp(m_StartColor, m_EndColor, m_Value);
            UGL.DrawRoundRectangle(vh, valuePos, valueWidth, m_ChartHeight, m_StartColor, endColor, 0,
                m_CornerRadius, true);
        }

    }
}