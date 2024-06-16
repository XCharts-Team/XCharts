using UnityEngine;
using XUGL;

namespace XCharts.Runtime
{
    public static class AnimationStyleHelper
    {
        public static float CheckDataAnimation(BaseChart chart, Serie serie, int dataIndex, float destProgress, float startPorgress = 0)
        {
            if (!serie.animation.IsDataAnimation())
            {
                serie.animation.context.isAllItemAnimationEnd = false;
                return destProgress;
            }
            if (serie.animation.IsFinish())
            {
                serie.animation.context.isAllItemAnimationEnd = false;
                return destProgress;
            }
            var isDataAnimationEnd = true;
            var currHig = serie.animation.CheckItemProgress(dataIndex, destProgress, ref isDataAnimationEnd, startPorgress);
            if (!isDataAnimationEnd)
            {
                serie.animation.context.isAllItemAnimationEnd = false;
            }
            return currHig;
        }

        public static void UpdateSerieAnimation(Serie serie)
        {
            var serieType = serie.GetType();
            var animationType = AnimationType.LeftToRight;
            var enableSerieDataAnimation = true;
            if (serieType.IsDefined(typeof(DefaultAnimationAttribute), false))
            {
                var attribute = serieType.GetAttribute<DefaultAnimationAttribute>();
                animationType = attribute.type;
                enableSerieDataAnimation = attribute.enableSerieDataAddedAnimation;
            }
            UpdateAnimationType(serie.animation, animationType, enableSerieDataAnimation);
        }

        public static void UpdateAnimationType(AnimationStyle animation, AnimationType defaultType, bool enableSerieDataAnimation)
        {
            animation.context.type = animation.type == AnimationType.Default ?
                defaultType :
                animation.type;
            animation.context.enableSerieDataAddedAnimation = enableSerieDataAnimation;
        }

        public static bool GetAnimationPosition(AnimationStyle animation, bool isY, Vector3 lp, Vector3 cp, float progress, ref Vector3 ip, ref float rate)
        {
            if (animation.context.type == AnimationType.AlongPath)
            {
                var dist = Vector3.Distance(lp, cp);
                rate = (dist - animation.context.currentPathDistance + animation.GetCurrDetail()) / dist;
                ip = Vector3.Lerp(lp, cp, rate);
                return true;
            }
            else
            {
                var startPos = isY ? new Vector3(-10000, progress) : new Vector3(progress, -10000);
                var endPos = isY ? new Vector3(10000, progress) : new Vector3(progress, 10000);

                if (UGLHelper.GetIntersection(lp, cp, startPos, endPos, ref ip))
                {
                    rate = Vector3.Distance(lp, ip) / Vector3.Distance(lp, cp);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}