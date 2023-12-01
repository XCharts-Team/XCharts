using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public partial class BaseChart
    {
        public bool TryAddChartComponent<T>() where T : MainComponent
        {
            return TryAddChartComponent(typeof(T));
        }

        public bool TryAddChartComponent(Type type)
        {
            if (CanAddChartComponent(type))
            {
                AddChartComponent(type);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryAddChartComponent<T>(out T component) where T : MainComponent
        {
            var type = typeof(T);
            if (CanAddChartComponent(type))
            {
                component = AddChartComponent(type) as T;
                return true;
            }
            else
            {
                component = null;
                return false;
            }
        }

        public T AddChartComponent<T>() where T : MainComponent
        {
            return (T)AddChartComponent(typeof(T));
        }

        public T AddChartComponentWhenNoExist<T>() where T : MainComponent
        {
            if (HasChartComponent<T>()) return null;
            return AddChartComponent<T>();
        }

        public MainComponent AddChartComponent(Type type)
        {
            if (!CanAddChartComponent(type))
            {
                Debug.LogError("XCharts ERROR: CanAddChartComponent:" + type.Name);
                return null;
            }
            CheckAddRequireChartComponent(type);
            var component = Activator.CreateInstance(type) as MainComponent;
            if (component == null)
            {
                Debug.LogError("XCharts ERROR: CanAddChartComponent:" + type.Name);
                return null;
            }
            component.SetDefaultValue();
            if (component is IUpdateRuntimeData)
                (component as IUpdateRuntimeData).UpdateRuntimeData(this);
            AddComponent(component);
            m_Components.Sort();
            CreateComponentHandler(component);
#if UNITY_EDITOR && UNITY_2019_1_OR_NEWER
            UnityEditor.EditorUtility.SetDirty(this);
            OnBeforeSerialize();
#endif
            return component;
        }

        private void AddComponent(MainComponent component)
        {
            var type = component.GetType();
            m_Components.Add(component);
            List<MainComponent> list;
            if (!m_ComponentMaps.TryGetValue(type, out list))
            {
                list = new List<MainComponent>();
                m_ComponentMaps[type] = list;
            }
            component.index = list.Count;
            list.Add(component);
            m_Components.Sort((a, b) => { return a.GetType().Name.CompareTo(b.GetType().Name); });
        }

        private void CheckAddRequireChartComponent(Type type)
        {
            if (Attribute.IsDefined(type, typeof(RequireChartComponentAttribute)))
            {
                foreach (var obj in type.GetCustomAttributes(typeof(RequireChartComponentAttribute), false))
                {
                    var attribute = obj as RequireChartComponentAttribute;
                    if (attribute.type0 != null && !HasChartComponent(attribute.type0))
                        AddChartComponent(attribute.type0);
                    if (attribute.type1 != null && !HasChartComponent(attribute.type1))
                        AddChartComponent(attribute.type1);
                    if (attribute.type2 != null && !HasChartComponent(attribute.type2))
                        AddChartComponent(attribute.type2);
                }
            }
        }

        private void CreateComponentHandler(MainComponent component)
        {
            if (!component.GetType().IsDefined(typeof(ComponentHandlerAttribute), false))
            {
                Debug.LogError("MainComponent no Handler:" + component.GetType());
                return;
            }
            var attrubte = component.GetType().GetAttribute<ComponentHandlerAttribute>();
            if (attrubte.handler == null)
                return;

            var handler = (MainComponentHandler)Activator.CreateInstance(attrubte.handler);
            handler.attribute = attrubte;
            handler.chart = this;
            handler.SetComponent(component);
            component.handler = handler;
            m_ComponentHandlers.Add(handler);
        }

        public bool RemoveChartComponent<T>(int index = 0)
        where T : MainComponent
        {
            return RemoveChartComponent(typeof(T), index);
        }

        public int RemoveChartComponents<T>()
        where T : MainComponent
        {
            return RemoveChartComponents(typeof(T));
        }

        public void RemoveAllChartComponent()
        {
            m_Components.Clear();
            InitComponentHandlers();
        }

        public bool RemoveChartComponent(Type type, int index = 0)
        {
            MainComponent toRemove = null;
            for (int i = 0; i < m_Components.Count; i++)
            {
                if (m_Components[i].GetType() == type && m_Components[i].index == index)
                {
                    toRemove = m_Components[i];
                    break;
                }
            }
            return RemoveChartComponent(toRemove);
        }

        public int RemoveChartComponents(Type type)
        {
            int count = 0;
            for (int i = m_Components.Count - 1; i > 0; i--)
            {
                if (m_Components[i].GetType() == type)
                {
                    RemoveChartComponent(m_Components[i]);
                    count++;
                }
            }
            return count;
        }

        public bool RemoveChartComponent(MainComponent component)
        {
            if (component == null) return false;
            if (m_Components.Remove(component))
            {
                if (component.gameObject != null)
                    ChartHelper.SetActive(component.gameObject, false);
#if UNITY_EDITOR && UNITY_2019_1_OR_NEWER
                UnityEditor.EditorUtility.SetDirty(this);
                OnBeforeSerialize();
#endif
                InitComponentHandlers();
                RefreshChart();
                return true;
            }
            return false;
        }

        public bool CanAddChartComponent(Type type)
        {
            if (!type.IsSubclassOf(typeof(MainComponent))) return false;
            if (!m_TypeListForComponent.ContainsKey(type)) return false;
            if (CanMultipleComponent(type)) return !HasChartComponent(type);
            else return true;
        }

        public bool HasChartComponent<T>()
        where T : MainComponent
        {
            return HasChartComponent(typeof(T));
        }

        public bool HasChartComponent(Type type)
        {
            foreach (var component in m_Components)
            {
                if (component == null) continue;
                if (component.GetType() == type)
                    return true;
            }
            return false;
        }

        public bool CanMultipleComponent(Type type)
        {
            return Attribute.IsDefined(type, typeof(DisallowMultipleComponent));
        }

        public int GetChartComponentNum<T>() where T : MainComponent
        {
            return GetChartComponentNum(typeof(T));
        }

        private static List<MainComponent> list;
        public int GetChartComponentNum(Type type)
        {
            if (m_ComponentMaps.TryGetValue(type, out list))
                return list.Count;
            else
                return 0;
        }

        public T GetChartComponent<T>(int index = 0) where T : MainComponent
        {
            foreach (var component in m_Components)
            {
                if (component is T && component.index == index)
                    return component as T;
            }
            return null;
        }

        public List<MainComponent> GetChartComponents<T>() where T : MainComponent
        {
            var type = typeof(T);
            if (m_ComponentMaps.ContainsKey(type))
                return m_ComponentMaps[type];
            else
                return null;
        }

        [Obsolete("'GetOrAddChartComponent' is obsolete, Use 'EnsureChartComponent' instead.")]
        public T GetOrAddChartComponent<T>() where T : MainComponent
        {
            var component = GetChartComponent<T>();
            if (component == null)
                return AddChartComponent<T>();
            else
                return component;
        }

        /// <summary>
        /// Ensure the chart has the component, if not, add it. 
        /// Note: it may fail to add.
        /// ||确保图表有该组件，如果没有则添加。注意：有可能添加不成功。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>component, or null if add failed.</returns>
        [Since("v3.6.0")]
        public T EnsureChartComponent<T>() where T : MainComponent
        {
            var component = GetChartComponent<T>();
            if (component == null)
                return AddChartComponent<T>();
            else
                return component;
        }

        public bool TryGetChartComponent<T>(out T component, int index = 0)
        where T : MainComponent
        {
            component = null;
            foreach (var com in m_Components)
            {
                if (com is T && com.index == index)
                {
                    component = (T)com;
                    return true;
                }
            }
            return false;
        }
        public GridCoord GetGrid(Vector2 local)
        {
            List<MainComponent> list;
            if (m_ComponentMaps.TryGetValue(typeof(GridCoord), out list))
            {
                foreach (var component in list)
                {
                    var grid = component as GridCoord;
                    if (grid.Contains(local)) return grid;
                }
            }
            return null;
        }

        public GridCoord GetGridOfDataZoom(DataZoom dataZoom)
        {
            GridCoord grid = null;
            if (dataZoom.xAxisIndexs != null && dataZoom.xAxisIndexs.Count > 0)
            {
                var xAxis = GetChartComponent<XAxis>(dataZoom.xAxisIndexs[0]);
                grid = GetChartComponent<GridCoord>(xAxis.gridIndex);
            }
            else if (dataZoom.yAxisIndexs != null && dataZoom.yAxisIndexs.Count > 0)
            {
                var yAxis = GetChartComponent<YAxis>(dataZoom.yAxisIndexs[0]);
                grid = GetChartComponent<GridCoord>(yAxis.gridIndex);
            }
            if (grid == null) return GetChartComponent<GridCoord>();
            else return grid;
        }

        public DataZoom GetDataZoomOfAxis(Axis axis)
        {
            foreach (var component in m_Components)
            {
                if (component is DataZoom)
                {
                    var dataZoom = component as DataZoom;
                    if (!dataZoom.enable) continue;
                    if (dataZoom.IsContainsAxis(axis)) return dataZoom;
                }
            }
            return null;
        }

        public VisualMap GetVisualMapOfSerie(Serie serie)
        {
            foreach (var component in m_Components)
            {
                if (component is VisualMap)
                {
                    var visualMap = component as VisualMap;
                    if (visualMap.serieIndex == serie.index) return visualMap;
                }
            }
            return null;
        }

        public void GetDataZoomOfSerie(Serie serie, out DataZoom xDataZoom, out DataZoom yDataZoom)
        {
            xDataZoom = null;
            yDataZoom = null;
            if (serie == null) return;
            foreach (var component in m_Components)
            {
                if (component is DataZoom)
                {
                    var dataZoom = component as DataZoom;
                    if (!dataZoom.enable) continue;
                    if (dataZoom.IsContainsXAxis(serie.xAxisIndex))
                    {
                        xDataZoom = dataZoom;
                    }
                    if (dataZoom.IsContainsYAxis(serie.yAxisIndex))
                    {
                        yDataZoom = dataZoom;
                    }
                }
            }
        }

        public DataZoom GetXDataZoomOfSerie(Serie serie)
        {
            if (serie == null) return null;
            foreach (var component in m_Components)
            {
                if (component is DataZoom)
                {
                    var dataZoom = component as DataZoom;
                    if (!dataZoom.enable) continue;
                    if (dataZoom.IsContainsXAxis(serie.xAxisIndex))
                        return dataZoom;
                }
            }
            return null;
        }

        /// <summary>
        /// reutrn true when all the show axis is `Value` type.
        /// ||纯数值坐标轴（数值轴或对数轴）。
        /// </summary>
        public bool IsAllAxisValue()
        {
            foreach (var component in m_Components)
            {
                if (component is Axis)
                {
                    var axis = component as Axis;
                    if (axis.show && !axis.IsValue() && !axis.IsLog() && !axis.IsTime()) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 纯类目轴。
        /// </summary>
        public bool IsAllAxisCategory()
        {
            foreach (var component in m_Components)
            {
                if (component is Axis)
                {
                    var axis = component as Axis;
                    if (axis.show && !axis.IsCategory()) return false;
                }
            }
            return true;
        }

        public bool IsInAnyGrid(Vector2 local)
        {
            List<MainComponent> list;
            if (m_ComponentMaps.TryGetValue(typeof(GridCoord), out list))
            {
                foreach (var grid in list)
                {
                    if ((grid as GridCoord).Contains(local)) return true;
                }
            }
            return false;
        }

        internal string GetTooltipCategory(Serie serie)
        {
            var xAxis = GetChartComponent<XAxis>(serie.xAxisIndex);
            var yAxis = GetChartComponent<YAxis>(serie.yAxisIndex);
            if (yAxis.IsCategory())
            {
                return yAxis.GetData(serie.context.pointerItemDataIndex);
            }
            else if (xAxis.IsCategory())
            {
                return xAxis.GetData(serie.context.pointerItemDataIndex);
            }
            return null;
        }

        internal bool GetSerieGridCoordAxis(Serie serie, out Axis axis, out Axis relativedAxis)
        {
            var yAxis = GetChartComponent<YAxis>(serie.yAxisIndex);
            if (yAxis == null)
            {
                axis = null;
                relativedAxis = null;
                return false;
            }
            var isY = yAxis.IsCategory();
            if (isY)
            {
                axis = yAxis;
                relativedAxis = GetChartComponent<XAxis>(serie.xAxisIndex);
            }
            else
            {
                axis = GetChartComponent<XAxis>(serie.xAxisIndex);
                relativedAxis = yAxis;
            }
            return isY;
        }
    }
}