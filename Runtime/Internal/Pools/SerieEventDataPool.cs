using UnityEngine;

namespace XCharts.Runtime
{
    public static class SerieEventDataPool
    {
        private static readonly ObjectPool<SerieEventData> s_ListPool = new ObjectPool<SerieEventData>(null, OnClear);

        static void OnGet(SerieEventData data)
        {
        }

        static void OnClear(SerieEventData data)
        {
            data.Reset();
        }

        public static SerieEventData Get(Vector3 pos, int serieIndex, int dataIndex, int dimension, double value)
        {
            var data = s_ListPool.Get();
            data.serieIndex = serieIndex;
            data.dataIndex = dataIndex;
            data.pointerPos = pos;
            data.dimension = dimension;
            data.value = value;
            return data;
        }

        public static void Release(SerieEventData toRelease)
        {
            s_ListPool.Release(toRelease);
        }
    }
}