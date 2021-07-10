using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define;

namespace Channel.Define.Converter
{
    internal abstract class Converter
    {

        public abstract object Convert(DataObject original, string originalValue, Field template, int depth = 0);

        public abstract System.Type GetResultType();

        /// <summary>
        ///  获取作为list子转换器时,默认的分隔符
        /// </summary>
        /// <returns></returns>
        public virtual char ElementSplit()
        {
            return ',';
        }

        internal abstract int SepLevel();

        public bool Valid { get; internal set; } = true;

    }
}
