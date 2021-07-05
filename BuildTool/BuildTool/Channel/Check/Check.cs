using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define.Converter;
namespace Channel
{
    public class Check
    {
        public delegate void OnOverCompileEvent();
        public static event OnOverCompileEvent OnOverCompile;

        internal static bool CompileOverCheck()
        {
            try
            {
                OnOverCompile();
                return true;
            }
            catch (Exception e)
            {

                CLog.LogError(e.ToString());
            }
            return false;
        }


        static Check()
        {
            OnOverCompile += InnerCheckFunc.CheckConverterValid;
        }
    }

    internal class InnerCheckFunc
    {
        /// <summary>
        /// 检查扩展的类型转换器是否可用
        /// </summary>
        public static void CheckConverterValid()
        {
            var allextend = ExtendConverter.allExtend;
            foreach (var item in allextend)
            {
                if (item is EnumConverter)
                {
                    var e = Lookup.Enum[(item as EnumConverter).Name];
                    if (e == null)
                    {
                        CLog.LogError("不存在名称为{0}的枚举类型,但是却有配置表要使用该类型");
                    }
                }
                else if (item is CustomTypeConverter)
                {
                    var c = Lookup.CustomType[(item as CustomTypeConverter).Name];
                    if (c == null)
                    {
                        CLog.LogError("不存在名称为{0}的数据类型,但是却有配置表要使用该类型");
                    }
                }
            }
        }
    }
}
