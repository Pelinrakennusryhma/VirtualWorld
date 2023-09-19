using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace BackendConnection
{
    public static class WebRequestUtils
    {
        public static UnityWebRequest CreateRequest(string path, RequestType type, object data = null)
        {
            Debug.Log("path: " + path);
            UnityWebRequest request = new UnityWebRequest(path, type.ToString());

            if (data != null)
            {
                string json = JsonUtility.ToJson(data);
                byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            return request;
        }

        // get async webrequest
        public static async UniTask<string> GetTextAsync(UnityWebRequest req)
        {
            var op = await req.SendWebRequest();
            return op.downloadHandler.text;
        }
    }
}

