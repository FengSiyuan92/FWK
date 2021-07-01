using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Data
{

    public abstract class Content<T>
    {

        public Type ContentType => typeof(T);
        public abstract T GetValue();


    
    }

}
