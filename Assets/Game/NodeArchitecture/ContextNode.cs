using System;
using System.Collections.Generic;
using UnityEngine;
using ZergRush.CodeGen;

namespace Game.NodeArchitecture
{
    [GenTask(GenTaskFlags.SimpleDataPack | GenTaskFlags.PolymorphicConstruction)]
    public partial class ContextNode
    {
        [GenIgnore]
        public ContextRoot root;
        [GenIgnore]
        public ContextNode parent;
        public List<int> childs = new List<int>();
        public int nodeId;
        
        public virtual T CreateChild<T>(T prototype) where T : ContextNode
        {
            if (root == null)
            {
                Debug.LogError("Root is not set for " + this);
                return null;
            }
            T node = (T)prototype.NewInst();
            node.UpdateFrom(prototype);
            
            node.root = root;
            node.parent = this;
            
            root.RegisterNode(node);
            
            childs.Add(node.nodeId);
            
            this.PropagateHierarchy();
            return node;
        }
        
        public virtual void PropagateHierarchy()
        {
            for (var i = 0; i < childs.Count; i++)
            {
                var child = GetChild<ContextNode>(i);
                child.root = root;
                child.parent = this;
                child.PropagateHierarchy();
            }
        }
        
        public virtual T GetChild<T>(int idx) where T : ContextNode
        {
            return root.nodeRegistry[childs[idx]] as T;
        }
        
        public ContextNode ReachInTree<T>() where T : ContextNode
        {
            var node = this;
            while (node != null && node is not T)
            {
                node = node.parent;
            }
            
            return node;
        }
    }

    public partial class ContextRoot : ContextNode
    {
        [GenIgnore]
        public Dictionary<int, object> nodeRegistry = new Dictionary<int, object>();
        public int nodeCounter = 0;

        public void RegisterNode(ContextNode node)
        {
            node.nodeId = nodeCounter++;
            nodeRegistry[node.nodeId] = node;
        }
        public override T CreateChild<T>(T prototype)
        {
             T node = (T)prototype.NewInst();
            node.UpdateFrom(prototype);
            
            node.root = this;
            node.parent = this;
            
            RegisterNode(node);
            
            childs.Add(node.nodeId);
            
            this.PropagateHierarchy();
            return node;
        }
        public override T GetChild<T>(int idx)
        {
            return nodeRegistry[childs[idx]] as T;
        }
        public override void PropagateHierarchy()
        {
            foreach (var childIdx in childs)
            {
                var child = GetChild<ContextNode>(childIdx);
                child.root = this;
                child.parent = this;
                child.PropagateHierarchy();
            }
        }
    }
}