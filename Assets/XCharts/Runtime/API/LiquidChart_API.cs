/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    public partial class BaseChart
    {
        public Vessel vessel { get { return m_Vessels.Count > 0 ? m_Vessels[0] : null; } }
        /// <summary>
        /// 容器组件列表。
        /// </summary>
        public List<Vessel> vessels { get { return m_Vessels; } }

        /// <summary>
        /// 移除所有容器组件。
        /// </summary>
        public void RemoveVessel()
        {
            m_Vessels.Clear();
        }

        /// <summary>
        /// 添加容器组件。
        /// </summary>
        public void AddVessel(Vessel vessel)
        {
            m_Vessels.Add(vessel);
        }

        /// <summary>
        /// 添加容器组件。
        /// </summary>
        public Vessel AddVessel(Vessel.Shape shape, Vector2 center, float radius)
        {
            var vessel = new Vessel();
            vessel.shape = shape;
            vessel.radius = radius;
            vessel.center[0] = center.x;
            vessel.center[1] = center.y;
            AddVessel(vessel);
            return vessel;
        }

        /// <summary>
        /// 获得指定索引的容器组件。
        /// </summary>
        /// <param name="radarIndex"></param>
        /// <returns></returns>
        public Vessel GetVessel(int vesselIndex)
        {
            if (vesselIndex < 0 || vesselIndex > m_Vessels.Count - 1) return null;
            return m_Vessels[vesselIndex];
        }
    }
}