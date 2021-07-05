using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Define.Class
{
    public class Vector2
    {
        public float x { get; internal set; }
        public float y { get; internal set; }

        public Vector2(float x =0, float y=0)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class Vector3
    {
        public float x { get; internal set; }
        public float y { get; internal set; }
        public float z { get; internal set; }
        public Vector3(float x=0, float y= 0, float z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = y;
        }
    }

    public class Vector4
    {
        public float x { get; internal set; }
        public float y { get; internal set; }
        public float z { get; internal set; }
        public float w { get; internal set; }

        public Vector4(float x = 0, float y = 0, float z = 0, float w = 0)
        {
            this.x = x;
            this.y = y;
            this.z = y;
            this.w = w;
        }
    }
}
