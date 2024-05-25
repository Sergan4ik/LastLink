using System;
using System.Linq;
using System.Threading.Tasks;
using Game.GameCore;
using Game.GameCore.GameControllers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
    public RTSPlayerData shownCustomData;
    public short serverPlayerId => shownModelGetter().GetControlDataByGlobalPlayerId(shownCustomData.playerId).serverPlayerId;
    public async Task Show(Func<GameModel> modelGetter, long playerId, long localGlobalPlayerId)
    {
        shownModelGetter = modelGetter;
        
        SetupDropdowns(playerId,localGlobalPlayerId);
        
        shownCustomData = await GameSession.instance.playerDatabase.GetPlayer(playerId);
        
        playerAvatar.sprite = shownCustomData.customData.playerAvatar.GetPlayerAvatar();
        playerName.text = shownCustomData.username;
    }

    private void SetupDropdowns(long playerId, long localGlobalPlayerId)
    {
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
        
        factionSlotDropdown.interactable = playerId == localGlobalPlayerId;
        factionTypeDropdown.interactable = playerId == localGlobalPlayerId;
        
        factionTypeDropdown.value = factionTypeDropdown.options.FindIndex(o => o.text == shownModelGetter().GetFactionByGlobalId(playerId).factionType.ToString());
        factionSlotDropdown.value = factionSlotDropdown.options.FindIndex(o => o.text == shownModelGetter().GetControlDataByGlobalPlayerId(playerId).factionSlot.ToString());
    }

    private void Update()
    {
        if (shownModelGetter?.Invoke() == null) return;
        if (shownCustomData == null) return;
        
        //if changed slot reassign
        if (serverPlayerId != GameSession.instance.clientController?.serverPlayerId)
        {
            var slot = shownModelGetter().GetControlDataByGlobalPlayerId(shownCustomData.playerId).factionSlot.ToString();
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