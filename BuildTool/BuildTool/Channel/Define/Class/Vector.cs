using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Define.Class
{
    public class Vector2
    {
        public decimal x { get; internal set; }
        public decimal y { get; internal set; }

        public Vector2(decimal x =0, decimal y =0)
        {
            this.x = x;
            this.y = y;
        }
        public override string ToString()
        {
            return string.Format("[x={0},y={1}]", x, y);
        }
    }

    public class Vector3
    {
        public decimal x { get; internal set; }
        public decimal y { get; internal set; }
        public decimal z { get; internal set; }
        public Vector3(decimal x =0, decimal y = 0, decimal z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = y;
        }
        public override string ToString()
        {
            return string.Format("[x={0},y={1},z={2}]", x, y, z);
        }
    }

    public class Vector4
    {
        public decimal x { get; internal set; }
        public decimal y { get; internal set; }
        public decimal z { get; internal set; }
        public decimal w { get; internal set; }

        public Vector4(decimal x = 0, decimal y = 0, decimal z = 0, decimal w = 0)
        {
            this.x = x;
            this.y = y;
            this.z = y;
            this.w = w;
        }
        public override string ToString()
        {
            return string.Format("[x={0},y={1},z={2},w={3}]", x, y, z, w);
        }
    }
}
