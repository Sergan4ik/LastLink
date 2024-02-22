using System.Collections;
using Game.GameCore;
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
    public partial class Unit : RTSContextNode
    {
        public RTSTransform transform;

        [GenIgnore] public UnitView view;

        public float moveSpeed = 5;
        public void Tick(float dt)
        {
            transform.position.z += dt;
        }
        
    }
}