using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [ExecuteInEditMode]
    public class SVGImage : MaskableGraphic
    {
        [SerializeField] private bool m_MirrorY;
        [SerializeField] private string m_SVGPath;

        private SVGPath m_Path;

        public string svgPath { set { m_SVGPath = value; } get { return m_SVGPath; } }
        public bool mirrorY { set { m_MirrorY = value; } get { return m_MirrorY; } }

        protected override void Awake()
        {
            base.Awake();
            m_Path = SVGPath.Parse(m_SVGPath);
            m_Path.mirrorY = m_MirrorY;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (m_Path != null)
                m_Path.Draw(vh);
        }

    }
}