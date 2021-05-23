/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    public class AddSerieEditor : EditorWindow
    {
        public static BaseChart chart;
        private static AddSerieEditor window;
        private static string serieName;

        private SerieType serieType;

        public static void ShowWindow()
        {
            serieName = "serie" + (chart.series.Count + 1);
            window = GetWindow<AddSerieEditor>();
            window.titleContent = new GUIContent("Add Serie");
            window.minSize = new Vector2(350, window.minSize.y);
            window.Focus();
            window.Show();
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            if (chart == null)
            {
                Close();
                return;
            }
            var iconRect = new Rect(5, 10, position.width - 10, EditorGUIUtility.singleLineHeight);
            serieType = (SerieType)EditorGUI.EnumPopup(iconRect, "Serie Type", serieType);
            iconRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            serieName = EditorGUI.TextField(iconRect, "Serie Name", serieName);
            iconRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            GUILayout.Space(iconRect.y + 5);
            if (GUILayout.Button("Add"))
            {
                SerieTemplate.AddDefaultSerie(chart, serieType, serieName);
                chart.RefreshAllComponent();
            }
        }
    }
}