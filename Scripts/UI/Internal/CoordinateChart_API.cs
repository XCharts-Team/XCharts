using System.Collections.Generic;

namespace XCharts
{
    /// <summary>
    /// The basic class of rectangular coordinate chart，such as LineChart,BarChart and ScatterChart.
    /// 直角坐标系类型图表的基类，如折线图LineChart，柱状图BarChart，散点图ScatterChart都属于这类型的图表。
    /// 不可用直接将CoordinateChart绑定到GameObject上。
    /// </summary>
    public partial class CoordinateChart
    {
        /// <summary>
        /// The lower left position x of coordinate system.
        /// 坐标系的左下角坐标X。
        /// </summary>
        public float coordinateX { get { return m_Grid.left; } }
        /// <summary>
        /// The lower left position y of coordinate system.
        /// 坐标系的左下角坐标Y。
        /// </summary>
        public float coordinateY { get { return m_Grid.bottom; } }
        /// <summary>
        /// the width of coordinate system。
        /// 坐标系的宽。
        /// </summary>
        public float coordinateWid { get { return chartWidth - m_Grid.left - m_Grid.right; } }
        /// <summary>
        /// the height of coordinate system。
        /// 坐标系的高。
        /// </summary>
        public float coordinateHig { get { return chartHeight - m_Grid.top - m_Grid.bottom; } }
        /// <summary>
        /// grid component.
        /// 网格组件。
        /// </summary>
        public Grid grid { get { return m_Grid; } }
        /// <summary>
        /// the x axises，xAxises[0] is the first x axis, xAxises[1] is the second x axis.
        /// 两个x轴。
        /// </summary>
        public List<XAxis> xAxises { get { return m_XAxises; } }
        /// <summary>
        /// the y axises, yAxises[0] is the first y axis, yAxises[1] is the second y axis.
        /// 两个y轴。
        /// </summary>
        public List<YAxis> yAxises { get { return m_YAxises; } }
        /// <summary>
        /// dataZoom component.
        /// 区域缩放组件。
        /// </summary>
        /// <value></value>
        public DataZoom dataZoom { get { return m_DataZoom; } }


        /// <summary>
        /// Remove all data from series,legend and axis.
        /// It just emptying all of serie's data without emptying the list of series.
        /// 清空所有图例，系列和坐标轴类目数据。系列中指示清空系列中的数据，会保留系列列表。
        /// </summary>
        public override void ClearData()
        {
            base.ClearData();
            ClearAxisData();
        }

        /// <summary>
        /// Remove all data from series,legend and axis.
        /// The series list is also cleared.
        /// 清空所有图例，系列和坐标轴类目数据。系列的列表也会被清空。
        /// </summary>
        public override void RemoveData()
        {
            base.RemoveData();
            ClearAxisData();
        }

        /// <summary>
        /// Remove all data of axises.
        /// 清除所有x轴和y轴的类目数据。
        /// </summary>
        public void ClearAxisData()
        {
            foreach (var item in m_XAxises) item.data.Clear();
            foreach (var item in m_YAxises) item.data.Clear();
        }

        /// <summary>
        /// Add a category data to xAxis.
        /// 添加一个类目数据到指定的x轴。
        /// </summary>
        /// <param name="category">the category data</param>
        /// <param name="xAxisIndex">which xAxis should category add to</param>
        public void AddXAxisData(string category, int xAxisIndex = 0)
        {
            m_XAxises[xAxisIndex].AddData(category, m_MaxCacheDataNumber);
            OnXAxisChanged();
        }

        /// <summary>
        /// Add a category data to yAxis.
        /// 添加一个类目数据到指定的y轴。
        /// </summary>
        /// <param name="category">the category data</param>
        /// <param name="yAxisIndex">which yAxis should category add to</param>
        public void AddYAxisData(string category, int yAxisIndex = 0)
        {
            m_YAxises[yAxisIndex].AddData(category, m_MaxCacheDataNumber);
            OnYAxisChanged();
        }

        /// <summary>
        /// reutrn true when all the show axis is `Value` type.
        /// 纯数值坐标。
        /// </summary>
        /// <returns></returns>
        public bool IsValue()
        {
            foreach (var axis in m_XAxises)
            {
                if (axis.show && !axis.IsValue()) return false;
            }
            foreach (var axis in m_YAxises)
            {
                if (axis.show && !axis.IsValue()) return false;
            }
            return true;
        }

    }
}

