using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example90_Candlestick : MonoBehaviour
    {
        private CandlestickChart chart;
        private float updateTime;
        public int dataCount = 100;

        void Awake()
        {
            chart = gameObject.GetComponent<CandlestickChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<CandlestickChart>();
            }
            GenerateOHLC(dataCount);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddData();
            }
        }

        void AddData() { }

        void GenerateOHLC(int count)
        {
            chart.ClearData();

            var xValue = System.DateTime.Now;
            var baseValue = Random.Range(0f, 1f) * 12000;
            var boxVals = new float[4];
            var dayRange = 12;

            for (int i = 0; i < count; i++)
            {
                baseValue = baseValue + Random.Range(0f, 1f) * 30 - 10;
                for (int j = 0; j < 4; j++)
                {
                    boxVals[j] = (Random.Range(0f, 1f) - 0.5f) * dayRange + baseValue;
                }
                System.Array.Sort(boxVals);
                var openIdx = Mathf.RoundToInt(Random.Range(0f, 1f) * 3);
                var closeIdx = Mathf.RoundToInt(Random.Range(0f, 1f) * 2);
                if (openIdx == closeIdx)
                {
                    closeIdx++;
                }
                //var volumn = boxVals[3]*(1000+Random.Range(0f,1f) * 500);
                var open = boxVals[openIdx];
                var close = boxVals[closeIdx];
                var lowest = boxVals[0];
                var heighest = boxVals[3];

                chart.AddXAxisData(i.ToString());
                chart.AddData(0, open, close, lowest, heighest);
            }
        }
    }
}