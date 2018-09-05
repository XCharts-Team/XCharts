
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace xchart
{
    [System.Serializable]
    public class LineData
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public string key;
        [SerializeField]
        public Color lineColor;
        [SerializeField]
        public Color pointColor;
        [SerializeField]
        public Button button;

        private List<float> _dataList = new List<float>();
        public List<float> dataList
        {
            get { return _dataList; }
        }

        private bool _visible = true;
        public bool visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                if (button)
                {
                    button.GetComponent<Image>().color = visible ? lineColor : Color.grey;
                }
            }
        }

        private int _min = 0;
        public int min
        {
            get { return _min; }
        }

        private int _max = 10;
        public int max
        {
            get { return _max; }
        }
        private int _step = 10;
        public int step
        {
            get { return _step; }
            set { _step = value; }
        }

        public void AddData(float data, int maxCount)
        {
            dataList.Add(data);
            if (dataList.Count > maxCount)
            {
                dataList.RemoveAt(0);
                UpdateMinMax();
            }
            else
            {
                if (data < _min)
                {
                    _min = (int)data;
                }
                if (data > _max)
                {
                    _max = (int)data;
                }
                CheckMax();
            }
        }

        public void ClearData()
        {
            _dataList.Clear();
        }

        public void UpdateMinMax()
        {
            _min = 0;
            _max = 4;
            foreach (var data in dataList)
            {
                if (data < _min)
                {
                    _min = (int)data;
                }
                if (data > _max)
                {
                    _max = (int)data;
                }
            }
            CheckMax();
        }

        private void CheckMax()
        {
            if (_max <= 10)
            {
                if (_max < 4) _max = 4;
            }
            else
            {
                int diff = _max % _step;
                if (diff > 1)
                {
                    _max = (_max - diff) + _step;
                }
            }
        }
    }


    public class LineChart : MaskableGraphic
    {
        [SerializeField]
        private int pointWidth = 15;
        [SerializeField]
        private float lineSize = 1f;
        [SerializeField]
        private float pointSize = 1.5f;
        [SerializeField]
        private int graduationStep = 10;
        [SerializeField]
        private List<Text> graduationList = new List<Text>();
        [SerializeField]
        private List<LineData> lineList = new List<LineData>();

        private Dictionary<string, LineData> lineMap = new Dictionary<string, LineData>();
        private RectTransform rectTrans;
        private float lastMax = 0;

        protected override void Awake()
        {
            base.Awake();
            rectTrans = GetComponent<RectTransform>();
            for (int i = 0; i < lineList.Count; i++)
            {
                LineData line = lineList[i];
                line.dataList.Clear();
                if (line.button)
                {
                    line.button.GetComponent<Image>().color = line.visible ? line.lineColor : Color.grey;
                    line.button.GetComponentInChildren<Text>().text = line.name;
                    line.button.onClick.AddListener(delegate ()
                    {
                        OnClickButton(line.key);
                    });
                }
                AddLineToLineMap(line);
            }
        }

        private float time;
        private void Update()
        {
            time += Time.deltaTime;
            if (time >= 1)
            {
                time = 0;
                AddPoint("fps", Random.Range(24.0f, 60.0f));
                AddPoint("rtt", Random.Range(15, 30));
                AddPoint("ping", Random.Range(0, 100));
            }
        }

        void OnClickButton(string key)
        {
            LineData line = lineMap[key];
            line.visible = !line.visible;
            if (line.visible)
            {
                line.step = graduationStep;
                line.UpdateMinMax();
            }
            UpdateGradution();
            UpdateMesh();
        }

        private void AddLineToLineMap(LineData line)
        {
            if (lineMap.ContainsKey(line.key))
            {
                Debug.LogError("LineChart:line key is duplicated:" + line.key);
            }
            else
            {
                lineMap[line.key] = line;
            }
        }

        private float GetAllLineMax()
        {
            float max = 4;
            foreach (var line in lineList)
            {
                if (line.visible && line.max > max)
                {
                    max = line.max;
                }
            }
            return max;
        }

        public int GetMaxPointCount()
        {
            Vector2 size = rectTransform.sizeDelta;
            int max = (int)(size.x / pointWidth);
            return max;
        }

        public void AddLine(string key, string name, Color lineColor, Color pointColor)
        {
            LineData line = new LineData();
            line.key = key;
            line.name = name;
            line.lineColor = lineColor;
            line.pointColor = pointColor;
            lineList.Add(line);
            AddLineToLineMap(line);
        }

        public void AddPoint(string key, float point)
        {
            if (!lineMap.ContainsKey(key))
            {
                Debug.LogError("LineChart:not contain line key:" + key);
                return;
            }
            LineData line = lineMap[key];
            line.AddData(point, GetMaxPointCount());
            UpdateMesh();
            UpdateGradution();
        }

        public void ResetDataStart()
        {
            foreach (var line in lineList)
            {
                line.ClearData();
            }
        }

        public void ResetData(string key, float data)
        {
            if (!lineMap.ContainsKey(key))
            {
                Debug.LogError("LineChart:not contain line key:" + key);
                return;
            }
            LineData line = lineMap[key];
            line.AddData(data, GetMaxPointCount());
        }

        public void ResetDataEnd()
        {
            foreach (var line in lineList)
            {
                line.UpdateMinMax();
            }
            UpdateMesh();
            UpdateGradution();
        }

        private void UpdateMesh()
        {
            Vector2 size = rectTransform.sizeDelta;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (int)size.x - 1);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (int)size.x);
        }

        private void UpdateGradution()
        {
            float dataMax = GetAllLineMax();
            if (lastMax != dataMax)
            {
                lastMax = dataMax;
                for (int i = 0; i < graduationList.Count; i++)
                {
                    graduationList[i].text = ((int)(dataMax * i / graduationList.Count)).ToString();
                }
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            float chartHigh = rectTransform.sizeDelta.y;
            int chartWid = (int)(rectTransform.sizeDelta.x / pointWidth) * pointWidth;
            float dataMax = GetAllLineMax();

            // draw coordinate
            Vector3 coordZero = Vector3.zero;

            DrawLine(vh, new Vector3(chartWid + 5, -5), new Vector3(chartWid + 5, chartHigh+0.5f), 1, Color.grey);
            for (int i = 0; i < graduationList.Count; i++)
            {
                Vector3 sp = new Vector3(-5, chartHigh * i / (graduationList.Count - 1));
                Vector3 ep = new Vector3(chartWid + 5, chartHigh * i / (graduationList.Count - 1));
                DrawLine(vh, sp, ep, 0.5f, Color.grey);
            }

            // draw line
            for (int index = 0; index < lineList.Count; index++)
            {
                LineData line = lineList[index];
                if (!line.visible) continue;
                Vector3 lp = Vector3.zero;
                Vector3 np = Vector3.zero;

                for (int i = 0; i < line.dataList.Count; i++)
                {
                    float data = line.dataList[i] * chartHigh / dataMax;
                    np = new Vector3(i * pointWidth, data);
                    if (i > 0)
                    {
                        DrawLine(vh, lp, np, lineSize, line.lineColor);
                    }
                    lp = np;
                }

                // draw point
                for (int i = 0; i < line.dataList.Count; i++)
                {
                    UIVertex[] quadverts = new UIVertex[4];
                    float data = line.dataList[i] * chartHigh / dataMax;
                    Vector3 p = new Vector3(i * pointWidth, data);
                    DrawCube(vh, p, pointSize, line.pointColor);
                }
            }

            DrawLine(vh, new Vector3(0, -6.5f), new Vector3(0, chartHigh + 15), 1.5f, Color.white);
            DrawLine(vh, new Vector3(0, -5), new Vector3(chartWid + 25, -5), 1.5f, Color.white);
        }

        private void DrawLine(VertexHelper vh, Vector3 p1, Vector3 p2, float size, Color color)
        {
            Vector3 v = Vector3.Cross(p2 - p1, Vector3.forward).normalized * size;

            UIVertex[] vertex = new UIVertex[4];
            vertex[0].position = p1 + v;
            vertex[1].position = p2 + v;
            vertex[2].position = p2 - v;
            vertex[3].position = p1 - v;
            for (int j = 0; j < 4; j++)
            {
                vertex[j].color = color;
                vertex[j].uv0 = Vector2.zero;
            }
            vh.AddUIVertexQuad(vertex);
        }

        private void DrawCube(VertexHelper vh, Vector3 p, float size, Color color)
        {
            UIVertex[] vertex = new UIVertex[4];
            vertex[0].position = new Vector3(p.x - pointSize, p.y - pointSize);
            vertex[1].position = new Vector3(p.x + pointSize, p.y - pointSize);
            vertex[2].position = new Vector3(p.x + pointSize, p.y + pointSize);
            vertex[3].position = new Vector3(p.x - pointSize, p.y + pointSize);
            for (int j = 0; j < 4; j++)
            {
                vertex[j].color = color;
                vertex[j].uv0 = Vector2.zero;
            }
            vh.AddUIVertexQuad(vertex);
        }
    }
}
