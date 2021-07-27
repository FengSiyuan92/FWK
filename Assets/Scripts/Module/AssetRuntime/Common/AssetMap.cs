using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace AssetRuntime
{
    public class AssetMap 
    {
        Dictionary<string, string> asset2Bundle = new Dictionary<string, string>();
        public AssetMap()
        {
            var validPath = AssetUtils.GetValidFilePath(AssetUtils.AssetMap);
            Debug.Log("AssetMap Path= " + validPath);
            TextReader reader = File.OpenText(validPath);
            while (true)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) break;
                var fmt = line.Split(AssetUtils.AssetMapSplit);
#if UNITY_EDITOR
                if (asset2Bundle.ContainsKey(fmt[0]))
                {
                    Debug.LogErrorFormat("存在相同名称资源'{0}'", fmt[0]);
                    continue;
                }
#endif
                asset2Bundle.Add(fmt[0], fmt[1]);
            }
        }

        public bool ContainsAsset(string assetName)
        {
            return asset2Bundle.ContainsKey(assetName);
        }

        public string GetAssetBundleName(string assetName)
        {
            string res = null;
            asset2Bundle.TryGetValue(assetName, out res);
            return res;
        }

      
    }
}
