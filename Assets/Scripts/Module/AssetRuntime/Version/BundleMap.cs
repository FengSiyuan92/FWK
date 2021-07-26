
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace AssetRuntime
{
    public struct BundleInfo
    {
        public string name;
        public long size;
        public string md5;

    }

    public class BundleMap
    {
        Dictionary<string, BundleInfo> bundleMap = new Dictionary<string, BundleInfo>();

        public const char AssetSplit = '|';

        public BundleMap(FileInfo content)
        {
            TextReader reader = content.OpenText();
            while (true)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) break;

                var fmt = line.Split(AssetSplit);
#if UNITY_EDITOR
                if (bundleMap.ContainsKey(fmt[0]))
                {
                    Debug.LogErrorFormat("存在相同名称资源'{0}'", fmt[0]);
                    continue;
                }
#endif

                var bundleInfo = new BundleInfo();
                bundleInfo.name = fmt[0];
                bundleInfo.size = long.Parse(fmt[1]);
                bundleInfo.md5 = fmt[2];
                bundleMap.Add(fmt[0], bundleInfo);
            }

        }

        public bool ContainsBundle(string bundlePath)
        {
            return bundleMap.ContainsKey(bundlePath);
        }

        public BundleInfo GetBundleInfo(string bundlePath)
        {
            BundleInfo res = default(BundleInfo);
            bundleMap.TryGetValue(bundlePath, out res);
            return res;
        }
    }
}
