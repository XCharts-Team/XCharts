using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example_Test : MonoBehaviour
    {
        BaseChart chart;
        void Awake()
        {
            chart = gameObject.GetComponent<BaseChart>();
            var btnTrans = transform.parent.Find("Button");
            if (btnTrans)
            {
                btnTrans.gameObject.GetComponent<Button>().onClick.AddListener(OnTestBtn);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddData();
                //OnTestBtn();
            }
        }

        void OnTestBtn()
        {
            object[][] m_TestData = new object[][]
            {
                new object[] { "01/06/20", 2.2d, 5.6d },
                new object[] { "22/06/20", 2.4d, 5.3d },
                new object[] { "04/08/21", 4.5d, 5.4d },
                new object[] { "05/08/21", 6.3d, 6.4d },
                new object[] { "06/08/21", 3.1d, 6.4d },
                new object[] { "09/08/21", 3.9d, 6.3d },
                new object[] { "10/08/21", 1.9d, 4.6d },
            };
            chart.ClearData();
            foreach (var list in m_TestData)
            {
                chart.AddXAxisData((string) list[0]);
                chart.AddData(0, (double) list[1]);
                chart.AddData(1, (double) list[2]);
            }
        }

        void AddData()
        {
            chart.ClearData();
            int count = Random.Range(5, 100);
            for (int i = 0; i < count; i++)
            {
                chart.AddXAxisData("x" + i);
                if (Random.Range(1, 3) == 2)
                    chart.AddData(0, Random.Range(-110, 200));
                else
                    chart.AddData(0, Random.Range(-100, 100));
            }
        }
    }
}