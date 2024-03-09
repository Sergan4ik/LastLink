using System.Collections.Generic;
using System.Linq;
using Game.GameCore;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class GameView
{
    private void ProcessSelectionInput((SelectionRectClipSpace rectClipSpace, float time) t)
    {
        List<Unit> toSelect = new List<Unit>();
        if (t.rectClipSpace.area < 1e-4) return;

        toSelect = game.GetUnitsInsideOpaqueQuadrangle(t.rectClipSpace, u => CanAddToCurrentSelection(u) == false);
        ProcessSelection(toSelect, null);
    }

    private (IEnumerable<Unit> newbies, IEnumerable<Unit> toDelete) GetUnionDifference(List<Unit> newSelection)
    {
        var newUnits = newSelection.Except(currentSelection.Select(uv => uv.currentUnit));
        var unitsToDelete = currentSelection.Select(uv => uv.currentUnit).Except(newSelection);
        return (newUnits, unitsToDelete);
    }

    public void ResetSelection()
    {
        ProcessSelection(null, currentSelectionModels);
    }

    private void ProcessSelection(IEnumerable<Unit> newbies, IEnumerable<Unit> toDelete)
    {
        var toDeleteCached = toDelete == null ? new List<Unit>() : toDelete.ToList();
        var newbiesCached = newbies == null ? new List<Unit>() : newbies.ToList();

        foreach (var unit in newbiesCached)
        {
            AddToSelection(unit);
        }

        foreach (var unit in toDeleteCached)
        {
            RemoveFromSelection(unit);
        }

        UpdateSelectionView();
    }

    public void AddToSelection(Unit unit)
    {
        if (CanAddToCurrentSelection(unit) == false) return;

        var view = GetViewByModel(unit);
        currentSelection.Add(view);
        view.OnSelectionToggle(true);
        UpdateSelectionView();
    }

    public void RemoveFromSelection(Unit unit)
    {
        var view = GetViewByModel(unit);
        if (view == null) return;

        currentSelection.Remove(view);
        view.OnSelectionToggle(false);
        UpdateSelectionView();
    }

    private bool CanAddToCurrentSelection(Unit unit)
    {
        return unit != null && (currentSelection.Count == 0 ||
                                (unit.factionSlot == currentSelectionModels.First().factionSlot &&
                                 currentSelectionModels.All(m => m != unit)));
    }

    private void UpdateSelectionView()
    {
        gameUI.selectionUI.ShowSelection(currentSelection);
    }

    private void OnSecondaryAction(InputAction.CallbackContext ctx)
    {
        if (game != null && game.gameState.value == GameState.InProgress)
        {
            if (cameraController.TryGetWorldMousePosition(out var worldMousePosition) == false) return;
            if (cameraController.TryGetPointedUnit(out var unit))
            {
                unit.SetupAction(game, new DOT()
                {
                    duration = 3,
                });
            }
            else
            {
                OnTerrainClick();
                if (currentSelection.Count > 0)
                    localPlayerFaction.MoveStackTo(game, currentSelection.Select(uv => uv.currentUnit).ToList(),
                        worldMousePosition);
            }
        }
    }
}