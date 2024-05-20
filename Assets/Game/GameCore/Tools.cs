﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameCore
{
    public static class Tools
    {
        public static Vector3 Sum<T>(this IEnumerable<T> coll, Func<T, Vector3> selector)
        {
            Vector3 sum = Vector3.zero;
            foreach (var item in coll)
            {
                sum += selector(item);
            }

            return sum;
        }
        
        public static float Cross2D(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }
        
        public static float TriangleArea(Vector2 a, Vector2 b, Vector2 c)
        {
            return Cross2D(b - a, c - a) / 2f;
        }

        public static Matrix4x4 GetUnitToViewportMatrix(Camera camera)
        {
            return camera.projectionMatrix * camera.transform.worldToLocalMatrix;
        }
        
        public static UnitView GetUnitView(this string unitId)
        {
            return Resources.Load<UnitView>($"UnitViews/{unitId}");
        }
        public static Sprite GetUnitIcon(this string unitId)
        {
            return Resources.Load<Sprite>($"UnitIcons/{unitId}");
        }
    }
}