using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace XUGL
{
    public class SVGPath
    {
        private static Regex s_PathRegex = new Regex(@"(([a-z]|[A-Z])(\d|\.|,|-)*)");
        private static Regex s_PathValueRegex = new Regex(@"(^[a-z]|[A-Z])\s*(-?\d+\.*\d*)*[\s|,|-]*(\d+\.*\d*)*");
        private static Regex s_PathValueRegex2 = new Regex(@"(-?\d+\.?\d*)");
        public bool mirrorY = true;
        public List<SVGPathSeg> segs = new List<SVGPathSeg>();

        public void AddSegment(SVGPathSeg seg)
        {
            segs.Add(seg);
        }

        public static SVGPath Parse(string path)
        {
            if (string.IsNullOrEmpty(path))
                return new SVGPath();
            if (path.StartsWith("path://"))
            {
                path = path.Substring(7);
            }
            path = path.Replace(' ', ',');
            var mc = s_PathRegex.Matches(path);
            var svgPath = new SVGPath();

            foreach (var m in mc)
            {
                var key = m.ToString();
                if (key.Equals("Z") || key.Equals("z"))
                {
                    var seg = new SVGPathSeg(SVGPathSegType.Z);
                    seg.raw = key;
                    seg.relative = key.Equals("z");
                    svgPath.AddSegment(seg);
                }
                else
                {
                    var type = s_PathValueRegex.Match(key).Groups[1].ToString().ToCharArray() [0];
                    var mc3 = s_PathValueRegex2.Matches(key);
                    SVGPathSeg seg = null;
                    switch (type)
                    {
                        case 'M':
                        case 'm':
                            seg = new SVGPathSeg(SVGPathSegType.M);
                            seg.relative = type == 'm';
                            break;
                        case 'L':
                        case 'l':
                            seg = new SVGPathSeg(SVGPathSegType.L);
                            seg.relative = type == 'l';
                            break;
                        case 'H':
                        case 'h':
                            seg = new SVGPathSeg(SVGPathSegType.H);
                            seg.relative = type == 'h';
                            break;
                        case 'V':
                        case 'v':
                            seg = new SVGPathSeg(SVGPathSegType.V);
                            seg.relative = type == 'v';
                            break;
                        case 'C':
                        case 'c':
                            seg = new SVGPathSeg(SVGPathSegType.C);
                            seg.relative = type == 'c';
                            break;
                        case 'S':
                        case 's':
                            seg = new SVGPathSeg(SVGPathSegType.S);
                            seg.relative = type == 's';
                            break;
                        case 'Q':
                        case 'q':
                            seg = new SVGPathSeg(SVGPathSegType.Q);
                            seg.relative = type == 'q';
                            break;
                        case 'T':
                        case 't':
                            seg = new SVGPathSeg(SVGPathSegType.T);
                            seg.relative = type == 't';
                            break;
                        case 'A':
                        case 'a':
                            seg = new SVGPathSeg(SVGPathSegType.A);
                            seg.relative = type == 'a';
                            break;
                    }
                    if (seg != null)
                    {
                        seg.raw = key;
                        foreach (var m3 in mc3)
                        {
                            // if (type == 'c' || type == 'C')
                            //Debug.LogError("\tmc3:" + type + "," + m3.ToString());
                            float p;
                            if (float.TryParse(m3.ToString(), out p))
                                seg.parameters.Add(p);
                        }
                        svgPath.AddSegment(seg);
                    }
                }
            }
            // Debug.LogError(path);
            // foreach (var cmd in svgPath.commands)
            // {
            //     Debug.LogError(cmd.raw);
            // }
            return svgPath;
        }

        public void Draw(VertexHelper vh)
        {
            var sp = Vector2.zero;
            var np = Vector2.zero;
            var posList = new List<Vector3>();
            var bezierList = new List<Vector3>();
            var cp2 = Vector2.zero;
            foreach (var seg in segs)
            {
                switch (seg.type)
                {
                    case SVGPathSegType.M:
                        sp = np = seg.relative ? np + seg.p1 : seg.p1;
                        if (posList.Count > 0)
                        {
                            DrawPosList(vh, posList);
                        }
                        posList.Add(np);
                        break;
                    case SVGPathSegType.L:
                        np = seg.relative ? np + seg.p1 : seg.p1;
                        posList.Add(np);
                        break;
                    case SVGPathSegType.H:
                        np = seg.relative ? np + new Vector2(seg.value, 0) : new Vector2(seg.value, np.y);
                        posList.Add(np);
                        break;
                    case SVGPathSegType.V:
                        np = seg.relative ? np + new Vector2(0, seg.value) : new Vector2(np.x, seg.value);
                        posList.Add(np);
                        break;
                    case SVGPathSegType.C:
                        var cp1 = seg.relative ? np + seg.p1 : seg.p1;
                        cp2 = seg.relative ? np + seg.p2 : seg.p2;
                        var ep = seg.relative ? np + seg.p3 : seg.p3;
                        var dist = (int) Vector2.Distance(np, ep) * 2;
                        if (dist < 2) dist = 2;
                        UGLHelper.GetBezierList2(ref bezierList, np, ep, dist, cp1, cp2);
                        for (int n = 1; n < bezierList.Count; n++)
                            posList.Add(bezierList[n]);
                        np = ep;
                        break;
                    case SVGPathSegType.S:
                        cp1 = np + (np - cp2).normalized * Vector2.Distance(np, cp2);
                        var scp2 = seg.relative ? np + seg.p1 : seg.p1;
                        ep = seg.relative ? np + seg.p2 : seg.p2;
                        dist = (int) Vector2.Distance(np, ep) * 2;
                        if (dist < 2) dist = 2;
                        UGLHelper.GetBezierList2(ref bezierList, np, ep, dist, cp1, scp2);
                        for (int n = 1; n < bezierList.Count; n++)
                            posList.Add(bezierList[n]);
                        break;
                    case SVGPathSegType.Z:
                        posList.Add(sp);
                        DrawPosList(vh, posList);
                        break;
                    case SVGPathSegType.Q:
                    case SVGPathSegType.T:
                    case SVGPathSegType.A:
                    default:
                        Debug.LogError("unknow seg:" + seg.type);
                        break;
                }
            }
            if (posList.Count > 0)
                DrawPosList(vh, posList);
            //UGL.DrawCricle(vh, sp, 1, Color.black);
        }

        private void DrawPosList(VertexHelper vh, List<Vector3> posList)
        {
            if (mirrorY)
            {
                for (int i = posList.Count - 1; i >= 0; i--)
                {
                    var pos = posList[i];
                    posList[i] = new Vector3(pos.x, -pos.y);
                }
            }
            UGL.DrawLine(vh, posList, 1f, Color.red, false);
            posList.Clear();
        }
    }
}