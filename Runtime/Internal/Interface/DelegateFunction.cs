
/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// The delegate function for AxisLabel's formatter. |
    /// AxisLabel的formatter自定义委托函数。
    /// </summary>
    /// <param name="labelIndex">label索引</param>
    /// <param name="value">当前label对应的数值数据，Value轴或Time轴有效</param>
    /// <param name="category">当前label对应的类目数据，Category轴有效</param>
    /// <returns>最终显示的文本内容</returns>
    public delegate string DelegateAxisLabelFormatter(int labelIndex, double value, string category);
    /// <summary>
    /// The delegate function for SerieLabel‘s formatter.
    /// SerieLabel的formatter自定义委托函数。
    /// </summary>
    /// <param name="dataIndex">数据索引</param>
    /// <param name="value">数值</param>
    /// <returns>最终显示的文本内容</returns>
    public delegate string DelegateSerieLabelFormatter(int dataIndex, double value);
    /// <summary>
    /// Tooltip的position自定义委托函数。
    /// </summary>
    /// <param name="pos">Tooltip的当前坐标</param>
    /// <returns>Tooltip的最终坐标</returns>
    public delegate Vector3 DelegateTooltipPosition(Vector3 pos);
}