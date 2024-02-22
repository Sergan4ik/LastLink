using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using ZergRush.ReactiveCore;

namespace Game.GameCore
{
    public partial class RTSTransform : RTSRuntimeData
    {
        public Cell<Vector3> position;
        public Cell<Quaternion> rotation;

        public Vector3 Position => position.value;
        public Quaternion Rotation => rotation.value;
    }
    
    public enum ActionState
    {
        NotStarted,
        Preparing,
        Processing,
        Finished
    }

    public partial class Unit : RTSContextNode
    {
        public RTSTransform transform;

        public List<UnitAction> activeActions;

        public float moveSpeed = 5;
        public void Tick(float dt)
        {
            foreach (var action in activeActions)
            {
                action.Tick(dt);
            }

            for (int i = activeActions.Count - 1; i >= 0; i--)
            {
                if (activeActions[i].state == ActionState.Finished)
                {
                    activeActions.RemoveAt(i);
                }
            }
        }
        
        public void MoveTo(Vector3 destination, bool isDirectionalMove)
        {
        }
    }
}