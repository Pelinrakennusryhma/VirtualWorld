using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Dev
{
    public static class Utils
    {
        public static void DumpToConsole(object obj)
        {
            var output = JsonConvert.SerializeObject(obj, Formatting.Indented);
            Debug.Log(output);
        }
#if UNITY_EDITOR
        public static T[] GetAtPath<T>(string path)
        {
            path = path.Substring(7);

            ArrayList al = new ArrayList();
            string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);
            foreach (string fileName in fileEntries)
            {
                int index = fileName.LastIndexOf("\\");
                string localPath = "Assets/" + path;

                if (index > 0)
                    localPath += fileName.Substring(index);
                localPath = localPath.Replace("\\", "/");
                //Debug.Log("localPath: " + localPath);

                Object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));

                if (t != null)
                    al.Add(t);
            }
            T[] result = new T[al.Count];
            for (int i = 0; i < al.Count; i++)
                result[i] = (T)al[i];

            return result;

        }

#endif
    }
}

