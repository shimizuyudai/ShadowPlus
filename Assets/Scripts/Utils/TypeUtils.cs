using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TypeUtil
{
    public struct IntVec2
    {
        public int x;
        public int y;
        public IntVec2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    [Serializable]
    public struct IntVec3
    {
        public int x;
        public int y;
        public int z;
        public IntVec3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    namespace Json
    {
        public struct Vec2
        {
            public float x;
            public float y;
            public Vec2(float x, float y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public struct Vec3
        {
            public float x;
            public float y;
            public float z;
            public Vec3(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }

        public struct Vec4
        {
            public float x;
            public float y;
            public float z;
            public float w;
            public Vec4(float x, float y, float z, float w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }
        }

        public struct Color
        {
            public float r;
            public float g;
            public float b;
            public float a;
            public Color(float r, float g, float b, float a)
            {
                this.r = r;
                this.g = g;
                this.b = b;
                this.a = a;
            }
        }
    }
}