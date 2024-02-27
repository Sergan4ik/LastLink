using System.Collections.Generic;
using Game.GameCore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZergRush;
using ZergRush.ReactiveCore;
using ZergRush.ReactiveUI;

namespace Game
{
    public class SelectionUI : RTSView
    {
        public Image mainIcon;
        public RectTransform otherSelectedParent;
        public ProgressBar hpBar;
        public ProgressBar manaBar;
        public TextMeshProUGUI unitName;

        public Connections showConnections = new Connections();
        
        public List<UnitView> cachedSelection = new List<UnitView>();
        public Unit mainUnit => cachedSelection.Count > 0 ? cachedSelection[0].currentUnit : null;
        public TableConnectionsAndComponents<SelectedUnitViewUI, UnitView> selectionPresenter = null;
        public void ShowSelection(List<UnitView> units)
        {
            showConnections?.DisconnectAll();
            if (units == null || units.Count == 0)
            {
                ResetSelectionUI();
                return;
            }
            
            var firstUnit = units[0];
            mainIcon.sprite = firstUnit.currentUnit.cfg.name.GetUnitIcon();
            hpBar.gameObject.SetActive(true);
            manaBar.gameObject.SetActive(true);
            unitName.text = firstUnit.currentUnit.cfg.name; 
            
            selectionPresenter = units.ToStaticReactiveCollection().PresentWithLayout(otherSelectedParent,
                PrefabRef<SelectedUnitViewUI>.Auto(),
                (view, ui) =>
                {
                    ui.Show(view.currentUnit);
                }, Rui.GridLayout(2, LayoutDirection.Horizontal, 5, 5));

            showConnections += selectionPresenter;
            
            cachedSelection = units;
        }

        private void ResetSelectionUI()
        {
            cachedSelection = new List<UnitView>();
            mainIcon.sprite = null;
            hpBar.gameObject.SetActive(false);
            manaBar.gameObject.SetActive(false);
            unitName.text = "";
        }

        public void Update()
        {
            if (cachedSelection.Count <= 0) return;
         
            foreach (var selectedUnitViewUI in selectionPresenter.viewLoader.Views())
            {
                selectedUnitViewUI.hpBar.SetProgress(selectedUnitViewUI.shownUnit.hp, selectedUnitViewUI.shownUnit.maxHp, "0");
            }
            
            hpBar.SetProgress(mainUnit.hp, mainUnit.maxHp);
            manaBar.SetProgress(mainUnit.stats.Mana, mainUnit.stats.MaxMana);
        }
    }
}