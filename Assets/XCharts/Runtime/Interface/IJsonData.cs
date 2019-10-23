/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

namespace XCharts
{
    /// <summary>
    /// 从json导入数据接口
    /// </summary>
    public interface IJsonData
    {
        void ParseJsonData(string json);
    }
}