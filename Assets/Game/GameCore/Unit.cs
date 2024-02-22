using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.GameCore;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using ZergRush.CodeGen;
using ZergRush.ReactiveCore;

namespace Game.GameCore
{
    public partial class RTSTransform : RTSRuntimeData
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    public partial class Unit : RTSContextNode, ISelectable, IActionSource
    {
        public string sourceName => $"Unit ${nodeId}";
        public Faction faction => (Faction)parent;
        public RTSTransform transform;

        [GenIgnore, JetBrains.Annotations.CanBeNull] 
        public UnitView view;

        public float moveSpeed = 5;
        public List<UnitAction> unitActions;
        public void Tick(float dt)
        {
            foreach (var unitAction in unitActions)
            {
                unitAction.Tick(dt);
            }
            
            for (int i = unitActions.Count - 1; i >= 0; i--)
            {
                if (unitActions[i].state == ActionState.Finished)
                    unitActions.RemoveAt(i);
            }
        }

        public RTSTransform Transform => transform;
        [GenInclude]
        public bool IsSelected { get; set; }

        public void SetupAction(UnitAction actionPrototype)
        {
            UnitAction action = CreateChild(actionPrototype);
            action.Init();
            unitActions.Add(action);
        }
        
        public void MoveTo(Vector3 destination)
        {
            if (unitActions.Any(a => a is UnitMove))
            {
                unitActions.Find(a => a is UnitMove).Terminate(this);
            }
            SetupAction(new UnitMove {globalDestination = destination, moveSpeed = moveSpeed});
        }

        public void OnSelect()
        {
            if (view != null) 
                view.OnSelectionToggle(true);
        }

        public void OnDeselect()
        {
            if (view != null) 
                view.OnSelectionToggle(false);
        }

    }
}