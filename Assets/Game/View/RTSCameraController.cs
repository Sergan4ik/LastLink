﻿using System;
using Game.GameCore;
using JetBrains.Annotations;
using Unity.AI.Navigation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Game
{
    public class RTSCameraController : ConnectableMonoBehaviour
    {
        public Camera currentCamera;
        public float moveSpeed = 10f;
        public AnimationCurve moveSpeedCurve = AnimationCurve.Linear(0, 0.6f, 1, 5);
        public Vector2 scrollLimits = new Vector2(10, 100);
        public float scrollSpeed = 2f;

        private static readonly float maxPossibleMoveDelta = 70.1f;
        public void Update()
        {
            if (Mouse.current.middleButton.isPressed || Keyboard.current.altKey.isPressed)
            {
                var move = new Vector3(Mouse.current.delta.x.ReadValue(), 0, Mouse.current.delta.y.ReadValue());
                float curveModifier = moveSpeedCurve.Evaluate(move.magnitude / maxPossibleMoveDelta);
                var deltaVector = -move * (moveSpeed * curveModifier * Time.deltaTime);
                currentCamera.transform.position += deltaVector;
            }
            
            var scroll = Mouse.current.scroll.ReadValue().y;
            if (scroll != 0)
            {
                var scrollDelta = scroll * scrollSpeed * Time.deltaTime;
                var newScroll = Mathf.Clamp(currentCamera.transform.position.y - scrollDelta, scrollLimits.x, scrollLimits.y);
                currentCamera.transform.position = new Vector3(currentCamera.transform.position.x, newScroll, currentCamera.transform.position.z);
            }
        }

        public bool TryGetPointedUnitView(out UnitView view)
        {
            view = null;
            var ray = currentCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, 99999999, 1 << LayerMask.NameToLayer("Unit")))
            {
                bool isUnit = hit.transform.TryGetComponent<UnitView>(out var unitView);
                if (isUnit)
                    view = unitView;
                return isUnit;
            }
            else
            {
                return false;
            }
        }
        public bool TryGetPointedUnit(out Unit unit)
        {
            bool res = TryGetPointedUnitView(out var view);
            unit = view?.currentUnit;
            return res;
        }
        public bool TryGetWorldMousePosition(out Vector3 worldMousePosition)
        {
            var ray = currentCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, 99999999, 1 << LayerMask.NameToLayer("Ground")))
            {
                worldMousePosition = hit.point;
                return true;
            }
            else
            {
                worldMousePosition = Vector3.negativeInfinity;
                return false;
            }
        }
        
        public Vector3 worldMousePosition
        {
            get
            {
                TryGetWorldMousePosition(out var pos);
                return pos;
            }
        }
        
        [CanBeNull]
        public UnitView pointedUnitView
        {
            get
            {
                TryGetPointedUnitView(out var view);
                return view;
            }
        }
        
        [CanBeNull]
        public Unit pointedUnit
        {
            get
            {
                TryGetPointedUnit(out var unit);
                return unit;
            }
        }
    }
}   