/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;

namespace XCharts
{
    public static class FormatterHelper
    {
        public const string PH_NN = "\n";
        private static Regex s_Regex = new Regex(@"{([a-d|.]\d*)(:\d+(-\d+)?)?(:[c-g|x|p|r]\d*)?}", RegexOptions.IgnoreCase);
        private static Regex s_RegexSub = new Regex(@"(\w?-?\d+)|(\w)|(\.)", RegexOptions.IgnoreCase);
        private static Regex s_RegexN = new Regex(@"^\d+", RegexOptions.IgnoreCase);
        private static Regex s_RegexN_N = new Regex(@"\d+-\d+", RegexOptions.IgnoreCase);
        private static Regex s_RegexFn = new Regex(@"[c-g|x|p|r]\d*", RegexOptions.IgnoreCase);
        private static Regex s_RegexNewLine = new Regex(@"[\\|/]+n", RegexOptions.IgnoreCase);

        /// <summary>
        /// 替换字符串中的通配符，支持的通配符有{.}、{a}、{b}、{c}、{d}。
        /// </summary>
        /// <param name="content">要替换的字符串</param>
        /// <param name="dataIndex">选中的数据项serieData索引</param>
        /// <param name="numericFormatter">默认的数字格式化</param>
        /// <param name="serie">选中的serie</param>
        /// <param name="series">所有serie</param>
        /// <param name="themeInfo">用来获取指定index的颜色</param>
        /// <param name="category">选中的类目，一般用在折线图和柱状图</param>
        /// <param name="dataZoom">dataZoom</param>
        /// <returns></returns>
        public static bool ReplaceContent(ref string content, int dataIndex, string numericFormatter, Serie serie, Series series,
           ThemeInfo themeInfo, string category = null, DataZoom dataZoom = null)
        {
            var foundDot = false;
            var mc = s_Regex.Matches(content);
            foreach (var m in mc)
            {
                var old = m.ToString();
                var args = s_RegexSub.Matches(m.ToString());
                var argsCount = args.Count;
                if (argsCount <= 0) continue;
                int targetIndex = 0;
                char p = GetSerieIndex(args[0].ToString(), ref targetIndex);
                if (targetIndex >= 0)
                {
                    serie = series.GetSerie(targetIndex);
                    if (serie == null) continue;
                }
                else if (serie != null)
                {
                    targetIndex = serie.index;
                }
                else
                {
                    serie = series.GetSerie(0);
                    targetIndex = 0;
                }
                if (serie == null) continue;
                if (p == '.')
                {
                    var bIndex = targetIndex;
                    if (argsCount >= 2)
                    {
                        var args1Str = args[1].ToString();
                        if (s_RegexN.IsMatch(args1Str)) bIndex = int.Parse(args1Str);
                    }
                    content = content.Replace(old, ChartCached.ColorToDotStr(themeInfo.GetColor(bIndex)));
                    foundDot = true;
                }
                else if (p == 'a' || p == 'A')
                {
                    if (argsCount == 1)
                    {
                        content = content.Replace(old, serie.name);
                    }
                }
                else if (p == 'b' || p == 'B')
                {
                    var bIndex = dataIndex;
                    if (argsCount >= 2)
                    {
                        var args1Str = args[1].ToString();
                        if (s_RegexN.IsMatch(args1Str)) bIndex = int.Parse(args1Str);
                    }
                    var needCategory = serie.type == SerieType.Line || serie.type == SerieType.Bar;
                    if (needCategory)
                    {
                        content = content.Replace(old, category);
                    }
                    else
                    {
                        var serieData = serie.GetSerieData(bIndex, dataZoom);
                        content = content.Replace(old, serieData.name);
                    }
                }
                else if (p == 'c' || p == 'C' || p == 'd' || p == 'D')
                {
                    var isPercent = p == 'd' || p == 'D';
                    var bIndex = dataIndex;
                    var dimensionIndex = -1;
                    if (argsCount >= 2)
                    {
                        var args1Str = args[1].ToString();
                        if (s_RegexFn.IsMatch(args1Str))
                        {
                            numericFormatter = args1Str;
                        }
                        else if (s_RegexN_N.IsMatch(args1Str))
                        {
                            var temp = args1Str.Split('-');
                            bIndex = int.Parse(temp[0]);
                            dimensionIndex = int.Parse(temp[1]);
                        }
                        else if (s_RegexN.IsMatch(args1Str))
                        {
                            dimensionIndex = int.Parse(args1Str);
                        }
                        else
                        {
                            Debug.LogError("unmatch:" + args1Str);
                            continue;
                        }
                    }
                    if (argsCount >= 3)
                    {
                        numericFormatter = args[2].ToString();
                    }
                    if (dimensionIndex == -1) dimensionIndex = 1;
                    if (numericFormatter == string.Empty)
                    {
                        numericFormatter = SerieHelper.GetNumericFormatter(serie, serie.GetSerieData(bIndex));
                    }
                    var value = serie.GetData(bIndex, dimensionIndex, dataZoom);
                    if (isPercent)
                    {
                        var total = serie.GetDataTotal(dimensionIndex);
                        var percent = total == 0 ? 0 : value / serie.yTotal * 100;
                        content = content.Replace(old, ChartCached.FloatToStr(percent, numericFormatter));
                    }
                    else
                    {
                        content = content.Replace(old, ChartCached.FloatToStr(value, numericFormatter));
                    }
                }
            }
            content = s_RegexNewLine.Replace(content, PH_NN);
            return foundDot;
        }

        private static char GetSerieIndex(string strType, ref int index)
        {
            index = -1;
            if (strType.Length > 1)
            {
                if (!int.TryParse(strType.Substring(1), out index))
                {
                    index = -1;
                }
            }
            return strType.ElementAt(0);
        }

        public static string TrimAndReplaceLine(StringBuilder sb)
        {
            return TrimAndReplaceLine(sb.ToString());
        }

        public static string TrimAndReplaceLine(string content)
        {
            return s_RegexNewLine.Replace(content.Trim(), PH_NN);
        }
    }
}