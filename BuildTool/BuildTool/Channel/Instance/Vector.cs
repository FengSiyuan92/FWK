﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel
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

        public override bool Equals(object obj)
        {
            var target = obj as Vector2;
            if (target == null) return false;
            return x == target.x && y == target.y;
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

        public override bool Equals(object obj)
        {
            var target = obj as Vector3;
            if (target == null) return false;
            return x == target.x && y == target.y && z==target.z;
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

        public override bool Equals(object obj)
        {
            var target = obj as Vector4;
            if (target == null) return false;
            return x == target.x && y == target.y && z == target.z && w == target.w;
        }

    }
}