using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

namespace AssetRuntime
{
    public enum FileBelong
    {
        PERSISTENT,
        STEAMING
    }

    public class FileList
    {
        public class FileDetail
        {
            public string fileName;
            public long size;
            public string md5;
        }

        FileBelong m_Belong;

        string m_PreFilePath;

        public FileList(FileBelong belong)
        {
            m_Belong = belong;
        }

        Dictionary<string, FileDetail> files = new Dictionary<string, FileDetail>();

        public bool FillInfoByFilePath(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if (!file.Exists)
                return false;

            var reader = file.OpenText();
            while (true)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) break;

                var fmt = line.Split(AssetUtils.AssetMapSplit);
#if UNITY_EDITOR
                if (files.ContainsKey(fmt[0]))
                {
                    Debug.LogErrorFormat("存在相同名称文件'{0}'", fmt[0]);
                    continue;
                }
#endif

                var detail = new FileDetail();
                detail.fileName = fmt[0];
                detail.size = long.Parse(fmt[1]);
                detail.md5 = fmt[2];

                files.Add(fmt[0], detail);
            }
            reader.Close();

            return true;

        }

        public bool ContainsFile(string fileName)
        {
            return files.ContainsKey(fileName);
        }

        public string TrySearchFilePath(string fileName)
        {
            FileDetail res = null;
            if (files.TryGetValue(fileName, out res))
            {
                switch (m_Belong)
                {
                    case FileBelong.PERSISTENT:
                        return AssetUtils.GetPersistentFilePath(res.fileName);
                    case FileBelong.STEAMING:
                        return AssetUtils.GetStreamingFilePath(res.fileName);
                    default:
                        return null;
                }
            }
            return null;
        }
    }
}
