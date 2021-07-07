/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using XCharts;

namespace XChartsDemo
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    internal class Demo_Animation : MonoBehaviour
    {
        [SerializeField] private int m_FadeInDuration = 1000;
        [SerializeField] private int m_FadeOutDuration = 1000;
        [SerializeField] private int m_DataChangeDuration = 500;
        void Awake()
        {
            InitCharts();
            TryInitButton("buttons/btnFadeIn", AnimationFadeIn);
            TryInitButton("buttons/btnFadeOut", AnimationFadeOut);
            TryInitButton("buttons/btnDataAdd", AnimationDataAdd);
            TryInitButton("buttons/btnDataChange", AnimationDataChange);
            TryInitButton("buttons/btnPause", AnimationPause);
            TryInitButton("buttons/btnResume", AnimationResume);
            TryInitButton("buttons/btnReset", AnimationReset);
        }

        void InitCharts()
        {
            var charts = transform.GetComponentsInChildren<BaseChart>();
            foreach (var chart in charts)
            {
                foreach (var serie in chart.series.list)
                {
                    serie.animation.fadeInDuration = m_FadeInDuration;
                    serie.animation.fadeOutDuration = m_FadeOutDuration;
                    serie.animation.dataChangeDuration = m_DataChangeDuration;
                }
            }
            SetInputField("settings/fadeIn/InputField", m_FadeInDuration, OnFadeInDurationChanged);
            SetInputField("settings/fadeOut/InputField", m_FadeOutDuration, OnFadeOutDurationChanged);
            SetInputField("settings/dataChange/InputField", m_DataChangeDuration, OnDataChangeDurationChanged);
        }

        void SetInputField(string path, int value, UnityAction<string> act)
        {
            var input = transform.Find(path).gameObject.GetComponent<InputField>();
            input.onEndEdit.AddListener(act);
            input.text = value.ToString();
        }

        Button TryInitButton(string name, UnityAction act)
        {
            var trans = transform.Find(name);
            if (trans)
            {
                var btn = trans.gameObject.GetComponent<Button>();
                btn.onClick.AddListener(act);
                return btn;
            }
            else
            {
                return null;
            }
        }

        void AnimationFadeIn()
        {
            var charts = transform.GetComponentsInChildren<BaseChart>();
            foreach (var chart in charts)
            {
                chart.AnimationFadeIn();
            }
        }

        void AnimationFadeOut()
        {
            var charts = transform.GetComponentsInChildren<BaseChart>();
            foreach (var chart in charts)
            {
                chart.AnimationFadeOut();
            }
        }

        void AnimationDataAdd()
        {
            var charts = transform.GetComponentsInChildren<BaseChart>();
            foreach (var chart in charts)
            {
                chart.AnimationFadeOut();
            }
        }

        void AnimationDataChange()
        {
            var charts = transform.GetComponentsInChildren<BaseChart>();
            foreach (var chart in charts)
            {
                if (chart is RingChart)
                {
                    var serieData = chart.series.GetSerie(0).GetSerieData(0);
                    chart.UpdateData(0, 0, 0, Random.Range(0, (float)serieData.GetData(1)));
                }
                else
                {
                    var dataCount = chart.series.list[0].dataCount;
                    var index = UnityEngine.Random.Range(0, dataCount);
                    chart.UpdateData(0, index, UnityEngine.Random.Range(1, 100));
                }
            }
        }

        void AnimationPause()
        {
            var charts = transform.GetComponentsInChildren<BaseChart>();
            foreach (var chart in charts)
            {
                chart.AnimationPause();
            }
        }

        void AnimationResume()
        {
            var charts = transform.GetComponentsInChildren<BaseChart>();
            foreach (var chart in charts)
            {
                chart.AnimationResume();
            }
        }

        void AnimationReset()
        {
            var charts = transform.GetComponentsInChildren<BaseChart>();
            foreach (var chart in charts)
            {
                chart.AnimationReset();
            }
        }

        void OnFadeInDurationChanged(string content)
        {
            var charts = transform.GetComponentsInChildren<BaseChart>();
            m_FadeInDuration = int.Parse(content);
            foreach (var chart in charts)
            {
                foreach (var serie in chart.series.list)
                {
                    serie.animation.fadeInDuration = m_FadeInDuration;
                }
            }
        }

        void OnFadeOutDurationChanged(string content)
        {
            var charts = transform.GetComponentsInChildren<BaseChart>();
            m_FadeOutDuration = int.Parse(content);
            foreach (var chart in charts)
            {
                foreach (var serie in chart.series.list)
                {
                    serie.animation.fadeOutDuration = m_FadeOutDuration;
                }
            }
        }

        void OnDataChangeDurationChanged(string content)
        {
            var charts = transform.GetComponentsInChildren<BaseChart>();
            m_DataChangeDuration = int.Parse(content);
            foreach (var chart in charts)
            {
                foreach (var serie in chart.series.list)
                {
                    serie.animation.dataChangeDuration = m_DataChangeDuration;
                }
            }
        }
    }
}