using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    public class TooltipViewItem
    {
        public GameObject gameObject;
        public List<ChartLabel> columns = new List<ChartLabel>();
    }
    public class TooltipView
    {
        private static Vector2 anchorMax = new Vector2(0, 1);
        private static Vector2 anchorMin = new Vector2(0, 1);
        private static Vector2 pivot = new Vector2(0, 1);
        private static Vector2 v2_0_05 = new Vector2(0, 0.5f);

        public Tooltip tooltip;
        public ComponentTheme theme;
        public GameObject gameObject;
        public Transform transform;
        public Image background;
        public Outline border;
        public VerticalLayoutGroup layout;
        public ChartLabel title;
        private List<TooltipViewItem> m_Items = new List<TooltipViewItem>();
        private List<float> m_ColumnMaxWidth = new List<float>();
        private bool m_Active = false;
        private Vector3 m_TargetPos;
        private Vector3 m_CurrentVelocity;

        public void Update()
        {
            if (!m_Active)
                return;
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, m_TargetPos, ref m_CurrentVelocity, 0.08f);
        }

        public Vector3 GetCurrentPos()
        {
            return transform.localPosition;
        }

        public Vector3 GetTargetPos()
        {
            return m_TargetPos;
        }

        public void UpdatePosition(Vector3 pos)
        {
            m_TargetPos = pos;
        }

        public void SetActive(bool flag)
        {
            m_Active = flag && tooltip.showContent;
            ChartHelper.SetActive(gameObject, m_Active);
            if (!flag)
            {
                foreach (var item in m_Items)
                    item.gameObject.SetActive(false);
            }
        }

        public void Refresh()
        {
            if (tooltip == null) return;
            var data = tooltip.context.data;
            var ignoreColumn = string.IsNullOrEmpty(tooltip.ignoreDataDefaultContent);

            var titleActive = !string.IsNullOrEmpty(data.title);
            ChartHelper.SetActive(title, titleActive);
            title.SetText(data.title);

            m_ColumnMaxWidth.Clear();
            for (int i = 0; i < data.param.Count; i++)
            {
                var item = GetItem(i);
                var param = data.param[i];
                if (param.columns.Count <= 0 || (ignoreColumn && param.ignore))
                {
                    item.gameObject.SetActive(false);
                    continue;
                }
                item.gameObject.SetActive(true);
                for (int j = 0; j < param.columns.Count; j++)
                {
                    var column = GetItemColumn(item, j);
                    column.SetActive(true);
                    column.SetText(param.columns[j]);

                    if (j == 0)
                        column.text.SetColor(param.color);

                    if (j >= m_ColumnMaxWidth.Count)
                        m_ColumnMaxWidth.Add(0);

                    var columnWidth = column.GetWidth();
                    if (m_ColumnMaxWidth[j] < columnWidth)
                        m_ColumnMaxWidth[j] = columnWidth;
                }
                for (int j = param.columns.Count; j < item.columns.Count; j++)
                {
                    item.columns[j].SetActive(false);
                }
            }
            for (int i = data.param.Count; i < m_Items.Count; i++)
            {
                m_Items[i].gameObject.SetActive(false);
            }
            ResetSize();
            UpdatePosition(tooltip.context.pointer + tooltip.offset);
            tooltip.gameObject.transform.SetAsLastSibling();
        }

        private void ResetSize()
        {
            var maxHig = 0f;
            var maxWid = 0f;
            if (tooltip.fixedWidth > 0)
            {
                maxWid = tooltip.fixedWidth;
            }
            else
            {
                maxWid = TotalMaxWidth();
                var titleWid = title.GetTextWidth();
                if (maxWid < titleWid)
                    maxWid = titleWid;
            }

            if (tooltip.fixedHeight > 0)
            {
                maxHig = tooltip.fixedHeight;
            }
            else
            {
                if (!string.IsNullOrEmpty(title.text.GetText()))
                    maxHig += tooltip.titleHeight;
                maxHig += tooltip.itemHeight * tooltip.context.data.param.Count;
                maxHig += tooltip.paddingTopBottom * 2;
            }

            if (tooltip.minWidth > 0 && maxWid < tooltip.minWidth)
                maxWid = tooltip.minWidth;

            if (tooltip.minHeight > 0 && maxHig < tooltip.minHeight)
                maxHig = tooltip.minHeight;

            for (int i = 0; i < m_Items.Count; i++)
            {
                var item = m_Items[i];
                item.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(maxWid, tooltip.itemHeight);
                var xPos = 0f;
                for (int j = 0; j < m_ColumnMaxWidth.Count; j++)
                {
                    var deltaX = j == m_ColumnMaxWidth.Count - 1 ? maxWid - xPos : m_ColumnMaxWidth[j];
                    item.columns[j].text.SetSizeDelta(new Vector2(deltaX, tooltip.itemHeight));
                    item.columns[j].SetRectPosition(new Vector3(xPos, 0));
                    xPos += m_ColumnMaxWidth[j];
                }
            }
            tooltip.context.width = maxWid + tooltip.paddingLeftRight * 2;
            tooltip.context.height = maxHig;
            background.GetComponent<RectTransform>().sizeDelta = new Vector2(tooltip.context.width, tooltip.context.height);
        }

        private float TotalMaxWidth()
        {
            var total = 0f;
            foreach (var max in m_ColumnMaxWidth)
                total += max;
            return total;
        }

        private TooltipViewItem GetItem(int i)
        {
            if (i < 0) i = 0;
            if (i < m_Items.Count)
            {
                return m_Items[i];
            }
            else
            {
                var item = CreateViewItem(i, gameObject.transform, tooltip, theme);
                m_Items.Add(item);
                return item;
            }
        }

        private ChartLabel GetItemColumn(TooltipViewItem item, int i)
        {
            if (i < 0) i = 0;
            if (i < item.columns.Count)
            {
                return item.columns[i];
            }
            else
            {
                var column = CreateViewItemColumn(i, item.gameObject.transform, tooltip, theme);
                item.columns.Add(column);
                return column;
            }
        }

        public static TooltipView CreateView(Tooltip tooltip, ThemeStyle theme, Transform parent)
        {
            var view = new TooltipView();
            view.tooltip = tooltip;
            view.theme = theme.tooltip;

            view.gameObject = ChartHelper.AddObject("view", parent, anchorMin, anchorMax, pivot, Vector3.zero);
            view.gameObject.transform.localPosition = Vector3.zero;
            view.transform = view.gameObject.transform;

            view.background = ChartHelper.EnsureComponent<Image>(view.gameObject);
            view.background.sprite = tooltip.backgroundImage;
            view.background.type = tooltip.backgroundType;
            view.background.color = ChartHelper.IsClearColor(tooltip.backgroundColor) ?
                Color.white : tooltip.backgroundColor;

            view.border = ChartHelper.EnsureComponent<Outline>(view.gameObject);
            view.border.enabled = tooltip.borderWidth > 0;
            view.border.useGraphicAlpha = false;
            view.border.effectColor = tooltip.borderColor;
            view.border.effectDistance = new Vector2(tooltip.borderWidth, -tooltip.borderWidth);

            view.layout = ChartHelper.EnsureComponent<VerticalLayoutGroup>(view.gameObject);
            view.layout.childControlHeight = false;
            view.layout.childControlWidth = false;
            view.layout.childForceExpandHeight = false;
            view.layout.childForceExpandWidth = false;
            view.layout.padding = new RectOffset(tooltip.paddingLeftRight,
                tooltip.paddingLeftRight,
                tooltip.paddingTopBottom,
                tooltip.paddingTopBottom);

            view.title = ChartHelper.AddChartLabel("title", view.gameObject.transform, tooltip.titleLabelStyle, theme.tooltip,
                "", Color.clear, TextAnchor.MiddleLeft);

            var item = CreateViewItem(0, view.gameObject.transform, tooltip, theme.tooltip);
            view.m_Items.Add(item);

            view.Refresh();

            return view;
        }

        private static TooltipViewItem CreateViewItem(int i, Transform parent, Tooltip tooltip, ComponentTheme theme)
        {
            GameObject item1 = ChartHelper.AddObject("item" + i, parent, anchorMin, anchorMax, v2_0_05, Vector3.zero);

            var item = new TooltipViewItem();
            item.gameObject = item1;
            item.columns.Add(CreateViewItemColumn(0, item1.transform, tooltip, theme));
            item.columns.Add(CreateViewItemColumn(1, item1.transform, tooltip, theme));
            item.columns.Add(CreateViewItemColumn(2, item1.transform, tooltip, theme));
            return item;
        }

        private static ChartLabel CreateViewItemColumn(int i, Transform parent, Tooltip tooltip, ComponentTheme theme)
        {
            var labelStyle = tooltip.GetContentLabelStyle(i);
            var label = ChartHelper.AddChartLabel("column" + i, parent, labelStyle, theme,
                "", Color.clear, TextAnchor.MiddleLeft);
            return label;
        }
    }
}