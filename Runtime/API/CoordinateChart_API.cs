/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

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
        /// grid component.
        /// 网格组件。
        /// </summary>
        public Grid grid { get { return m_Grids.Count > 0 ? m_Grids[0] : null; } }
        public List<Grid> grids { get { return m_Grids; } }
        /// <summary>
        /// the x Axes，xAxes[0] is the first x axis, xAxes[1] is the second x axis.
        /// 两个x轴。
        /// </summary>
        public List<XAxis> xAxes { get { return m_XAxes; } }
        /// <summary>
        /// the y Axes, yAxes[0] is the first y axis, yAxes[1] is the second y axis.
        /// 两个y轴。
        /// </summary>
        public List<YAxis> yAxes { get { return m_YAxes; } }

        /// <summary>
        /// X轴（下）
        /// </summary>
        public XAxis xAxis0 { get { return m_XAxes.Count > 0 ? m_XAxes[0] : null; } }
        /// <summary>
        /// X轴（上）
        /// </summary>
        public XAxis xAxis1 { get { return m_XAxes.Count > 1 ? m_XAxes[1] : null; } }
        /// <summary>
        /// Y轴（左）
        /// </summary>
        public YAxis yAxis0 { get { return m_YAxes.Count > 0 ? m_YAxes[0] : null; } }
        /// <summary>
        /// Y轴（右）
        /// </summary>
        public YAxis yAxis1 { get { return m_YAxes.Count > 1 ? m_YAxes[1] : null; } }


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
        /// Remove all data of Axes.
        /// 清除所有x轴和y轴的类目数据。
        /// </summary>
        public void ClearAxisData()
        {
            foreach (var axis in m_XAxes)
            {
                axis.data.Clear();
                axis.SetAllDirty();
            }
            foreach (var axis in m_YAxes)
            {
                axis.data.Clear();
                axis.SetAllDirty();
            }
        }

        /// <summary>
        /// Add a category data to xAxis.
        /// 添加一个类目数据到指定的x轴。
        /// </summary>
        /// <param name="category">the category data</param>
        /// <param name="xAxisIndex">which xAxis should category add to</param>
        public void AddXAxisData(string category, int xAxisIndex = 0)
        {
            var xAxis = GetXAxis(xAxisIndex);
            if (xAxis != null)
            {
                xAxis.AddData(category);
            }
        }

        /// <summary>
        /// Update category data.
        /// 更新X轴类目数据。
        /// </summary>
        /// <param name="index">the index of category data</param>
        /// <param name="category"></param>
        /// <param name="xAxisIndex">which xAxis index to update to</param>
        public void UpdateXAxisData(int index, string category, int xAxisIndex = 0)
        {
            var xAxis = GetXAxis(xAxisIndex);
            if (xAxis != null)
            {
                xAxis.UpdateData(index, category);
            }
        }

        /// <summary>
        /// Add an icon to xAxis.
        /// 添加一个图标到指定的x轴。
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="xAxisIndex"></param>
        public void AddXAxisIcon(Sprite icon, int xAxisIndex = 0)
        {
            var xAxis = GetXAxis(xAxisIndex);
            if (xAxis != null)
            {
                xAxis.AddIcon(icon);
            }
        }

        /// <summary>
        /// Update xAxis icon.
        /// 更新X轴图标。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="icon"></param>
        /// <param name="xAxisIndex"></param>
        public void UdpateXAxisIcon(int index, Sprite icon, int xAxisIndex = 0)
        {
            var xAxis = GetXAxis(xAxisIndex);
            if (xAxis != null)
            {
                xAxis.UpdateIcon(index, icon);
            }
        }

        /// <summary>
        /// Add a category data to yAxis.
        /// 添加一个类目数据到指定的y轴。
        /// </summary>
        /// <param name="category">the category data</param>
        /// <param name="yAxisIndex">which yAxis should category add to</param>
        public void AddYAxisData(string category, int yAxisIndex = 0)
        {
            var yAxis = GetYAxis(yAxisIndex);
            if (yAxis != null)
            {
                yAxis.AddData(category);
            }
        }

        /// <summary>
        /// Update category data.
        /// 更新Y轴类目数据。
        /// </summary>
        /// <param name="index">the index of category data</param>
        /// <param name="category"></param>
        /// <param name="yAxisIndex">which yAxis index to update to</param>
        public void UpdateYAxisData(int index, string category, int yAxisIndex = 0)
        {
            var yAxis = GetYAxis(yAxisIndex);
            if (yAxis != null)
            {
                yAxis.UpdateData(index, category);
            }
        }

        /// <summary>
        /// Add an icon to yAxis.
        /// 添加一个图标到指定的y轴。
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="yAxisIndex"></param>
        public void AddYAxisIcon(Sprite icon, int yAxisIndex = 0)
        {
            var yAxis = GetYAxis(yAxisIndex);
            if (yAxis != null)
            {
                yAxis.AddIcon(icon);
            }
        }

        /// <summary>
        /// 更新Y轴图标。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="icon"></param>
        /// <param name="yAxisIndex"></param>
        public void UpdateYAxisIcon(int index, Sprite icon, int yAxisIndex = 0)
        {
            var yAxis = GetYAxis(yAxisIndex);
            if (yAxis != null)
            {
                yAxis.UpdateIcon(index, icon);
            }
        }

        /// <summary>
        /// reutrn true when all the show axis is `Value` type.
        /// 纯数值坐标轴（数值轴或对数轴）。
        /// </summary>
        public bool IsValue()
        {
            foreach (var axis in m_XAxes)
            {
                if (axis.show && !axis.IsValue() && !axis.IsLog()) return false;
            }
            foreach (var axis in m_YAxes)
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
            foreach (var axis in m_XAxes)
            {
                if (axis.show && !axis.IsCategory()) return false;
            }
            foreach (var axis in m_YAxes)
            {
                if (axis.show && !axis.IsCategory()) return false;
            }
            return true;
        }

        /// <summary>
        /// 坐标是否在坐标轴内。
        /// </summary>
        public bool IsInGrid(Grid grid, Vector2 local)
        {
            return IsInGrid(grid, local.x, local.y);
        }

        public bool IsInGrid(Grid grid, Vector3 local)
        {
            return IsInGrid(grid, local.x, local.y);
        }

        public bool IsInGrid(Grid grid, float x, float y)
        {
            if (x < grid.runtimeX - 1 || x > grid.runtimeX + grid.runtimeWidth + 1 ||
                y < grid.runtimeY - 1 || y > grid.runtimeY + grid.runtimeHeight + 1)
            {
                return false;
            }
            return true;
        }

        public bool IsInAnyGrid(Vector2 local)
        {
            foreach (var grid in m_Grids)
            {
                if (IsInGrid(grid, local)) return true;
            }
            return false;
        }

        public Grid GetGrid(Vector2 local)
        {
            for (int i = 0; i < m_Grids.Count; i++)
            {
                var grid = m_Grids[i];
                grid.index = i;
                if (IsInGrid(grid, local)) return grid;
            }
            return null;
        }

        /// <summary>
        /// 在下一帧刷新DataZoom
        /// </summary>
        public void RefreshDataZoom()
        {
            foreach (var handler in m_ComponentHandlers)
            {
                if (handler is DataZoomHandler)
                {
                    (handler as DataZoomHandler).RefreshDataZoomLabel();
                }
            }
        }

        /// <summary>
        /// 立即刷新数值坐标轴的最大最小值
        /// </summary>
        public void RefreshAxisMinMaxValue()
        {
            CheckMinMaxValue();
        }

        public Vector3 ClampInGrid(Grid grid, Vector3 pos)
        {
            if (IsInGrid(grid, pos)) return pos;
            else
            {
                // var pos = new Vector3(pos.x, pos.y);
                if (pos.x < grid.runtimeX) pos.x = grid.runtimeX;
                if (pos.x > grid.runtimeX + grid.runtimeWidth) pos.x = grid.runtimeX + grid.runtimeWidth;
                if (pos.y < grid.runtimeY) pos.y = grid.runtimeY;
                if (pos.y > grid.runtimeY + grid.runtimeHeight) pos.y = grid.runtimeY + grid.runtimeHeight;
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
                var xAxis = m_XAxes[index];
                var yAxis = m_YAxes[index];
                var tempX = m_XAxes[index].Clone();
                xAxis.Copy(m_YAxes[index]);
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
            foreach (var grid in m_Grids)
            {
                grid.UpdateRuntimeData(m_ChartX, m_ChartY, m_ChartWidth, m_ChartHeight);
            }
            foreach (var dataZoom in m_DataZooms)
            {
                dataZoom.UpdateRuntimeData(m_ChartX, m_ChartY, m_ChartWidth, m_ChartHeight);
            }
        }

        /// <summary>
        /// 设置可缓存的最大数据量。当数据量超过该值时，会自动删除第一个值再加入最新值。
        /// </summary>
        public void SetMaxCache(int maxCache)
        {
            foreach (var serie in m_Series.list) serie.maxCache = maxCache;
            foreach (var axis in m_XAxes) axis.maxCache = maxCache;
            foreach (var axis in m_YAxes) axis.maxCache = maxCache;
        }

        public Grid GetGrid(int index)
        {
            if (index >= 0 && index < m_Grids.Count) return m_Grids[index];
            else return null;
        }

        public XAxis GetXAxis(int index)
        {
            if (index >= 0 && index < m_XAxes.Count) return m_XAxes[index];
            else return null;
        }

        public YAxis GetYAxis(int index)
        {
            if (index >= 0 && index < m_YAxes.Count) return m_YAxes[index];
            else return null;
        }
    }
}

