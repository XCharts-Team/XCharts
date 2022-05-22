using UnityEngine;

namespace XCharts.Runtime
{
    internal static class SerieDataPool
    {
        private static readonly ObjectPool<SerieData> s_ListPool = new ObjectPool<SerieData>(null, OnClear);

        static void OnGet(SerieData serieData) { }

        static void OnClear(SerieData serieData)
        {
            serieData.Reset();
        }

        public static SerieData Get()
        {
            return s_ListPool.Get();
        }

        public static void Release(SerieData toRelease)
        {
            s_ListPool.Release(toRelease);
        }
    }
}