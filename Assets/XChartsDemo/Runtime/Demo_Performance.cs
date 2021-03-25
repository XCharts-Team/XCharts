/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using XCharts;

namespace XChartsDemo
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    internal class Demo_Performance : MonoBehaviour
    {
        [SerializeField] private float fps = 0;
        [SerializeField] private int m_MaxCacheDataNumber = 3000;

        private const float INTERVAL = 0.2f;
        private int frameCount = 0;
        private float lastTime = 0f;
        private int initCount = 0;
        private System.DateTime timeNow;

        private LineChart fpsChart;
        private LineChart lineChart;
        private BarChart barChart;
        private Text fpsText;

        void Awake()
        {
            fpsChart = transform.Find("fps/FpsChart").gameObject.GetComponent<LineChart>();
            lineChart = transform.Find("charts/LineChart").gameObject.GetComponent<LineChart>();
            barChart = transform.Find("charts/BarChart").gameObject.GetComponent<BarChart>();
            fpsText = transform.Find("settings/txtFps").gameObject.GetComponent<Text>();
            fpsChart.ClearData();
            lineChart.ClearData();
            barChart.ClearData();
            InitCharts();
        }

        void Start()
        {
            lastTime = Time.realtimeSinceStartup;
        }

        void Update()
        {
            frameCount++;
            if (initCount < m_MaxCacheDataNumber)
            {
                for (int i = 0; i < 10; i++)
                {
                    initCount++;
                    if (initCount > m_MaxCacheDataNumber) break;
                    AddOneData();
                }
            }
            if (Time.realtimeSinceStartup - lastTime >= INTERVAL)
            {
                fps = frameCount / (Time.realtimeSinceStartup - lastTime);
                frameCount = 0;
                lastTime = Time.realtimeSinceStartup;
                fpsChart.AddData(0, fps);
                fpsChart.AddXAxisData(Time.frameCount.ToString());
                fpsText.text = string.Format("FPS:{0:f1}", fps);
                if (initCount >= m_MaxCacheDataNumber)
                {
                    AddOneData();
                    initCount++;
                }
            }
        }

        void AddOneData()
        {
            lineChart.title.text = initCount + "数据";
            barChart.title.text = initCount + "数据";
            timeNow = timeNow.AddSeconds(1);
            string category = timeNow.ToString("hh:mm:ss");
            float xvalue = Mathf.PI / 180 * initCount;
            float yvalue = 15 + Mathf.Sin(xvalue) * 2;

            lineChart.AddData(0, yvalue);
            lineChart.AddXAxisData(category);
            barChart.AddData(0, yvalue);
            barChart.AddXAxisData(category);
        }

        void InitCharts()
        {
            fpsChart.SetMaxCache(1000);
            lineChart.SetMaxCache(m_MaxCacheDataNumber);
            barChart.SetMaxCache(m_MaxCacheDataNumber);
            SetInputField("settings/fadeIn/InputField", m_MaxCacheDataNumber, OnFadeInDurationChanged);
        }

        void SetInputField(string path, int value, UnityAction<string> act)
        {
            var input = transform.Find(path).gameObject.GetComponent<InputField>();
            input.onEndEdit.AddListener(act);
            input.text = value.ToString();
        }

        Button TryInitButton(string name, UnityAction act)
        {
            var trans = transform.Find(name);
            if (trans)
            {
                var btn = trans.gameObject.GetComponent<Button>();
                btn.onClick.AddListener(act);
                return btn;
            }
            else
            {
                return null;
            }
        }


        void OnFadeInDurationChanged(string content)
        {
            m_MaxCacheDataNumber = int.Parse(content);
            lineChart.SetMaxCache(m_MaxCacheDataNumber);
            barChart.SetMaxCache(m_MaxCacheDataNumber);
        }
    }
}