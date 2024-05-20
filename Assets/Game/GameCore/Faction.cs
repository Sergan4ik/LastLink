using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using ZergRush;
using ZergRush.ReactiveCore;

namespace Game.GameCore
{
    public enum FactionType
    {
        Humans,
        Scavengers
    }

    public enum FactionSlot
    {
        Player1,
        Player2,
        Player3,
        Player4,
        Neutral
    }

    public partial class ControlData : RTSRuntimeData
    {
        public short serverPlayerId = -1;
        public FactionSlot factionSlot;
        public long globalPlayerId;
    }

    public partial class Faction : RTSRuntimeData, IActionSource 
    {
        public ControlData FactionControlData(GameModel gameModel) => gameModel.controlData.FirstOrDefault(cd => cd.factionSlot == slot);
        public FactionSlot slot;
        public string sourceName => $"Faction {factionType}";
        public FactionType factionType;

        public void Init(GameModel gameModel, IEnumerable<UnitConfig> unitConfigs)
        {
            foreach (var unitConfig in unitConfigs)
            {
                gameModel.CreateUnit(slot, unitConfig, 0);
            }
        }

        private void SetupActionForStack(GameModel gameModel, List<Unit> stack, Func<Unit, RTSInput, UnitAction> actionFactory, RTSInput input)
        {
            for (var i = 0; i < stack.Count; i++)
            {
                var unit = stack[i];
                unit.SetupAction(gameModel, actionFactory(unit, input), input);
            }
        }
        
        public void AutoAttackStack(GameModel gameModel, List<Unit> stack, RTSInput input)
        {
            SetupActionForStack(gameModel, stack, (unit, _) => new DefaultAttack()
            {
                duration = 2f
            }, input);
        }
        
        public void MoveStackTo(GameModel gameModel, List<Unit> stack, Vector3 destination)
        {
            if (stack.Any(u => u.factionSlot != slot))
                return;

            RTSInput inputGeneral = new RTSInput()
            {
                inputType = RTSInputType.Move,
                targetData = new TargetData()
                {
                    worldPosition = destination,
                    sourceIds = stack.Select(u => u.id).ToList()
                }
            };
            
            Vector3 centerOfMass = stack.Sum(s => s.transform.position) / stack.Count;
            
            for (var i = 0; i < stack.Count; i++)
            {
                Vector3 offset = stack[i].transform.position - centerOfMass;
                RTSInput inputForUnit = new RTSInput();
                inputForUnit.UpdateFrom(inputGeneral);
                inputForUnit.targetData.worldPosition = destination + offset;
                stack[i].MoveTo(gameModel, inputForUnit);
            }
        }
        public void Tick(GameModel gameModel, float dt)
        {
        }
    }
}