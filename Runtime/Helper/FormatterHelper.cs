using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace XCharts.Runtime
{
    public static class FormatterHelper
    {
        public const string PH_NN = "\n";
        private static Regex s_Regex = new Regex(@"{([a-h|.]\d*)(:\d+(-\d+)?)?(:[c-g|x|p|r]\d*|:0\.#*)?}", RegexOptions.IgnoreCase);
        private static Regex s_RegexSub = new Regex(@"(0\.#*)|(\d+-\d+)|(\w+)|(\.)", RegexOptions.IgnoreCase);
        private static Regex s_RegexN = new Regex(@"^\d+", RegexOptions.IgnoreCase);
        private static Regex s_RegexN_N = new Regex(@"\d+-\d+", RegexOptions.IgnoreCase);
        private static Regex s_RegexFn = new Regex(@"[c-g|x|p|r]\d*|0\.#*", RegexOptions.IgnoreCase);
        private static Regex s_RegexNewLine = new Regex(@"[\\|/]+n|</br>|<br>|<br/>", RegexOptions.IgnoreCase);
        private static Regex s_RegexForAxisLabel = new Regex(@"{value(:[c-g|x|p|r]\d*)?}", RegexOptions.IgnoreCase);
        private static Regex s_RegexSubForAxisLabel = new Regex(@"(value)|([c-g|x|p|r]\d*)", RegexOptions.IgnoreCase);
        private static Regex s_RegexForSerieLabel = new Regex(@"{[a-h|\.]\d*(:[c-g|x|p|r]\d*)?}", RegexOptions.IgnoreCase);
        private static Regex s_RegexSubForSerieLabel = new Regex(@"(\.)|([a-h]\d*)|([c-g|x|p|r]\d*)", RegexOptions.IgnoreCase);

        public static bool NeedFormat(string content)
        {
            return !string.IsNullOrEmpty(content) && content.IndexOf('{') >= 0;
        }

        /// <summary>
        /// 替换字符串中的通配符，支持的通配符有{.}、{a}、{b}、{c}、{d}、{e}、{f}、{g}、{h}。
        /// </summary>
        /// <param name="content">要替换的字符串</param>
        /// <param name="dataIndex">选中的数据项serieData索引</param>
        /// <param name="numericFormatter">默认的数字格式化</param>
        /// <param name="serie">选中的serie</param>
        /// <param name="series">所有serie</param>
        /// <param name="theme">用来获取指定index的颜色</param>
        /// <param name="category">选中的类目，一般用在折线图和柱状图</param>
        /// <returns></returns>
        public static bool ReplaceContent(ref string content, int dataIndex, string numericFormatter, Serie serie,
            BaseChart chart, string colorName = null)
        {
            var foundDot = false;
            var mc = s_Regex.Matches(content);
            if (dataIndex < 0)
            {
                dataIndex = serie != null ? serie.context.pointerItemDataIndex : 0;
            }
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
                    serie = chart.GetSerie(targetIndex);
                    if (serie == null) continue;
                }
                else if (serie != null)
                {
                    targetIndex = serie.index;
                }
                else
                {
                    serie = chart.GetSerie(0);
                    targetIndex = 0;
                }
                if (serie == null) continue;
                if (p == '.' || p == 'h' || p == 'H')
                {
                    var bIndex = dataIndex;
                    if (argsCount >= 2)
                    {
                        var args1Str = args[1].ToString();
                        if (s_RegexN.IsMatch(args1Str)) bIndex = int.Parse(args1Str);
                    }
                    var color = string.IsNullOrEmpty(colorName) ?
                        (Color)chart.GetMarkColor(serie, serie.GetSerieData(bIndex)) :
                        SeriesHelper.GetNameColor(chart, bIndex, colorName);
                    if (p == '.')
                    {
                        content = content.Replace(old, ChartCached.ColorToDotStr(color));
                        foundDot = true;
                    }
                    else
                    {
                        content = content.Replace(old, "#" + ChartCached.ColorToStr(color));
                    }
                }
                else if (p == 'a' || p == 'A')
                {
                    if (argsCount == 1)
                    {
                        content = content.Replace(old, serie.serieName);
                    }
                }
                else if (p == 'b' || p == 'B' || p == 'e' || p == 'E')
                {
                    var bIndex = dataIndex;
                    if (argsCount >= 2)
                    {
                        var args1Str = args[1].ToString();
                        if (s_RegexN.IsMatch(args1Str)) bIndex = int.Parse(args1Str);
                    }
                    var needCategory = p != 'e' && p != 'E' && serie.defaultColorBy != SerieColorBy.Data;
                    if (needCategory)
                    {
                        var category = chart.GetTooltipCategory(serie);
                        content = content.Replace(old, category);
                    }
                    else
                    {
                        var serieData = serie.GetSerieData(bIndex);
                        content = content.Replace(old, serieData.name);
                    }
                }
                else if (p == 'g' || p == 'G')
                {
                    content = content.Replace(old, ChartCached.NumberToStr(serie.dataCount, ""));
                }
                else if (p == 'c' || p == 'C' || p == 'd' || p == 'D' || p == 'f' || p == 'f')
                {
                    var isPercent = p == 'd' || p == 'D';
                    var isTotal = p == 'f' || p == 'f';
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
                        numericFormatter = SerieHelper.GetNumericFormatter(serie, serie.GetSerieData(bIndex), "");
                    }
                    var value = serie.GetData(bIndex, dimensionIndex);
                    var ignore = serie.IsIgnoreIndex(bIndex);
                    if (isPercent)
                    {
                        var total = serie.GetDataTotal(dimensionIndex, serie.GetSerieData(bIndex));
                        var percent = total == 0 ? 0 : value / serie.yTotal * 100;
                        content = content.Replace(old, ChartCached.FloatToStr(percent, numericFormatter));
                    }
                    else if (isTotal)
                    {
                        var total = serie.GetDataTotal(dimensionIndex, serie.GetSerieData(bIndex));
                        content = content.Replace(old, ChartCached.FloatToStr(total, numericFormatter));
                    }
                    else
                    {
                        if (ignore)
                            content = content.Replace(old, "-");
                        else
                            content = content.Replace(old, ChartCached.FloatToStr(value, numericFormatter));
                    }
                }
            }
            content = s_RegexNewLine.Replace(content, PH_NN);
            return foundDot;
        }

        public static void ReplaceSerieLabelContent(ref string content, string numericFormatter, int dataCount, double value, double total,
            string serieName, string category, string dataName, Color color, SerieData serieData)
        {
            var mc = s_RegexForSerieLabel.Matches(content);
            foreach (var m in mc)
            {
                var old = m.ToString();
                var args = s_RegexSubForSerieLabel.Matches(old);
                var argsCount = args.Count;
                if (argsCount <= 0) continue;
                var pstr = args[0].ToString();
                var p = pstr.ElementAt(0);
                var pIndex = -1;
                if (pstr.Length > 1)
                {
                    int.TryParse(pstr.Substring(1, pstr.Length - 1), out pIndex);
                }
                if (argsCount >= 2)
                {
                    numericFormatter = args[1].ToString();
                }
                if (p == '.')
                {
                    content = content.Replace(old, ChartCached.ColorToDotStr(color));
                }
                else if (p == 'a' || p == 'A')
                {
                    content = content.Replace(old, serieName);
                }
                else if (p == 'b' || p == 'B')
                {
                    content = content.Replace(old, category);
                }
                else if (p == 'e' || p == 'E')
                {
                    content = content.Replace(old, dataName);
                }
                else if (p == 'd' || p == 'D')
                {
                    if (serieData != null && serieData.ignore)
                        content = content.Replace(old, "-");
                    else
                    {
                        var rate = pIndex >= 0 && serieData != null ?
                            (value == 0 ? 0 : serieData.GetData(pIndex) / value * 100) :
                            (total == 0 ? 0 : value / total * 100);
                        content = content.Replace(old, ChartCached.NumberToStr(rate, numericFormatter));
                    }
                }
                else if (p == 'c' || p == 'C')
                {
                    if (serieData != null && serieData.ignore)
                        content = content.Replace(old, "-");
                    else if (serieData != null && pIndex >= 0)
                        content = content.Replace(old, ChartCached.NumberToStr(serieData.GetData(pIndex), numericFormatter));
                    else
                        content = content.Replace(old, ChartCached.NumberToStr(value, numericFormatter));
                }
                else if (p == 'f' || p == 'f')
                {
                    content = content.Replace(old, ChartCached.NumberToStr(total, numericFormatter));
                }
                else if (p == 'g' || p == 'G')
                {
                    content = content.Replace(old, ChartCached.NumberToStr(dataCount, numericFormatter));
                }
                else if (p == 'h' || p == 'H')
                {
                    content = content.Replace(old, "#" + ChartCached.ColorToStr(color));
                }
            }
            content = TrimAndReplaceLine(content);
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

        public static void ReplaceAxisLabelContent(ref string content, string numericFormatter, double value)
        {
            var mc = s_RegexForAxisLabel.Matches(content);
            foreach (var m in mc)
            {
                var old = m.ToString();
                var args = s_RegexSubForAxisLabel.Matches(m.ToString());
                var argsCount = args.Count;
                if (argsCount <= 0) continue;
                if (argsCount >= 2)
                {
                    numericFormatter = args[1].ToString();
                }
                content = content.Replace(old, ChartCached.FloatToStr(value, numericFormatter));
            }
            content = TrimAndReplaceLine(content);
        }

        public static void ReplaceAxisLabelContent(ref string content, string value)
        {
            var mc = s_RegexForAxisLabel.Matches(content);
            foreach (var m in mc)
            {
                var old = m.ToString();
                var args = s_RegexSubForAxisLabel.Matches(m.ToString());
                var argsCount = args.Count;
                if (argsCount <= 0) continue;
                content = content.Replace(old, value);
            }
            content = TrimAndReplaceLine(content);
        }

    }
}