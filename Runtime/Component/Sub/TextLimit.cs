/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// 文本字符限制和自适应。当文本长度超过设定的长度时进行裁剪，并将后缀附加在最后。
    /// 只在类目轴中有效。
    /// </summary>
    [Serializable]
    public class TextLimit : SubComponent
    {
        [SerializeField] private bool m_Enable = true;
        [SerializeField] private float m_MaxWidth = 0;
        [SerializeField] private float m_Gap = 1;
        [SerializeField] private string m_Suffix = "...";

        /// <summary>
        /// 是否启用文本自适应。默认为true。
        /// </summary>
        public bool enable
        {
            get { return m_Enable; }
            set { if (PropertyUtility.SetStruct(ref m_Enable, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 设定最大宽度。默认为0表示自动获取，否则表示自定义。当文本的宽度大于该值进行裁剪。
        /// </summary>
        public float maxWidth
        {
            get { return m_MaxWidth; }
            set { if (PropertyUtility.SetStruct(ref m_MaxWidth, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 两边留白像素距离。默认为10
        /// </summary>
        public float gap
        {
            get { return m_Gap; }
            set { if (PropertyUtility.SetStruct(ref m_Gap, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 长度超出时的后缀。
        /// </summary>
        public string suffix
        {
            get { return m_Suffix; }
            set { if (PropertyUtility.SetClass(ref m_Suffix, value)) SetComponentDirty(); }
        }

        private Text m_RelatedText;
        private TextGenerationSettings m_RelatedTextSettings;
        private float m_RelatedTextWidth = 0;

        public TextLimit Clone()
        {
            var textLimit = new TextLimit();
            textLimit.enable = enable;
            textLimit.maxWidth = maxWidth;
            textLimit.gap = gap;
            textLimit.suffix = suffix;
            return textLimit;
        }

        public void Copy(TextLimit textLimit)
        {
            enable = textLimit.enable;
            maxWidth = textLimit.maxWidth;
            gap = textLimit.gap;
            suffix = textLimit.suffix;
        }

        public void SetRelatedText(Text txt, float labelWidth)
        {
            m_RelatedText = txt;
            m_RelatedTextSettings = txt.GetGenerationSettings(Vector2.zero);
            m_RelatedTextWidth = labelWidth;
        }


        public string GetLimitContent(string content)
        {
            float checkWidth = m_MaxWidth > 0 ? m_MaxWidth : m_RelatedTextWidth;
            if (m_RelatedText == null || checkWidth <= 0) return content;
            else
            {
                if (m_Enable)
                {
                    float len = m_RelatedText.cachedTextGenerator.GetPreferredWidth(content, m_RelatedTextSettings);
                    float suffixLen = m_RelatedText.cachedTextGenerator.GetPreferredWidth(suffix, m_RelatedTextSettings);
                    if (len >= checkWidth - m_Gap * 2)
                    {
                        return content.Substring(0, GetAdaptLength(content, suffixLen)) + suffix;
                    }
                    else
                    {
                        return content;
                    }
                }
                else
                {
                    return content;
                }
            }
        }

        private int GetAdaptLength(string content, float suffixLen)
        {
            int start = 0;
            int middle = content.Length / 2;
            int end = content.Length;
            float checkWidth = m_MaxWidth > 0 ? m_MaxWidth : m_RelatedTextWidth;
            float limit = checkWidth - m_Gap * 2 - suffixLen;
            if (limit < 0) return 0;
            float len = 0;
            while (len != limit && middle != start)
            {
                len = m_RelatedText.cachedTextGenerator.GetPreferredWidth(content.Substring(0, middle), m_RelatedTextSettings);
                if (len < limit)
                {
                    start = middle;
                }
                else if (len > limit)
                {
                    end = middle;
                }
                else
                {
                    break;
                }
                middle = (start + end) / 2;
            }
            return middle;
        }
    }
}