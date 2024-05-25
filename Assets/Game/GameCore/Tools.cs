using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZergRush.ReactiveCore;

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
        public static GlobalPlayerDatabase GetGlobalPlayerDatabase()
        {
            return Resources.Load<GlobalPlayerDatabase>("GlobalPlayerDatabase");
        }
        public static IEnumerable<Sprite> GetPlayerAvatars()
        {
            return Resources.LoadAll<Sprite>("PlayerAvatars");
        }
        public static Sprite GetPlayerAvatar(this string avatarId)
        {
            return Resources.Load<Sprite>($"PlayerAvatars/{avatarId}");
        }

        [MustUseReturnValue]
        public static StateButton SetupButtonStates(this Button button, Action<Button, TextMeshProUGUI, int> onChange,
            TextMeshProUGUI label = null, Cell<int> stateCell = null)
        {
            var stateButton = new StateButton
            {
                button = button,
                label = label
            };
            
            if (stateCell == null)
                stateButton.Setup(onChange);
            else
                stateButton.Setup(onChange, stateCell);

            return stateButton;
        }
    }
}