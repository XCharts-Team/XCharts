using System.Collections.Generic;
using UnityEngine;

namespace XUGL
{
    public static class UGLHelper
    {
        public static bool IsValueEqualsColor(Color32 color1, Color32 color2)
        {
            return color1.a == color2.a &&
                color1.b == color2.b &&
                color1.g == color2.g &&
                color1.r == color2.r;
        }

        public static bool IsValueEqualsColor(Color color1, Color color2)
        {
            return color1.a == color2.a &&
                color1.b == color2.b &&
                color1.g == color2.g &&
                color1.r == color2.r;
        }

        public static bool IsValueEqualsString(string str1, string str2)
        {
            if (str1 == null && str2 == null)
                return true;
            else if (str1 != null && str2 != null)
                return str1.Equals(str2);
            else return false;
        }

        public static bool IsValueEqualsVector2(Vector2 v1, Vector2 v2)
        {
            return v1.x == v2.x &&
                v1.y == v2.y;
        }

        public static bool IsValueEqualsVector3(Vector3 v1, Vector3 v2)
        {
            return v1.x == v2.x &&
                v1.y == v2.y &&
                v1.z == v2.z;
        }

        public static bool IsValueEqualsVector3(Vector3 v1, Vector2 v2)
        {
            return v1.x == v2.x &&
                v1.y == v2.y;
        }

        public static bool IsValueEqualsList<T>(List<T> list1, List<T> list2)
        {
            if (list1 == null || list2 == null)
                return false;

            if (list1.Count != list2.Count)
                return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] == null && list2[i] == null) { }
                else
                {
                    if (list1[i] != null)
                    {
                        if (!list1[i].Equals(list2[i]))
                            return false;
                    }
                    else
                    {
                        if (!list2[i].Equals(list1[i]))
                            return false;
                    }
                }
            }
            return true;
        }

        public static bool IsClearColor(Color32 color)
        {
            return color.a == 0 &&
                color.b == 0 &&
                color.g == 0 &&
                color.r == 0;
        }

        public static bool IsClearColor(Color color)
        {
            return color.a == 0 &&
                color.b == 0 &&
                color.g == 0 &&
                color.r == 0;
        }

        public static bool IsZeroVector(Vector3 pos)
        {
            return pos.x == 0 &&
                pos.y == 0 &&
                pos.z == 0;
        }

        public static Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)
        {
            Vector3 point = Quaternion.AngleAxis(angle, axis) * (position - center);
            Vector3 resultVec3 = center + point;
            return resultVec3;
        }

        public static void GetBezierList(ref List<Vector3> posList, Vector3 sp, Vector3 ep,
            Vector3 lsp, Vector3 nep, float smoothness = 2f, float k = 2.0f, bool limit = false, bool randomDire = false)
        {
            Vector3 cp1, cp2;
            var dist = Vector3.Distance(sp, ep);
            var dir = (ep - sp).normalized;
            var diff = (randomDire ? dist : Mathf.Abs(sp.x - ep.x)) / k;
            if (lsp == sp)
            {
                cp1 = sp + (nep - ep).normalized * diff;
                if (limit) cp1.y = sp.y;
            }
            else
            {
                cp1 = sp + (ep - lsp).normalized * diff;
                if (limit) cp1.y = sp.y;
            }
            if (nep == ep)
            {
                cp2 = ep;
            }
            else
            {
                cp2 = ep - (nep - sp).normalized * diff;
                if (limit) cp2.y = ep.y;
            }
            int segment = (int) (dist / (smoothness <= 0 ? 2f : smoothness));
            if (segment < 1) segment = (int) (dist / 0.5f);
            if (segment < 4) segment = 4;
            GetBezierList2(ref posList, sp, ep, segment, cp1, cp2);
            if (posList.Count < 2)
            {
                posList.Clear();
                posList.Add(sp);
                posList.Add(ep);
            }
        }

        public static void GetBezierListVertical(ref List<Vector3> posList, Vector3 sp, Vector3 ep,
            float smoothness = 2f, float k = 2.0f)
        {
            Vector3 dir = (ep - sp).normalized;
            float dist = Vector3.Distance(sp, ep);
            Vector3 cp1 = sp + dist / k * dir * 1;
            Vector3 cp2 = sp + dist / k * dir * (k - 1);
            cp1.x = sp.x;
            cp2.x = ep.x;
            int segment = (int) (dist / (smoothness <= 0 ? 2f : smoothness));
            GetBezierList2(ref posList, sp, ep, segment, cp1, cp2);
            if (posList.Count < 2)
            {
                posList.Clear();
                posList.Add(sp);
                posList.Add(ep);
            }
        }

        public static List<Vector3> GetBezierList(Vector3 sp, Vector3 ep, int segment, Vector3 cp)
        {
            List<Vector3> list = new List<Vector3>();
            for (int i = 0; i < segment; i++)
            {
                list.Add(GetBezier(i / (float) segment, sp, cp, ep));
            }
            list.Add(ep);
            return list;
        }

        public static void GetBezierList2(ref List<Vector3> posList, Vector3 sp, Vector3 ep,
            int segment, Vector3 cp, Vector3 cp2)
        {
            posList.Clear();
            if (posList.Capacity < segment + 1)
            {
                posList.Capacity = segment + 1;
            }
            for (int i = 0; i < segment; i++)
            {
                posList.Add((GetBezier2(i / (float) segment, sp, cp, cp2, ep)));
            }
            posList.Add(ep);
        }

        public static Vector3 GetBezier(float t, Vector3 sp, Vector3 cp, Vector3 ep)
        {
            Vector3 aa = sp + (cp - sp) * t;
            Vector3 bb = cp + (ep - cp) * t;
            return aa + (bb - aa) * t;
        }

        public static Vector3 GetBezier2(float t, Vector3 sp, Vector3 p1, Vector3 p2, Vector3 ep)
        {
            t = Mathf.Clamp01(t);
            var oneMinusT = 1f - t;
            return oneMinusT * oneMinusT * oneMinusT * sp +
                3f * oneMinusT * oneMinusT * t * p1 +
                3f * oneMinusT * t * t * p2 +
                t * t * t * ep;
        }

        public static Vector3 GetDire(float angle, bool isDegree = false)
        {
            angle = isDegree ? angle * Mathf.Deg2Rad : angle;
            return new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));
        }

        public static Vector3 GetVertialDire(Vector3 dire)
        {
            if (dire.x == 0)
                return new Vector3(-1, 0, 0);

            if (dire.y == 0)
                return new Vector3(0, -1, 0);
            else
                return new Vector3(-dire.y / dire.x, 1, 0).normalized;
        }

        /// <summary>
        /// 获得0-360的角度（12点钟方向为0度）
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float GetAngle360(Vector2 from, Vector2 to)
        {
            float angle;

            Vector3 cross = Vector3.Cross(from, to);
            angle = Vector2.Angle(from, to);
            angle = cross.z > 0 ? -angle : angle;
            angle = (angle + 360) % 360;
            return angle;
        }

        public static Vector3 GetPos(Vector3 center, float radius, float angle, bool isDegree = false)
        {
            angle = isDegree ? angle * Mathf.Deg2Rad : angle;
            return new Vector3(center.x + radius * Mathf.Sin(angle),
                center.y + radius * Mathf.Cos(angle));
        }

        /// <summary>
        /// 获得两直线的交点
        /// </summary>
        /// <param name="p1">线段1起点</param>
        /// <param name="p2">线段1终点</param>
        /// <param name="p3">线段2起点</param>
        /// <param name="p4">线段2终点</param>
        /// <param name="intersection">相交点。当不想交时默认为 Vector3.zero </param>
        /// <returns>相交则返回 true, 否则返回 false</returns>
        public static bool GetIntersection(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, ref Vector3 intersection)
        {
            intersection = Vector3.zero;

            var d = (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);
            if (d == 0)
                return false;

            var u = ((p3.x - p1.x) * (p4.y - p3.y) - (p3.y - p1.y) * (p4.x - p3.x)) / d;
            var v = ((p3.x - p1.x) * (p2.y - p1.y) - (p3.y - p1.y) * (p2.x - p1.x)) / d;
            if (u < 0 || u > 1 || v < 0 || v > 1)
                return false;

            intersection.x = p1.x + u * (p2.x - p1.x);
            intersection.y = p1.y + u * (p2.y - p1.y);
            return true;
        }

        /// <summary>
        /// 三个点画线段所需要的六个关键点
        /// </summary>
        /// <param name="lp">上一个点</param>
        /// <param name="cp">当前点</param>
        /// <param name="np">下一个点</param>
        /// <param name="width">线段宽度</param>
        /// <param name="ltp">上一个点的上角点</param>
        /// <param name="lbp">上一个点的下角点</param>
        /// <param name="ntp">下一个点的上角点</param>
        /// <param name="nbp">下一个点的下角点</param>
        /// <param name="itp">交汇点的上角点</param>
        /// <param name="ibp">交汇点的下角点</param>
        internal static void GetLinePoints(Vector3 lp, Vector3 cp, Vector3 np, float width,
            ref Vector3 ltp, ref Vector3 lbp,
            ref Vector3 ntp, ref Vector3 nbp,
            ref Vector3 itp, ref Vector3 ibp,
            ref Vector3 clp, ref Vector3 crp,
            ref bool bitp, ref bool bibp, int debugIndex = 0)
        {
            var dir1 = (cp - lp).normalized;
            var dir1v = Vector3.Cross(dir1, Vector3.forward).normalized * width;
            ltp = lp - dir1v;
            lbp = lp + dir1v;
            if (debugIndex == 1 && cp == np)
            {
                ntp = np - dir1v;
                nbp = np + dir1v;
                clp = cp - dir1v;
                crp = cp + dir1v;
                return;
            }

            var dir2 = (cp - np).normalized;
            var dir2v = Vector3.Cross(dir2, Vector3.back).normalized * width;
            ntp = np - dir2v;
            nbp = np + dir2v;
            clp = cp - dir2v;
            crp = cp + dir2v;

            if (Vector3.Cross(dir1, dir2) == Vector3.zero && np != cp)
            {
                itp = ntp;
                ibp = nbp;
                return;
            }

            var ldist = (Vector3.Distance(cp, lp) + 1) * dir1;
            var rdist = (Vector3.Distance(cp, np) + 1) * dir2;

            bitp = true;
            if (!UGLHelper.GetIntersection(ltp, ltp + ldist, ntp, ntp + rdist, ref itp))
            {
                itp = cp - dir1v;
                clp = cp - dir1v;
                crp = cp - dir2v;
                bitp = false;
            }
            bibp = true;
            if (!UGLHelper.GetIntersection(lbp, lbp + ldist, nbp, nbp + rdist, ref ibp))
            {
                ibp = cp + dir1v;
                clp = cp + dir1v;
                crp = cp + dir2v;
                bibp = false;
            }
            if (bitp == false && bibp == false && cp == np)
            {
                ltp = cp - dir1v;
                clp = cp + dir1v;
                crp = cp + dir1v;
            }
        }

        public static bool IsPointInTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 check)
        {
            var dire1 = check - p1;
            var dire2 = check - p2;
            var dire3 = check - p3;
            var c1 = dire1.x * dire2.y - dire1.y * dire2.x;
            var c2 = dire2.x * dire3.y - dire2.y * dire3.x;
            var c3 = dire3.x * dire1.y - dire3.y * dire1.x;
            return c1 * c2 >= 0 && c1 * c3 >= 0;
        }

        public static bool IsPointInPolygon(Vector3 p, List<Vector3> polyons)
        {
            if (polyons.Count == 0) return false;
            var inside = false;
            var j = polyons.Count - 1;
            for (int i = 0; i < polyons.Count; j = i++)
            {
                var pi = polyons[i];
                var pj = polyons[j];
                if (((pi.y <= p.y && p.y < pj.y) || (pj.y <= p.y && p.y < pi.y)) &&
                    (p.x < (pj.x - pi.x) * (p.y - pi.y) / (pj.y - pi.y) + pi.x))
                    inside = !inside;
            }
            return inside;
        }
        public static bool IsPointInPolygon(Vector3 p, List<Vector2> polyons)
        {
            if (polyons.Count == 0) return false;
            var inside = false;
            var j = polyons.Count - 1;
            for (int i = 0; i < polyons.Count; j = i++)
            {
                var pi = polyons[i];
                var pj = polyons[j];
                if (((pi.y <= p.y && p.y < pj.y) || (pj.y <= p.y && p.y < pi.y)) &&
                    (p.x < (pj.x - pi.x) * (p.y - pi.y) / (pj.y - pi.y) + pi.x))
                    inside = !inside;
            }
            return inside;
        }
    }
}