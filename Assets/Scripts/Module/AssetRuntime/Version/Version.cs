
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace AssetRuntime
{
    public enum UpdateType
    {
        None,
        // 大版本更新(C#代码变更)需要重新安装app
        ReInstall,

        // 有热修复的C#的lua代码变更,更新并退出游戏
        UpdateAndQuit,

        //lua框架侧代码修改,重启lua虚拟机
        UpdateAndReStart,

        // 只资源,非lua框架的代码,无热修复c#代码时,只需要更新即可
        UpdateOnly,
    }


    public class Version
    {

        int apk;
        int main;
        int minor;
        int last;

        List<FileList> fileMaps = new List<FileList>();

        public string VersionCode => string.Format("{0}-{1}.{2}.{3}", apk, main, minor, last);



        /// <summary>
        /// 计算两个版本的更新类型
        /// </summary>
        /// <param name="oldVersion"></param>
        /// <param name="newVersion"></param>
        /// <returns></returns>
        public  UpdateType Compare(Version newVersion)
        {
            Version oldVersion = this;
            if (newVersion.apk > oldVersion.apk)
            {
                return UpdateType.ReInstall;
            }
            else if (newVersion.main > oldVersion.main)
            {
                return UpdateType.UpdateAndQuit;
            }
            else if (newVersion.minor > oldVersion.minor)
            {
                return UpdateType.UpdateAndReStart;
            }
            else if (newVersion.last > oldVersion.last)
            {
                return UpdateType.UpdateOnly;
            }
            return UpdateType.None;
        }

        public IEnumerator UpdateTo(Version newVersion)
        {
            yield return null;
        }

        public static Version GenServerVersion()
        {
            return null;
        }

        /// <summary>
        /// 获取该版本内对应的文件路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFilePath(string fileName)
        {
            for (int i = 0; i < fileMaps.Count; i++)
            {
                var path = fileMaps[i].TrySearchFilePath(fileName);
                if (!string.IsNullOrEmpty(path)) return path;
            }
            throw new System.Exception("没有找到对应文件" + fileName);
        }

        public void SetVersionCode(string versionCode)
        {
            var split = versionCode.Split('.', '-');
            apk = int.Parse(split[0]);
            main = int.Parse(split[1]);
            minor = int.Parse(split[2]);
            last = int.Parse(split[3]);
        }

        public static implicit operator Version(string versionCode)
        {
            Version version = new Version();
            version.SetVersionCode(versionCode);
            return version;
        }

        /// <summary>
        /// 获取客户端版本
        /// </summary>
        /// <returns></returns>
        public static Version GenClientVersion()
        {
            Version clientVersion = new Version();
            clientVersion.fileMaps = new List<FileList>(2);

            // 构建persistentPath中的文件映射
            var persistentFileMapPath = AssetUtils.GetPersistentFilePath(AssetUtils.FileDetail);
            if (File.Exists(persistentFileMapPath))
            {
                FileList persistentFileList = new FileList(FileBelong.PERSISTENT);
                persistentFileList.FillInfoByFilePath(persistentFileMapPath);
                clientVersion.fileMaps.Add(persistentFileList);
            }

            // 构建StreamingAssets中的文件映射
            var streamingPath = AssetUtils.GetStreamingFilePath(AssetUtils.FileDetail);
            FileList steamigFileList = new FileList(FileBelong.STEAMING);
            steamigFileList.FillInfoByFilePath(streamingPath);
            clientVersion.fileMaps.Add(steamigFileList);

            // 构建版本号
            string versionCode = "0.0.0.0";

            // 优先使用persistent中的
            var validPath = AssetUtils.GetValidFilePath(AssetUtils.VersionFileName);
            if (!string.IsNullOrEmpty(validPath))
            {
                var read = File.OpenText(validPath);
                versionCode = read.ReadLine().Trim();
                read.Close();
            }
            clientVersion.SetVersionCode(versionCode);
            return clientVersion;
        }

    }
}
