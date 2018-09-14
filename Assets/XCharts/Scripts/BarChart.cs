using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace xcharts
{
    [System.Serializable]
    public class BarGroup
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public Color color;
    }

    [System.Serializable]
    public class BarData
    {
        [SerializeField]
        public string group;
        [SerializeField]
        public string key;
        [SerializeField]
        public float value;
    }

    public class BarChart : MaskableGraphic
    {
        [SerializeField]
        private float keyRectWidth = 50;
        [SerializeField]
        private float valueRectWidth = 50;
        [SerializeField]
        private float barWidth = 50;
        [SerializeField]
        private float barSpace = 20;
        [SerializeField] 
        private Color backgroundColor = Color.black;

        [SerializeField]
        private List<BarGroup> groupList = new List<BarGroup>();
        [SerializeField]
        private List<BarData> dataList = new List<BarData>();

        private float dataTotal = 0;
        private List<Text> keyTextList = new List<Text>();
        private List<Text> valueTextList = new List<Text>();

        private float chartWid { get { return rectTransform.sizeDelta.x; } }
        private float chartHig { get { return rectTransform.sizeDelta.y; } }
        

        void Awake()
        {
            dataTotal = getDataTotal();
        }

        void Update()
        {

        }

        private float getDataTotal()
        {
            float total = 0;
            foreach (var data in dataList)
            {
                total += data.value;
            }
            return total;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            // draw bg
            Vector3 p1 = new Vector3(-keyRectWidth, 0);
            Vector3 p2 = new Vector3(chartWid, 0);
            Vector3 p3 = new Vector3(chartWid, -chartHig);
            Vector3 p4 = new Vector3(-keyRectWidth, -chartHig);
            ChartUtils.DrawPolygon(vh, p1, p2, p3, p4, backgroundColor);

            //draw data bar

            dataTotal = getDataTotal();
            for(int i=0;i<dataList.Count;i++)
            {
                BarData data = dataList[i];
                float barLen = data.value / dataTotal * (chartWid - valueRectWidth);
                float posY = i * (barWidth + barSpace);
                p1 = new Vector3(0,-posY);
                p2 = new Vector3(barLen, -posY);
                p3 = new Vector3(barLen, -(posY + barWidth));
                p4 = new Vector3(0, -(posY + barWidth));
                ChartUtils.DrawPolygon(vh, p1, p2, p3, p4, Color.grey);
            }

            ChartUtils.DrawLine(vh, new Vector3(0, 0), new Vector3(0, -chartHig), 1.5f, Color.white);
        }
    }
}
