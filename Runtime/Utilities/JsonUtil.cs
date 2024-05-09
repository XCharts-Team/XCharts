using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

namespace XCharts.Runtime
{
    public static class JsonUtil
    {

        public static IEnumerator GetWebJson<T>(string url, Action<T[]> callback)
        {
            var www = UnityWebRequest.Get(url);
            yield return www;
#if UNITY_2020_1_OR_NEWER
            if (www.result != UnityWebRequest.Result.Success)
#else
            if (www.isNetworkError || www.isHttpError)
#endif
            {
                Debug.LogError("GetWebJson Error: " + www.error);
            }

            else
            {
                var json = www.downloadHandler.text.Trim();
                callback(GetJsonArray<T>(json));
                www.Dispose();
            }
        }

        public static IEnumerator GetWebJson<T>(string url, Action<T> callback)
        {
            var www = UnityWebRequest.Get(url);
            yield return www;
#if UNITY_2020_1_OR_NEWER
            if (www.result != UnityWebRequest.Result.Success)
#else
            if (www.isNetworkError || www.isHttpError)
#endif
            {
                Debug.LogError("GetWebJson Error: " + www.error);
            }
            else
            {
                var json = www.downloadHandler.text.Trim();
                callback(GetJsonObject<T>(json));
                www.Dispose();
            }
        }

        public static T GetJsonObject<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        public static T[] GetJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [Serializable]
        private class Wrapper<T>
        {
#pragma warning disable 0649
            public T[] array;
#pragma warning restore 0649
        }
    }
}