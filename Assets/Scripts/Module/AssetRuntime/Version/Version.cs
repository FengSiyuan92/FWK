
using System.Collections;
namespace AssetsRuntime
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

        public static implicit operator Version(string versionCode)
        {
            Version version = new Version();
            var split = versionCode.Split('.');
            version.apk = int.Parse(split[0]);
            version.main = int.Parse(split[1]);
            version.minor = int.Parse(split[2]);
            version.last = int.Parse(split[3]);
            return version;
        }

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
    }
}
