using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    internal static class LineHelper
    {
        private static List<Vector3> s_CurvesPosList = new List<Vector3>();

        public static int GetDataAverageRate(Serie serie, GridCoord grid, int maxCount, bool isYAxis)
        {
            var sampleDist = serie.sampleDist;
            var rate = 0;
            var width = isYAxis ? grid.context.height : grid.context.width;
            if (sampleDist > 0)
                rate = (int) ((maxCount - serie.minShow) / (width / sampleDist));
            if (rate < 1)
                rate = 1;
            return rate;
        }

        public static void DrawSerieLineArea(VertexHelper vh, Serie serie, Serie lastStackSerie,
            ThemeStyle theme, VisualMap visualMap, bool isY, Axis axis, Axis relativedAxis, GridCoord grid)
        {
            if (serie.areaStyle == null || !serie.areaStyle.show)
                return;

            var srcAreaColor = SerieHelper.GetAreaColor(serie, null, theme, serie.context.colorIndex, false);
            var srcAreaToColor = SerieHelper.GetAreaToColor(serie, null, theme, serie.context.colorIndex, false);
            var gridXY = (isY ? grid.context.x : grid.context.y);
            if (lastStackSerie == null)
            {
                DrawSerieLineNormalArea(vh, serie, isY,
                    gridXY + relativedAxis.context.offset,
                    gridXY,
                    gridXY + (isY ? grid.context.width : grid.context.height),
                    srcAreaColor,
                    srcAreaToColor,
                    visualMap,
                    axis,
                    relativedAxis,
                    grid);
            }
            else
            {
                DrawSerieLineStackArea(vh, serie, lastStackSerie, isY,
                    gridXY + relativedAxis.context.offset,
                    gridXY,
                    gridXY + (isY ? grid.context.width : grid.context.height),
                    srcAreaColor,
                    srcAreaToColor,
                    visualMap);
            }
        }

        private static void DrawSerieLineNormalArea(VertexHelper vh, Serie serie, bool isY,
            float zero, float min, float max, Color32 areaColor, Color32 areaToColor,
            VisualMap visualMap, Axis axis, Axis relativedAxis, GridCoord grid)
        {
            var points = serie.context.drawPoints;
            var count = points.Count;
            if (count < 2)
                return;

            var isBreak = false;
            var lp = Vector3.zero;
            var isVisualMapGradient = VisualMapHelper.IsNeedAreaGradient(visualMap);
            var areaLerp = !ChartHelper.IsValueEqualsColor(areaColor, areaToColor);
            var zsp = isY ?
                new Vector3(zero, points[0].position.y) :
                new Vector3(points[0].position.x, zero);
            var zep = isY ?
                new Vector3(zero, points[count - 1].position.y) :
                new Vector3(points[count - 1].position.x, zero);

            var lastDataIsIgnore = false;
            for (int i = 0; i < points.Count; i++)
            {
                var tp = points[i].position;
                var isIgnore = points[i].isIgnoreBreak;
                var color = areaColor;
                var toColor = areaToColor;
                var lerp = areaLerp;

                if (serie.animation.CheckDetailBreak(tp, isY))
                {
                    isBreak = true;

                    var progress = serie.animation.GetCurrDetail();
                    var ip = Vector3.zero;
                    var axisStartPos = isY ? new Vector3(-10000, progress) : new Vector3(progress, -10000);
                    var axisEndPos = isY ? new Vector3(10000, progress) : new Vector3(progress, 10000);

                    if (UGLHelper.GetIntersection(lp, tp, axisStartPos, axisEndPos, ref ip))
                        tp = ip;
                }
                var zp = isY ? new Vector3(zero, tp.y) : new Vector3(tp.x, zero);
                if (isVisualMapGradient)
                {
                    color = VisualMapHelper.GetLineGradientColor(visualMap, zp, grid, axis, relativedAxis, areaColor);
                    toColor = VisualMapHelper.GetLineGradientColor(visualMap, tp, grid, axis, relativedAxis, areaToColor);
                    lerp = true;
                }
                if (i > 0)
                {
                    if ((lp.y - zero > 0 && tp.y - zero < 0) || (lp.y - zero < 0 && tp.y - zero > 0))
                    {
                        var ip = Vector3.zero;
                        if (UGLHelper.GetIntersection(lp, tp, zsp, zep, ref ip))
                        {
                            if (lerp)
                                AddVertToVertexHelperWithLerpColor(vh, ip, ip, color, toColor, isY, min, max, i > 0);
                            else
                            {
                                if (lastDataIsIgnore)
                                    UGL.AddVertToVertexHelper(vh, ip, ip, ColorUtil.clearColor32, true);

                                UGL.AddVertToVertexHelper(vh, ip, ip, toColor, color, i > 0);

                                if (isIgnore)
                                    UGL.AddVertToVertexHelper(vh, ip, ip, ColorUtil.clearColor32, true);
                            }
                        }
                    }
                }

                if (lerp)
                    AddVertToVertexHelperWithLerpColor(vh, tp, zp, color, toColor, isY, min, max, i > 0);
                else
                {
                    if (lastDataIsIgnore)
                        UGL.AddVertToVertexHelper(vh, tp, zp, ColorUtil.clearColor32, true);

                    UGL.AddVertToVertexHelper(vh, tp, zp, toColor, color, i > 0);

                    if (isIgnore)
                        UGL.AddVertToVertexHelper(vh, tp, zp, ColorUtil.clearColor32, true);
                }
                lp = tp;
                lastDataIsIgnore = isIgnore;
                if (isBreak)
                    break;
            }
        }

        private static void DrawSerieLineStackArea(VertexHelper vh, Serie serie, Serie lastStackSerie, bool isY,
            float zero, float min, float max, Color32 color, Color32 toColor, VisualMap visualMap)
        {
            if (lastStackSerie == null)
                return;

            var upPoints = serie.context.drawPoints;
            var downPoints = lastStackSerie.context.drawPoints;
            var upCount = upPoints.Count;
            var downCount = downPoints.Count;

            if (upCount <= 0 || downCount <= 0)
                return;

            var lerp = !ChartHelper.IsValueEqualsColor(color, toColor);
            var ltp = upPoints[0].position;
            var lbp = downPoints[0].position;

            if (lerp)
                AddVertToVertexHelperWithLerpColor(vh, ltp, lbp, color, toColor, isY, min, max, false);
            else
                UGL.AddVertToVertexHelper(vh, ltp, lbp, color, false);

            int u = 1, d = 1;
            var isBreakTop = false;
            var isBreakBottom = false;

            while ((u < upCount || d < downCount))
            {
                var tp = u < upCount ? upPoints[u].position : upPoints[upCount - 1].position;
                var bp = d < downCount ? downPoints[d].position : downPoints[downCount - 1].position;

                var tnp = (u + 1) < upCount ? upPoints[u + 1].position : upPoints[upCount - 1].position;
                var bnp = (d + 1) < downCount ? downPoints[d + 1].position : downPoints[downCount - 1].position;

                if (serie.animation.CheckDetailBreak(tp, isY))
                {
                    isBreakTop = true;

                    var progress = serie.animation.GetCurrDetail();
                    var ip = Vector3.zero;

                    if (UGLHelper.GetIntersection(ltp, tp,
                            new Vector3(progress, -10000),
                            new Vector3(progress, 10000), ref ip))
                        tp = ip;
                    else
                        tp = new Vector3(progress, tp.y);
                }
                if (serie.animation.CheckDetailBreak(bp, isY))
                {
                    isBreakBottom = true;

                    var progress = serie.animation.GetCurrDetail();
                    var ip = Vector3.zero;

                    if (UGLHelper.GetIntersection(lbp, bp,
                            new Vector3(progress, -10000),
                            new Vector3(progress, 10000), ref ip))
                        bp = ip;
                    else
                        bp = new Vector3(progress, bp.y);
                }

                if (lerp)
                    AddVertToVertexHelperWithLerpColor(vh, tp, bp, color, toColor, isY, min, max, true);
                else
                    UGL.AddVertToVertexHelper(vh, tp, bp, color, true);
                u++;
                d++;
                if (bp.x < tp.x && bnp.x < tp.x)
                    u--;
                if (tp.x < bp.x && tnp.x < bp.x)
                    d--;

                ltp = tp;
                lbp = bp;
                if (isBreakTop && isBreakBottom)
                    break;
            }
        }

        private static void AddVertToVertexHelperWithLerpColor(VertexHelper vh, Vector3 tp, Vector3 bp,
            Color32 color, Color32 toColor, bool isY, float min, float max, bool needTriangle)
        {
            var range = max - min;
            var color1 = Color32.Lerp(color, toColor, ((isY ? tp.x : tp.y) - min) / range);
            var color2 = Color32.Lerp(color, toColor, ((isY ? bp.x : bp.y) - min) / range);
            UGL.AddVertToVertexHelper(vh, tp, bp, color1, color2, needTriangle);
        }

        internal static void DrawSerieLine(VertexHelper vh, ThemeStyle theme, Serie serie, VisualMap visualMap,
            GridCoord grid, Axis axis, Axis relativedAxis, float lineWidth)
        {
            if (!serie.lineStyle.show || serie.lineStyle.type == LineStyle.Type.None)
                return;

            var datas = serie.context.drawPoints;

            var dataCount = datas.Count;
            if (dataCount < 2)
                return;

            var ltp = Vector3.zero;
            var lbp = Vector3.zero;
            var ntp = Vector3.zero;
            var nbp = Vector3.zero;
            var itp = Vector3.zero;
            var ibp = Vector3.zero;
            var clp = Vector3.zero;
            var crp = Vector3.zero;

            var isBreak = false;
            var isY = axis is YAxis;
            var isVisualMapGradient = VisualMapHelper.IsNeedLineGradient(visualMap);
            var isLineStyleGradient = serie.lineStyle.IsNeedGradient();

            //var highlight = serie.highlight || serie.context.pointerEnter;
            var lineColor = SerieHelper.GetLineColor(serie, null, theme, serie.context.colorIndex, false);

            var lastDataIsIgnore = datas[0].isIgnoreBreak;
            var smooth = serie.lineType == LineType.Smooth;
            for (int i = 1; i < dataCount; i++)
            {
                var cdata = datas[i];
                var isIgnore = cdata.isIgnoreBreak;
                var cp = cdata.position;
                var lp = datas[i - 1].position;

                var np = i == dataCount - 1 ? cp : datas[i + 1].position;
                if (serie.animation.CheckDetailBreak(cp, isY))
                {
                    isBreak = true;
                    var ip = Vector3.zero;
                    var progress = serie.animation.GetCurrDetail();
                    if (AnimationStyleHelper.GetAnimationPosition(serie.animation, isY, lp, cp, progress, ref ip))
                        cp = np = ip;
                }
                serie.context.lineEndPostion = cp;
                serie.context.lineEndValue = AxisHelper.GetAxisPositionValue(grid, relativedAxis, cp);

                var handled = false;
                if (!smooth)
                {
                    switch (serie.lineStyle.type)
                    {
                        case LineStyle.Type.Dashed:
                            UGL.DrawDashLine(vh, lp, cp, lineWidth, lineColor, lineColor, 0, 0);
                            handled = true;
                            break;
                        case LineStyle.Type.Dotted:
                            UGL.DrawDotLine(vh, lp, cp, lineWidth, lineColor, lineColor, 0, 0);
                            handled = true;
                            break;
                        case LineStyle.Type.DashDot:
                            UGL.DrawDashDotLine(vh, lp, cp, lineWidth, lineColor, 0, 0, 0);
                            handled = true;
                            break;
                        case LineStyle.Type.DashDotDot:
                            UGL.DrawDashDotDotLine(vh, lp, cp, lineWidth, lineColor, 0, 0, 0);
                            handled = true;
                            break;
                        case LineStyle.Type.None:
                            handled = true;
                            break;
                    }
                }
                if (handled)
                {
                    lastDataIsIgnore = isIgnore;
                    if (isBreak)
                        break;
                    else
                        continue;
                }
                bool bitp = true, bibp = true;
                UGLHelper.GetLinePoints(lp, cp, np, lineWidth,
                    ref ltp, ref lbp,
                    ref ntp, ref nbp,
                    ref itp, ref ibp,
                    ref clp, ref crp,
                    ref bitp, ref bibp, i);
                if (i == 1)
                {
                    AddLineVertToVertexHelper(vh, ltp, lbp, lineColor, isVisualMapGradient, isLineStyleGradient,
                        visualMap, serie.lineStyle, grid, axis, relativedAxis, false, lastDataIsIgnore, isIgnore);
                    if (dataCount == 2 || isBreak)
                    {
                        AddLineVertToVertexHelper(vh, clp, crp, lineColor, isVisualMapGradient, isLineStyleGradient,
                            visualMap, serie.lineStyle, grid, axis, relativedAxis, true, lastDataIsIgnore, isIgnore);
                        serie.context.lineEndPostion = cp;
                        serie.context.lineEndValue = AxisHelper.GetAxisPositionValue(grid, relativedAxis, cp);
                        break;
                    }
                }

                if (bitp == bibp)
                {
                    if (bitp)
                        AddLineVertToVertexHelper(vh, itp, ibp, lineColor, isVisualMapGradient, isLineStyleGradient,
                            visualMap, serie.lineStyle, grid, axis, relativedAxis, true, lastDataIsIgnore, isIgnore);
                    else
                    {
                        AddLineVertToVertexHelper(vh, ltp, clp, lineColor, isVisualMapGradient, isLineStyleGradient,
                            visualMap, serie.lineStyle, grid, axis, relativedAxis, true, lastDataIsIgnore, isIgnore);
                        AddLineVertToVertexHelper(vh, ltp, crp, lineColor, isVisualMapGradient, isLineStyleGradient,
                            visualMap, serie.lineStyle, grid, axis, relativedAxis, true, lastDataIsIgnore, isIgnore);
                    }
                }
                else
                {
                    if (bitp)
                    {
                        AddLineVertToVertexHelper(vh, itp, clp, lineColor, isVisualMapGradient, isLineStyleGradient,
                            visualMap, serie.lineStyle, grid, axis, relativedAxis, true, lastDataIsIgnore, isIgnore);
                        AddLineVertToVertexHelper(vh, itp, crp, lineColor, isVisualMapGradient, isLineStyleGradient,
                            visualMap, serie.lineStyle, grid, axis, relativedAxis, true, lastDataIsIgnore, isIgnore);
                    }
                    else if (bibp)
                    {
                        AddLineVertToVertexHelper(vh, clp, ibp, lineColor, isVisualMapGradient, isLineStyleGradient,
                            visualMap, serie.lineStyle, grid, axis, relativedAxis, true, lastDataIsIgnore, isIgnore);
                        AddLineVertToVertexHelper(vh, crp, ibp, lineColor, isVisualMapGradient, isLineStyleGradient,
                            visualMap, serie.lineStyle, grid, axis, relativedAxis, true, lastDataIsIgnore, isIgnore);
                    }
                }
                lastDataIsIgnore = isIgnore;
                if (isBreak)
                    break;
            }
        }

        public static float GetLineWidth(ref bool interacting, Serie serie, float defaultWidth)
        {
            var lineWidth = 0f;
            if (!serie.interact.TryGetValue(ref lineWidth, ref interacting))
            {
                lineWidth = serie.lineStyle.GetWidth(defaultWidth);
                serie.interact.SetValue(ref interacting, lineWidth);
            }
            return lineWidth;
        }

        private static void AddLineVertToVertexHelper(VertexHelper vh, Vector3 tp, Vector3 bp,
            Color32 lineColor, bool visualMapGradient, bool lineStyleGradient, VisualMap visualMap,
            LineStyle lineStyle, GridCoord grid, Axis axis, Axis relativedAxis, bool needTriangle,
            bool lastIgnore, bool ignore)
        {
            if (lastIgnore && needTriangle)
                UGL.AddVertToVertexHelper(vh, tp, bp, ColorUtil.clearColor32, true);

            if (visualMapGradient)
            {
                var color1 = VisualMapHelper.GetLineGradientColor(visualMap, tp, grid, axis, relativedAxis, lineColor);
                var color2 = VisualMapHelper.GetLineGradientColor(visualMap, bp, grid, axis, relativedAxis, lineColor);
                UGL.AddVertToVertexHelper(vh, tp, bp, color1, color2, needTriangle);
            }
            else if (lineStyleGradient)
            {
                var color1 = VisualMapHelper.GetLineStyleGradientColor(lineStyle, tp, grid, axis, lineColor);
                var color2 = VisualMapHelper.GetLineStyleGradientColor(lineStyle, bp, grid, axis, lineColor);
                UGL.AddVertToVertexHelper(vh, tp, bp, color1, color2, needTriangle);
            }
            else
            {
                UGL.AddVertToVertexHelper(vh, tp, bp, lineColor, needTriangle);
            }
            if (lastIgnore && !needTriangle)
            {
                UGL.AddVertToVertexHelper(vh, tp, bp, ColorUtil.clearColor32, false);
            }
            if (ignore && needTriangle)
            {
                UGL.AddVertToVertexHelper(vh, tp, bp, ColorUtil.clearColor32, false);
            }
        }

        internal static void UpdateSerieDrawPoints(Serie serie, Settings setting, ThemeStyle theme, VisualMap visualMap,
            float lineWidth, bool isY = false)
        {
            serie.context.drawPoints.Clear();
            var last = Vector3.zero;
            switch (serie.lineType)
            {
                case LineType.Smooth:
                    UpdateSmoothLineDrawPoints(serie, setting, isY);
                    break;
                case LineType.StepStart:
                case LineType.StepMiddle:
                case LineType.StepEnd:
                    UpdateStepLineDrawPoints(serie, setting, theme, isY, lineWidth);
                    break;
                default:
                    UpdateNormalLineDrawPoints(serie, setting, visualMap);
                    break;
            }
        }

        private static void UpdateNormalLineDrawPoints(Serie serie, Settings setting, VisualMap visualMap)
        {
            var isVisualMapGradient = VisualMapHelper.IsNeedGradient(visualMap);
            if (isVisualMapGradient)
            {
                var dataPoints = serie.context.dataPoints;
                if (dataPoints.Count > 1)
                {
                    var sp = dataPoints[0];
                    for (int i = 1; i < dataPoints.Count; i++)
                    {
                        var ep = dataPoints[i];
                        var ignore = serie.context.dataIgnores[i];

                        var dir = (ep - sp).normalized;
                        var dist = Vector3.Distance(sp, ep);
                        var segment = (int) (dist / setting.lineSegmentDistance);
                        serie.context.drawPoints.Add(new PointInfo(sp, ignore));
                        for (int j = 1; j < segment; j++)
                        {
                            var np = sp + dir * dist * j / segment;
                            serie.context.drawPoints.Add(new PointInfo(np, ignore));
                        }
                        sp = ep;
                        if (i == dataPoints.Count - 1)
                        {
                            serie.context.drawPoints.Add(new PointInfo(ep, ignore));
                        }
                    }
                }
                else
                {
                    serie.context.drawPoints.Add(new PointInfo(dataPoints[0], serie.context.dataIgnores[0]));
                }
            }
            else
            {
                for (int i = 0; i < serie.context.dataPoints.Count; i++)
                {
                    serie.context.drawPoints.Add(new PointInfo(serie.context.dataPoints[i], serie.context.dataIgnores[i]));
                }
            }
        }

        private static void UpdateSmoothLineDrawPoints(Serie serie, Settings setting, bool isY)
        {
            var points = serie.context.dataPoints;
            float smoothness = setting.lineSmoothness;
            for (int i = 0; i < points.Count - 1; i++)
            {
                var sp = points[i];
                var ep = points[i + 1];
                var lsp = i > 0 ? points[i - 1] : sp;
                var nep = i < points.Count - 2 ? points[i + 2] : ep;
                var ignore = serie.context.dataIgnores[i];
                if (isY)
                    UGLHelper.GetBezierListVertical(ref s_CurvesPosList, sp, ep, smoothness, setting.lineSmoothStyle);
                else
                    UGLHelper.GetBezierList(ref s_CurvesPosList, sp, ep, lsp, nep, smoothness, setting.lineSmoothStyle, true);
                for (int j = 1; j < s_CurvesPosList.Count; j++)
                {
                    serie.context.drawPoints.Add(new PointInfo(s_CurvesPosList[j], ignore));
                }
            }
        }

        private static void UpdateStepLineDrawPoints(Serie serie, Settings setting, ThemeStyle theme, bool isY, float lineWidth)
        {
            var points = serie.context.dataPoints;
            var lp = points[0];
            serie.context.drawPoints.Clear();
            serie.context.drawPoints.Add(new PointInfo(lp, serie.context.dataIgnores[0]));
            for (int i = 1; i < points.Count; i++)
            {
                var cp = points[i];
                var ignore = serie.context.dataIgnores[i];
                if ((isY && Mathf.Abs(lp.x - cp.x) <= lineWidth) ||
                    (!isY && Mathf.Abs(lp.y - cp.y) <= lineWidth))
                {
                    serie.context.drawPoints.Add(new PointInfo(cp, ignore));
                    lp = cp;
                    continue;
                }
                switch (serie.lineType)
                {
                    case LineType.StepStart:
                        serie.context.drawPoints.Add(new PointInfo(isY ?
                            new Vector3(cp.x, lp.y) :
                            new Vector3(lp.x, cp.y), ignore));
                        break;
                    case LineType.StepMiddle:
                        serie.context.drawPoints.Add(new PointInfo(isY ?
                            new Vector3(lp.x, (lp.y + cp.y) / 2) :
                            new Vector3((lp.x + cp.x) / 2, lp.y), ignore));
                        serie.context.drawPoints.Add(new PointInfo(isY ?
                            new Vector3(cp.x, (lp.y + cp.y) / 2) :
                            new Vector3((lp.x + cp.x) / 2, cp.y), ignore));
                        break;
                    case LineType.StepEnd:
                        serie.context.drawPoints.Add(new PointInfo(isY ?
                            new Vector3(lp.x, cp.y) :
                            new Vector3(cp.x, lp.y), ignore));
                        break;
                }
                serie.context.drawPoints.Add(new PointInfo(cp, ignore));
                lp = cp;
            }
        }
    }
}