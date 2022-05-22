using System;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class Painter : MaskableGraphic
    {
        public enum Type
        {
            Base,
            Serie,
            Top
        }
        protected int m_Index = -1;
        protected Type m_Type = Type.Base;
        protected bool m_Refresh;
        protected Action<VertexHelper, Painter> m_OnPopulateMesh;

        public Action<VertexHelper, Painter> onPopulateMesh { set { m_OnPopulateMesh = value; } }
        public int index { get { return m_Index; } set { m_Index = value; } }
        public Type type { get { return m_Type; } set { m_Type = value; } }
        public void Refresh()
        {
            if (gameObject == null) return;
            if (!gameObject.activeSelf) return;
            m_Refresh = true;
        }

        public void Init()
        {
            raycastTarget = false;
        }

        public void SetActive(bool flag, bool isDebugMode = false)
        {
            if (gameObject.activeInHierarchy != flag)
            {
                gameObject.SetActive(flag);
            }
            var hideFlags = flag && isDebugMode ? HideFlags.None : HideFlags.HideInHierarchy;
            if (gameObject.hideFlags != hideFlags)
            {
                gameObject.hideFlags = hideFlags;
            }
        }

        protected override void Awake()
        {
            Init();
        }

        internal void CheckRefresh()
        {
            if (m_Refresh && gameObject.activeSelf)
            {
                m_Refresh = false;
                SetVerticesDirty();
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (m_OnPopulateMesh != null)
            {
                m_OnPopulateMesh(vh, this);
            }
        }
    }
}