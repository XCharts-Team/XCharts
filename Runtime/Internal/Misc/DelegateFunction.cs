

using System.Collections.Generic;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    /// <summary>
    /// The delegate function for AxisLabel's formatter.
    /// |AxisLabel的formatter自定义委托。
    /// </summary>
    /// <param name="labelIndex">label索引</param>
    /// <param name="value">当前label对应的数值数据，Value轴或Time轴有效</param>
    /// <param name="category">当前label对应的类目数据，Category轴有效</param>
    /// <returns>最终显示的文本内容</returns>
    public delegate string AxisLabelFormatterFunction(int labelIndex, double value, string category);
    /// <summary>
    /// The delegate function for SerieLabel‘s formatter.
    /// |SerieLabel的formatter自定义委托。
    /// </summary>
    /// <param name="dataIndex">数据索引</param>
    /// <param name="value">数值</param>
    /// <returns>最终显示的文本内容</returns>
    public delegate string SerieLabelFormatterFunction(int dataIndex, double value);
    public delegate float AnimationDelayFunction(int dataIndex);
    public delegate float AnimationDurationFunction(int dataIndex);
    /// <summary>
    /// 获取标记大小的回调。
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public delegate float SymbolSizeFunction(List<double> data);
    public delegate void CustomDrawGaugePointerFunction(VertexHelper vh, int serieIndex, int dataIndex, float currentAngle);
}