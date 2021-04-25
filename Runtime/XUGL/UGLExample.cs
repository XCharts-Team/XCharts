/******************************************/
/*                                        */
/*     Copyright (c) 2020 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XUGL
{
    [ExecuteInEditMode]
    public class UGLExample : MaskableGraphic
    {
        private float m_Width = 800;
        private float m_Height = 800;
        private Vector3 m_Center = Vector3.zero;
        private Vector3 m_LeftTopPos = Vector3.zero;
        private Color32 m_BackgroundColor = new Color32(224, 224, 224, 255);
        private Color32 m_DrawColor = new Color32(255, 132, 142, 255);
        private float[] m_BorderRadius = new float[] { 5, 5, 10, 10 };

        protected override void Awake()
        {
            base.Awake();
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(500, 500);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            m_Center = Vector3.zero;
            m_LeftTopPos = new Vector3(-m_Width / 2, m_Height / 2);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            Vector3 sp, cp, ep;
            vh.Clear();

            //背景边框
            UGL.DrawSquare(vh, m_Center, m_Width / 2, m_BackgroundColor);
            UGL.DrawBorder(vh, m_Center, m_Width, m_Height, 40, Color.green, Color.red, 0, m_BorderRadius,false,1);

            //点
            UGL.DrawCricle(vh, m_LeftTopPos + new Vector3(20, -20), 10, m_DrawColor);

            //直线
            sp = new Vector3(m_LeftTopPos.x + 50, m_LeftTopPos.y - 20);
            ep = new Vector3(m_LeftTopPos.x + 250, m_LeftTopPos.y - 20);
            UGL.DrawLine(vh, sp, ep, 3, m_DrawColor);

            //3点确定的折线
            sp = new Vector3(m_LeftTopPos.x + 20, m_LeftTopPos.y - 100);
            cp = new Vector3(m_LeftTopPos.x + 200, m_LeftTopPos.y - 40);
            ep = new Vector3(m_LeftTopPos.x + 250, m_LeftTopPos.y - 80);
            UGL.DrawLine(vh, sp, cp, ep, 5, m_DrawColor);

        }
    }
}