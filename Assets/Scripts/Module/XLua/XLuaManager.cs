using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.IO;

public class XLuaManager : FMonoModule
{
    static LuaEnv m_Global;

    const string LUA_SCRIPT_PATH = "LuaCode";

    public override void OnInitialize()
    {
        m_Global = new LuaEnv();
        m_Global.AddLoader(CustomLoader);

        //注册pblua
        m_Global.AddBuildin("pb", XLua.LuaDLL.Lua.LoadPb);
        //注册rapidjson
        m_Global.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
    }

    public static byte[] CustomLoader(ref string filepath)
    {
        string scriptPath = string.Empty;
   
#if UNITY_EDITOR
        scriptPath = Path.Combine(Application.dataPath, LUA_SCRIPT_PATH);
        scriptPath = Path.Combine(scriptPath, filepath);

        return FileUtil.SafeReadAllBytes(scriptPath);
#endif
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
}
