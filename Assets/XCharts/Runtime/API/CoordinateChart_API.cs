/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

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
        public float coordinateX { get { return m_CoordinateX; } }
        /// <summary>
        /// The lower left position y of coordinate system.
        /// 坐标系的左下角坐标Y。
        /// </summary>
        public float coordinateY { get { return m_CoordinateY; } }

        /// <summary>
        /// the width of coordinate system。
        /// 坐标系的宽。
        /// </summary>
        public float coordinateWidth { get { return m_CoordinateWidth; } }
        /// <summary>
        /// the height of coordinate system。
        /// 坐标系的高。
        /// </summary>
        public float coordinateHeight { get { return m_CoordinateHeight; } }
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
        public DataZoom dataZoom { get { return m_DataZoom; } }
        /// <summary>
        /// visualMap component.
        /// 视觉映射组件。
        /// </summary>
        public VisualMap visualMap { get { return m_VisualMap; } }
        /// <summary>
        /// X轴（下）
        /// </summary>
        public XAxis xAxis0 { get { return m_XAxises[0]; } }
        /// <summary>
        /// X轴（上）
        /// </summary>
        public XAxis xAxis1 { get { return m_XAxises[1]; } }
        /// <summary>
        /// Y轴（左）
        /// </summary>
        public YAxis yAxis0 { get { return m_YAxises[0]; } }
        /// <summary>
        /// Y轴（右）
        /// </summary>
        public YAxis yAxis1 { get { return m_YAxises[1]; } }


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
            m_XAxises[xAxisIndex].AddData(category);
        }

        /// <summary>
        /// Add a category data to yAxis.
        /// 添加一个类目数据到指定的y轴。
        /// </summary>
        /// <param name="category">the category data</param>
        /// <param name="yAxisIndex">which yAxis should category add to</param>
        public void AddYAxisData(string category, int yAxisIndex = 0)
        {
            m_YAxises[yAxisIndex].AddData(category);
        }

        /// <summary>
        /// reutrn true when all the show axis is `Value` type.
        /// 纯数值坐标轴（数值轴或对数轴）。
        /// </summary>
        public bool IsValue()
        {
            foreach (var axis in m_XAxises)
            {
                if (axis.show && !axis.IsValue() && !axis.IsLog()) return false;
            }
            foreach (var axis in m_YAxises)
            {
                if (axis.show && !axis.IsValue() && !axis.IsLog()) return false;
            }
            return true;
        }

        /// <summary>
        /// 纯类目轴。
        /// </summary>
        public bool IsCategory()
        {
            foreach (var axis in m_XAxises)
            {
                if (axis.show && !axis.IsCategory()) return false;
            }
            foreach (var axis in m_YAxises)
            {
                if (axis.show && !axis.IsCategory()) return false;
            }
            return true;
        }

        /// <summary>
        /// 坐标是否在坐标轴内。
        /// </summary>
        public bool IsInCooridate(Vector2 local)
        {
            return IsInCooridate(local.x, local.y);
        }

        public bool IsInCooridate(Vector3 local)
        {
            return IsInCooridate(local.x, local.y);
        }

        public bool IsInCooridate(float x, float y)
        {
            if (x < m_CoordinateX - 1 || x > m_CoordinateX + m_CoordinateWidth + 1 ||
                y < m_CoordinateY - 1 || y > m_CoordinateY + m_CoordinateHeight + 1)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 在下一帧刷新DataZoom
        /// </summary>
        public void RefreshDataZoom()
        {
            RefreshDataZoomLabel();
        }

        /// <summary>
        /// 立即刷新数值坐标轴的最大最小值
        /// </summary>
        public void RefreshAxisMinMaxValue()
        {
            CheckMinMaxValue();
        }

        public Vector3 ClampInCoordinate(Vector3 pos)
        {
            if (IsInCooridate(pos)) return pos;
            else
            {
                // var pos = new Vector3(pos.x, pos.y);
                if (pos.x < m_CoordinateX) pos.x = m_CoordinateX;
                if (pos.x > m_CoordinateX + m_CoordinateWidth) pos.x = m_CoordinateX + m_CoordinateWidth;
                if (pos.y < m_CoordinateY) pos.y = m_CoordinateY;
                if (pos.y > m_CoordinateY + m_CoordinateHeight) pos.y = m_CoordinateY + m_CoordinateHeight;
                return pos;
            }
        }

        /// <summary>
        /// 转换X轴和Y轴的配置
        /// </summary>
        /// <param name="index">坐标轴索引，0或1</param>
        public void CovertXYAxis(int index)
        {
            if (index >= 0 && index <= 1)
            {
                var xAxis = m_XAxises[index];
                var yAxis = m_YAxises[index];
                var tempX = m_XAxises[index].Clone();
                xAxis.Copy(m_YAxises[index]);
                yAxis.Copy(tempX);
                xAxis.runtimeZeroXOffset = 0;
                xAxis.runtimeZeroYOffset = 0;
                yAxis.runtimeZeroXOffset = 0;
                yAxis.runtimeZeroYOffset = 0;
                xAxis.runtimeMinValue = 0;
                xAxis.runtimeMaxValue = 0;
                yAxis.runtimeMinValue = 0;
                yAxis.runtimeMaxValue = 0;
                RefreshChart();
            }
        }

        /// <summary>
        /// 更新坐标系原点和宽高
        /// </summary>
        public void UpdateCoordinate()
        {
            m_CoordinateX = m_ChartX + m_Grid.left;
            m_CoordinateY = m_ChartY + m_Grid.bottom;
            m_CoordinateWidth = m_ChartWidth - m_Grid.left - m_Grid.right;
            m_CoordinateHeight = m_ChartHeight - m_Grid.top - m_Grid.bottom;
        }

        /// <summary>
        /// 设置可缓存的最大数据量。当数据量超过该值时，会自动删除第一个值再加入最新值。
        /// </summary>
        public void SetMaxCache(int maxCache)
        {
            foreach (var serie in m_Series.list) serie.maxCache = maxCache;
            foreach (var axis in m_XAxises) axis.maxCache = maxCache;
            foreach (var axis in m_YAxises) axis.maxCache = maxCache;
        }
    }
}

