using System;
using System.Linq;
using Game.GameCore;
using Game.GameCore.GameControllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZergRush.ReactiveCore;
using ZergRush.ReactiveUI;

public class PlayerLobbyView : ReusableView
{
    public Image playerAvatar;
    public TextMeshProUGUI playerName;
    public TMP_Dropdown factionTypeDropdown;
    public TMP_Dropdown factionSlotDropdown;

    public TextMeshProUGUI readyStatus;
    
    public Func<GameModel> shownModelGetter;
    public GlobalPlayerData shownData;
    public short serverPlayerId => shownModelGetter().GetControlDataByGlobalPlayerId(shownData.globalPlayerId).serverPlayerId;
    public void Show(Func<GameModel> modelGetter, GlobalPlayerData player, long localGlobalPlayerId)
    {
        shownModelGetter = modelGetter;
        shownData = player;
        
        playerAvatar.sprite = player.playerAvatar;
        playerName.text = player.playerName;

        var factionTypes = Enum.GetNames(typeof(FactionType)).ToList();
        if (factionTypeDropdown.options.Count != factionTypes.Count || factionTypeDropdown.options.Any(o => factionTypes.Contains(o.text) == false))
        {
            factionTypeDropdown.ClearOptions();
            factionTypeDropdown.AddOptions(factionTypes);
        }
        
        var factionSlots = Enum.GetNames(typeof(FactionSlot)).ToList();
        if (factionSlotDropdown.options.Count != factionSlots.Count || factionSlotDropdown.options.Any(o => factionSlots.Contains(o.text) == false))
        {
            factionSlotDropdown.ClearOptions();
            factionSlotDropdown.AddOptions(factionSlots);
        }
        
        factionSlotDropdown.interactable = player.globalPlayerId == localGlobalPlayerId;
        factionTypeDropdown.interactable = player.globalPlayerId == localGlobalPlayerId;
        
        factionTypeDropdown.value = factionTypeDropdown.options.FindIndex(o => o.text == shownModelGetter().GetFactionByServerPlayerId(serverPlayerId).factionType.ToString());
        factionSlotDropdown.value = factionSlotDropdown.options.FindIndex(o => o.text == shownModelGetter().GetControlDataByGlobalPlayerId(shownData.globalPlayerId).factionSlot.ToString());
    }

    private void Update()
    {
        if (shownModelGetter?.Invoke() == null) return;
        if (shownData == null) return;
        
        //if changed slot reassign
        if (serverPlayerId != GameSession.instance.clientController?.serverPlayerId)
        {
            var slot = shownModelGetter().GetControlDataByGlobalPlayerId(shownData.globalPlayerId).factionSlot.ToString();
            if (factionSlotDropdown.options[factionSlotDropdown.value].text != slot)
            {
                factionSlotDropdown.value = factionSlotDropdown.options.FindIndex(o => o.text == slot);
            }

            var type = shownModelGetter().GetFactionByServerPlayerId(serverPlayerId).factionType.ToString();
            if (factionTypeDropdown.options[factionTypeDropdown.value].text != type) 
            {
                factionTypeDropdown.value = factionTypeDropdown.options.FindIndex(o => o.text == type);
            }
        }

        var isReady = shownModelGetter().readyPlayers.Any(p => p == serverPlayerId);
        readyStatus.text = isReady ? "Ready" : "Not ready";
        readyStatus.color = isReady ? Color.green : Color.red;
    }
}