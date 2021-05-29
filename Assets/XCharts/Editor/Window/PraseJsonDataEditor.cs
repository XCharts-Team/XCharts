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
    public class PraseJsonDataEditor : EditorWindow
    {
        public static BaseChart chart;
        public static int serieIndex;
        private static PraseJsonDataEditor window;
        private string inputJsonText = "";

        public static void ShowWindow()
        {
            window = GetWindow<PraseJsonDataEditor>();
            window.titleContent = new GUIContent("PraseJsonData");
            window.minSize = new Vector2(450, 550);
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
            EditorGUILayout.LabelField("Input json data, or echarts serie data:");
            inputJsonText = EditorGUILayout.TextArea(inputJsonText, GUILayout.Height(400));
            if (GUILayout.Button("Add"))
            {
                var serie = chart.series.GetSerie(serieIndex);
                if (serie != null)
                {
                    serie.ParseJsonData(inputJsonText);
                }
            }
        }
    }
}