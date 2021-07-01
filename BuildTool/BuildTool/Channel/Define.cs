using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel
{
    public enum TableModel
    {
        /// <summary>
        /// 哈希表 -k,v-
        /// </summary>
        MAP = 0,
        /// <summary>
        /// 双建表 -k,kv-
        /// </summary>
        BMAP = 2,
    }

    public enum ContentType
    {
        Unknow=0,
        Int,
        Long,
        Float,
        Double,
        String,
        Enum,
        Object,
    }
}
