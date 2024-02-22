using Game.GameCore;
using UnityEngine;
using ZergRush.ReactiveUI;

public class UnitView : RTSView, ISimpleUpdatableFrom<Unit>
{
    public Unit currentUnit;
    public GameObject selectionVFX;
    public void ShowUnit(Unit unit)
    {
        
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

        unit.view = this;
    }

    public float OnUnload()
    {
        return 0;
    }
}