using Game.GameCore;
using UnityEngine;
using UnityEngine.Serialization;
using ZergRush.ReactiveUI;

public class UnitView : RTSView, ISimpleUpdatableFrom<Unit>
{
    public Unit currentUnit;
    public GameObject selectionVFX;
    public ProgressBar hpBar;
    public void ShowUnit(Unit unit)
    {
        currentUnit = unit;
    }
    
    public void OnSelectionToggle(bool isSelected)
    {
        selectionVFX.SetActive(isSelected);
    }

    public void UpdateFrom(Unit unit)
    {
        ShowUnit(unit);
        //var offsetPos = .CubeCoordToOffsetCoord();

        transform.position = unit.transform.position;
        transform.rotation = unit.transform.rotation;

        hpBar.ChangeProgress(unit.hp, unit.maxHp);        
        
        unit.view = this;
    }

    public float OnUnload()
    {
        return 0;
    }
}