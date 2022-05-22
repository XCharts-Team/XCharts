using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example_TestTime : MonoBehaviour
    {
        public int maxCache = 100;
        LineChart chart;
        int timestamp;
        void Awake()
        {
            chart = gameObject.GetComponent<LineChart>();
            AddData();
            chart.SetMaxCache(maxCache);
        }

        float m_LastTime = 0;
        double m_Value = 100;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //AddData();
            }
            if (Time.time - m_LastTime > 0.1f)
            {
                timestamp += 3600;
                m_Value += 10;
                chart.AddData(0, timestamp, m_Value);
                m_LastTime = Time.time;

            }
        }

        void AddData()
        {
            chart.ClearData();
            timestamp = DateTimeUtil.GetTimestamp() - 10;
            for (int i = 0; i < 10; i++)
            {
                timestamp += i * 3600;
                double value = Random.Range(50, 200);
                chart.AddData(0, timestamp, value);
            }
        }
    }
}