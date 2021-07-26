using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo;
using Neo.IronLua;
using Channel.Define;
using System.IO;
using System.Diagnostics;
namespace Channel
{
    internal class LuaCheckManager
    {
        public static Neo.IronLua.Lua lua;
        public static LuaGlobal global;

        static LuaCheckManager()
        {
            lua = new Neo.IronLua.Lua();
            global = lua.CreateEnvironment();
            global.DoChunk(@"
local Channel = clr.Channel
CLog = Channel.CLog
Lookup = Channel.Lookup
Utils = Channel.Utils
local define = Channel.Define
Enum = define.Enum
Item = Enum.Item
CustomClass = define.CustomClass
Field = define.Field

DataArray = Channel.DaraArray
DataOject = Channel.DataObject
Vector2 = Channel.Vector2
Vector3 = Channel.Vector3
Vector4 = Channel.Vector4
", "Inject");
        }

        public static void AddCheckFile(string filePath)
        {

            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var content = File.OpenText(filePath);
            var info = content.ReadToEnd();
            try
            {
                var obs = global.DoChunk(info, fileName);
                var method = obs[0] as Delegate;

                var value2 = obs.Count > 1 ? obs[1] : null;
                var intValue = 0;
                if (value2 == null)
                {
                    int.TryParse(value2.ToString(), out intValue);
                }
                var stage = (CheckStage)intValue;

                Checker.AddRule(new Rule(fileName, (c) =>
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    try
                    {
                        method.DynamicInvoke(new object[] { c });
                    }
                    catch (Exception e)
                    {
                        CLog.LogError(e.ToString());
                    }
                    sw.Stop();
                    CLog.LogError("lua检查规则'{0}'总耗时" + sw.Elapsed.TotalSeconds, fileName);
                }),
                stage);
            }
            catch
            {
                CLog.LogError("定义的lua检查规则语法错误=>" + filePath);
            }
          
        }

    }
}
