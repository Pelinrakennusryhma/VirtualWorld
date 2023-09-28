using UnityEngine;
using Newtonsoft.Json;

namespace Dev
{
    public static class Utils
    {
        public static void DumpToConsole(object obj)
        {
            var output = JsonConvert.SerializeObject(obj, Formatting.Indented);
            Debug.Log(output);
        }

    }
}

