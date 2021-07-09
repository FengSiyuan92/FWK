using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public delegate void OnOverParseEvent();
        public static event OnOverParseEvent OnOverParse;

        internal static bool OverParse()
        {
            try
            {
                OnOverParse();
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
            OnOverCompile += InnerCheck_ConvertValid.CheckConverterValid;
            OnOverParse += InnerCheck_CheckReferenceValid.CheckReference;
        }
    }
}
