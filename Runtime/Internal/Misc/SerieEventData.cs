using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// the data of serie event.
    /// |serie事件的数据。
    /// </summary>
    public class SerieEventData
    {
        /// <summary>
        /// the position of pointer in chart.
        /// |鼠标在chart中的位置。
        /// </summary>
        public Vector3 pointerPos { get; set; }
        /// <summary>
        /// the index of serie in chart.series.
        /// |在chart.series中的索引。
        /// </summary>
        public int serieIndex { get; set; }
        /// <summary>
        /// the index of data in serie.data.
        /// |在serie.data中的索引。
        /// </summary>
        public int dataIndex { get; set; }
        /// <summary>
        /// the dimension of data.
        /// |数据的维度。
        /// </summary>
        public int dimension { get; set; }
        /// <summary>
        /// the value of data.
        /// |数据的值。
        /// </summary>
        public double value { get; set; }

        public void Reset()
        {
            serieIndex = -1;
            dataIndex = -1;
            dimension = -1;
            value = 0;
            pointerPos = Vector3.zero;
        }
    }
}