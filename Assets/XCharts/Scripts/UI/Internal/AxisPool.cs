
using System.Collections.Generic;

namespace XCharts
{
    public static class XAxisPool
    {
        private static readonly ObjectPool<XAxis> s_ListPool = new ObjectPool<XAxis>(null, null);

        public static XAxis Get()
        {
            return s_ListPool.Get();
        }

        public static void Release(XAxis toRelease)
        {
            s_ListPool.Release(toRelease);
        }
    }

    public static class YAxisPool
    {
        private static readonly ObjectPool<YAxis> s_ListPool = new ObjectPool<YAxis>(null, null);

        public static YAxis Get()
        {
            return s_ListPool.Get();
        }

        public static void Release(YAxis toRelease)
        {
            s_ListPool.Release(toRelease);
        }
    }
}
