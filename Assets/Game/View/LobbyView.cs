using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameCore;
using Game.GameCore.GameControllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZergRush;
using ZergRush.CodeGen;
using ZergRush.ReactiveCore;
using ZergRush.ReactiveUI;

public class LobbyView : ConnectableMonoBehaviour
{
    public static LobbyView instance;
    public RectTransform panel;
    public ReactiveScrollRect playersRect;
    
    public Button setReadyButton;

    public Connections showConnections = new Connections();
    private Func<GameModel> gmGetter;
    public GameModel gmOnInit;
    
    private ReactiveCollectionImitator<ControlData,ControlData> playerPresenter;
    public short serverPlayerId => GameSession.instance.clientController.serverPlayerId;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        instance = null;
    }

    public void Show(Func<GameModel> modelGetter)
    {
        showConnections.DisconnectAll();
        gmGetter = modelGetter;
        gmOnInit = modelGetter();

        panel.SetActiveSafe(true);
        playerPresenter = new ReactiveCollectionImitator<ControlData, ControlData>(gmGetter().controlData);

        PlayerLobbyView localPlayerView = null;
        
        showConnections += playerPresenter.data.PresentInScrollWithLayout(playersRect,
            PrefabRef<PlayerLobbyView>.Auto(), (data, view) =>
            {
                var playerData = Tools.GetGlobalPlayerDatabase().GetPlayer(data.globalPlayerId);
                view.Show(modelGetter, playerData, modelGetter().GetControlDataByServerPlayerId(serverPlayerId).globalPlayerId);
                if (data.serverPlayerId != serverPlayerId) return;

                localPlayerView = view;
                view.connections += view.factionSlotDropdown.ChangeStream().MergeWith(view.factionTypeDropdown.ChangeStream()).Subscribe(UpdateLobbyPlayer);

                void UpdateLobbyPlayer()
                {
                    GameSession.instance.SendRTSCommand(new UpdateLobbyPlayerCommand()
                    {
                        globalPlayerId = data.globalPlayerId,
                        factionType = (FactionType) Enum.Parse(typeof(FactionType), view.factionTypeDropdown.options[view.factionTypeDropdown.value].text),
                        factionSlot = (FactionSlot) Enum.Parse(typeof(FactionSlot), view.factionSlotDropdown.options[view.factionSlotDropdown.value].text)
                    });
                }
            });

        
        showConnections += setReadyButton.Subscribe(() =>
        {
            var cd = modelGetter().GetControlDataByServerPlayerId(serverPlayerId);
            GameSession.instance.SendRTSCommand(new SetReadyCommand()
            {
                globalPlayerId = cd.globalPlayerId,
                factionType = (FactionType) Enum.Parse(typeof(FactionType), localPlayerView.factionTypeDropdown.options[localPlayerView.factionTypeDropdown.value].text),
                factionSlot = (FactionSlot) Enum.Parse(typeof(FactionSlot), localPlayerView.factionSlotDropdown.options[localPlayerView.factionSlotDropdown.value].text)
            });
        });
    }

    private void Update()
    {
        if (gmGetter?.Invoke() == null) return;
        
        playerPresenter.UpdateFrom(gmGetter().controlData);
        
        if (panel.gameObject.activeSelf && gmGetter().gameState.value != GameState.NotStarted)
            Hide();
    }

    public void Hide()
    {
        showConnections.DisconnectAll();
        panel.SetActiveSafe(false);
    }

}