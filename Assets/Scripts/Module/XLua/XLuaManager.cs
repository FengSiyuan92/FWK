using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.IO;

public class XLuaManager : FMonoModule
{
    static LuaEnv m_Global;

    const string LUA_SCRIPT_PATH = "LuaCode";


    public override IEnumerator OnPrepare()
    {
        m_Global = new LuaEnv();
        m_Global.AddLoader(CustomLoader);
        yield return null;
        //注册pblua
        m_Global.AddBuildin("pb", XLua.LuaDLL.Lua.LoadPb);
        //注册rapidjson
        m_Global.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
        yield break;
    }

    // todo 加载出新的lua文件后可能需要重启虚拟机
    public override void Restart()
    {
        base.Restart();
    }

    public override void OnInitialize()
    {
        m_Global.DoString("require 'Core.define'");
        m_Global.DoString("(require 'Core.main').Start()");
    }

    /// <summary>
    /// todo 先只做editor侧的代码
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    public static byte[] CustomLoader(ref string filepath)
    {
        string scriptPath = string.Empty;
   
#if UNITY_EDITOR
        scriptPath = Path.Combine(Application.dataPath, LUA_SCRIPT_PATH);
        scriptPath = Path.Combine(scriptPath, filepath);

        scriptPath = scriptPath.Replace(".", "/") + ".lua";
        return FileUtil.SafeReadAllBytes(scriptPath);
#endif
        return new byte[0];
        //        string path = Path.GetFileName(filepath);

        //        ZipEntry entry = gameLua.GetEntry(path);
        //        if (entry != null)
        //        {
        //            Stream stream = gameLua.GetInputStream(entry) as InflaterInputStream;
        //            byte[] buffer = new byte[(int)entry.Size];
        //            stream.Read(buffer, 0, buffer.Length);
        //            stream.Close();
        //            return buffer;
        //        }
        //        return null;
    }

    public static void DoString(string str)
    {
        m_Global.DoString(str);
    }
}
