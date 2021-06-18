using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Data
{
    public interface ITable
    {
        string Name { get; }
        string Path { get;}

        ITable MainTable { get; }

        List<ITable> SubTable { get; }
    }
}
