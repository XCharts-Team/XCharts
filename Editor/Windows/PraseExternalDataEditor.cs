using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    public class PraseExternalDataEditor : UnityEditor.EditorWindow
    {
        private static BaseChart s_Chart;
        private static Serie s_Serie;
        private static Axis s_Axis;
        private static PraseExternalDataEditor window;
        private static string inputJsonText = "";

        public static void ShowWindow()
        {
            window = GetWindow<PraseExternalDataEditor>();
            window.titleContent = new GUIContent("PraseExternalData");
            window.minSize = new Vector2(450, 550);
            window.Focus();
            window.Show();
        }

        public static void UpdateData(BaseChart chart, Serie serie, Axis axis)
        {
            s_Chart = chart;
            s_Serie = serie;
            s_Axis = axis;
            inputJsonText = UnityEngine.GUIUtility.systemCopyBuffer;
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            if (s_Chart == null)
            {
                Close();
                return;
            }
            EditorGUILayout.LabelField("Input external data (echarts data):");
            inputJsonText = EditorGUILayout.TextArea(inputJsonText, GUILayout.Height(400));
            if (GUILayout.Button("Add"))
            {
                if (s_Serie != null)
                {
                    if (!ParseArrayData(s_Serie, inputJsonText))
                    {
                        if (ParseJsonData(s_Serie, inputJsonText))
                            inputJsonText = "";
                    }
                    else
                    {
                        inputJsonText = "";
                    }
                }
                else if (s_Axis != null)
                {
                    if (!ParseArrayData(s_Axis, inputJsonText))
                    {
                        if (ParseJsonData(s_Axis, inputJsonText))
                            inputJsonText = "";
                    }
                    else
                    {
                        inputJsonText = "";
                    }
                }
            }
        }

        private static bool ParseArrayData(Axis axis, string arrayData)
        {
            arrayData = arrayData.Trim();
            if (!arrayData.StartsWith("data: Array")) return false;
            axis.data.Clear();
            var list = arrayData.Split('\n');
            for (int i = 1; i < list.Length; i++)
            {
                var temp = list[i].Split(':');
                if (temp.Length == 2)
                {
                    var category = temp[1].Replace("\"", "").Trim();
                    axis.data.Add(category);
                }
            }
            axis.SetAllDirty();
            return true;
        }

        private static bool ParseArrayData(Serie serie, string arrayData)
        {
            arrayData = arrayData.Trim();
            if (!arrayData.StartsWith("data: Array")) return false;
            serie.ClearData();
            var list = arrayData.Split('\n');
            for (int i = 1; i < list.Length; i++)
            {
                var temp = list[i].Split(':');
                if (temp.Length == 2)
                {
                    var strvalue = temp[1].Replace("\"", "").Trim();
                    var value = 0d;
                    var flag = double.TryParse(strvalue, out value);
                    if (flag)
                    {
                        serie.AddYData(value);
                    }
                }
            }
            serie.SetAllDirty();
            return true;
        }

        private static bool ParseJsonData(Axis axis, string jsonData)
        {
            if (!CheckJsonData(ref jsonData)) return false;
            axis.data.Clear();
            string[] datas = jsonData.Split(',');
            for (int i = 0; i < datas.Length; i++)
            {
                var txt = datas[i].Trim().Replace("[", "").Replace("]", "");
                var value = 0d;
                if (!double.TryParse(txt, out value))
                    axis.data.Add(txt.Replace("\'", "").Replace("\"", ""));
            }
            axis.SetAllDirty();
            return true;
        }

        /// <summary>
        /// 从json中导入数据
        /// </summary>
        /// <param name="jsonData"></param>
        private static bool ParseJsonData(Serie serie, string jsonData)
        {
            if (!CheckJsonData(ref jsonData)) return false;
            serie.ClearData();
            if (jsonData.IndexOf("],") > -1 || jsonData.IndexOf("] ,") > -1)
            {
                string[] datas = jsonData.Split(new string[] { "],", "] ," }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < datas.Length; i++)
                {
                    var data = datas[i].Replace("[", "").Replace("]", "").Split(new char[] { '[', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var serieData = new SerieData();
                    double value = 0;
                    if (data.Length == 2 && !double.TryParse(data[0], out value))
                    {
                        double.TryParse(data[1], out value);
                        serieData.data = new List<double>() { i, value };
                        serieData.name = data[0].Replace("\"", "").Trim();
                    }
                    else
                    {
                        for (int j = 0; j < data.Length; j++)
                        {
                            var txt = data[j].Trim().Replace("]", "");
                            var flag = double.TryParse(txt, out value);
                            if (flag)
                            {
                                serieData.data.Add(value);
                            }
                            else serieData.name = txt.Replace("\"", "").Trim();
                        }
                    }
                    serie.AddSerieData(serieData);
                }
            }
            else if (jsonData.IndexOf("value") > -1 && jsonData.IndexOf("name") > -1)
            {
                string[] datas = jsonData.Split(new string[] { "},", "} ,", "}" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < datas.Length; i++)
                {
                    var arr = datas[i].Replace("{", "").Split(',');
                    var serieData = new SerieData();
                    foreach (var a in arr)
                    {
                        if (a.StartsWith("value:"))
                        {
                            double value = double.Parse(a.Substring(6, a.Length - 6));
                            serieData.data = new List<double>() { i, value };
                        }
                        else if (a.StartsWith("name:"))
                        {
                            string name = a.Substring(6, a.Length - 6 - 1);
                            serieData.name = name;
                        }
                        else if (a.StartsWith("selected:"))
                        {
                            string selected = a.Substring(9, a.Length - 9);
                            serieData.selected = bool.Parse(selected);
                        }
                    }
                    serie.AddSerieData(serieData);
                }
            }
            else
            {
                string[] datas = jsonData.Split(',');
                for (int i = 0; i < datas.Length; i++)
                {
                    double value;
                    var flag = double.TryParse(datas[i].Trim(), out value);
                    if (flag)
                    {
                        var serieData = new SerieData();
                        serieData.data = new List<double>() { i, value };
                        serie.AddSerieData(serieData);
                    }
                }
            }
            serie.SetAllDirty();
            return true;
        }

        private static bool CheckJsonData(ref string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) return false;
            jsonData = jsonData.Replace("\r\n", "");
            jsonData = jsonData.Replace(" ", "");
            jsonData = jsonData.Replace("\n", "");
            int startIndex = jsonData.IndexOf("[");
            int endIndex = jsonData.LastIndexOf("]");
            if (startIndex == -1 || endIndex == -1)
            {
                Debug.LogError("json data need include in [ ]");
                return false;
            }
            jsonData = jsonData.Substring(startIndex + 1, endIndex - startIndex - 1);
            return true;
        }
    }
}