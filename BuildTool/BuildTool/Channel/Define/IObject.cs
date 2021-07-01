using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Define
{
    public class IObject
    {
        /// <summary>
        /// 字段不排序,在生成时可以根据不同语言进行排序组织.比如lua可以先将所有的int类型排序
        /// </summary>
        Dictionary<string, IField> fields = new Dictionary<string, Define.IField>();

    }
}
