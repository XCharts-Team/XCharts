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
    public partial class BaseChart
    {
        public Radar radar { get { return m_Radars.Count > 0 ? m_Radars[0] : null; } }
        /// <summary>
        /// 雷达坐标系组件列表。
        /// </summary>
        public List<Radar> radars { get { return m_Radars; } }

        /// <summary>
        /// 移除所有雷达坐标系组件。
        /// </summary>
        public void RemoveRadar()
        {
            m_Radars.Clear();
        }

        /// <summary>
        /// 移除指定Radar的所有Indicator。
        /// </summary>
        /// <param name="radarIndex"></param>
        public void RemoveIndicator(int radarIndex)
        {
            var radar = GetRadar(radarIndex);
            if (radar == null) return;
            radar.indicatorList.Clear();
        }

        /// <summary>
        /// 添加雷达坐标系组件。
        /// </summary>
        public void AddRadar(Radar radar)
        {
            m_Radars.Add(radar);
        }

        /// <summary>
        /// 添加雷达坐标系组件。
        /// </summary>
        /// <param name="shape">形状，圆形还是多边形</param>
        /// <param name="center">中心点，0-1浮点数时表示百分比</param>
        /// <param name="radius">半径，0-1浮点数时表示百分比</param>
        /// <param name="splitNumber">指示器轴的分割段数</param>
        /// <param name="lineWidth">线条宽</param>
        /// <param name="showIndicator">是否显示指示器名称</param>
        /// <param name="showSplitArea">是否显示分割区域</param>
        /// <returns></returns>
        public Radar AddRadar(Radar.Shape shape, Vector2 center, float radius, int splitNumber = 5,
            float lineWidth = 0f, bool showIndicator = true, bool showSplitArea = true)
        {
            var radar = new Radar();
            radar.shape = shape;
            radar.splitNumber = splitNumber;
            radar.radius = radius;
            radar.indicator = showIndicator;
            radar.center[0] = center.x;
            radar.center[1] = center.y;
            radar.splitArea.show = showSplitArea;
            radar.splitLine.lineStyle.width = lineWidth;
            m_Radars.Add(radar);
            return radar;
        }

        public bool AddIndicator(int radarIndex, Radar.Indicator indicator)
        {
            var radar = GetRadar(radarIndex);
            if (radar == null) return false;
            radar.AddIndicator(indicator);
            return true;
        }

        /// <summary>
        /// 添加指示器。
        /// </summary>
        /// <param name="radarIndex">雷达坐标系组件索引，从0开始</param>
        /// <param name="name">指示器名称</param>
        /// <param name="min">指示器最小值</param>
        /// <param name="max">指示器最大值</param>
        /// <returns></returns>
        public Radar.Indicator AddIndicator(int radarIndex, string name, float min, float max)
        {
            var radar = GetRadar(radarIndex);
            if (radar == null) return null;
            return radar.AddIndicator(name, min, max);
        }

        /// <summary>
        /// 更新指示器。
        /// </summary>
        /// <param name="radarIndex">雷达坐标系组件的索引，从0开始</param>
        /// <param name="indicatorIndex">指示器索引，从0开始</param>
        /// <param name="name">指示器名称</param>
        /// <param name="min">指示器最小值</param>
        /// <param name="max">指示器最大值</param>
        /// <returns></returns>
        public bool UpdateIndicator(int radarIndex, int indicatorIndex, string name, float min, float max)
        {
            var radar = GetRadar(radarIndex);
            if (radar == null) return false;
            return radar.UpdateIndicator(indicatorIndex, name, min, max);
        }

        /// <summary>
        /// 获得指定索引的雷达坐标系组件。
        /// </summary>
        /// <param name="radarIndex"></param>
        /// <returns></returns>
        public Radar GetRadar(int radarIndex)
        {
            if (radarIndex < 0 || radarIndex > m_Radars.Count - 1) return null;
            return m_Radars[radarIndex];
        }

        /// <summary>
        /// 获得指定雷达坐标系组件指定索引的指示器。
        /// </summary>
        /// <param name="radarIndex"></param>
        /// <param name="indicatorIndex"></param>
        /// <returns></returns>
        public Radar.Indicator GetIndicator(int radarIndex, int indicatorIndex)
        {
            var radar = GetRadar(radarIndex);
            if (radar != null) return radar.GetIndicator(indicatorIndex);
            else return null;
        }
    }
}