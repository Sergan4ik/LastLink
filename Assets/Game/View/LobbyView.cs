using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameCore;
using Game.GameCore.GameControllers;
using GameKit.Unity.UnityNetork;
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

    public TMP_InputField joinCode;
    
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

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        instance = null;
    }

    public async void Show(Func<GameModel> modelGetter)
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

        string code = "";
        if (GameSession.instance.clientTransport != null)
        {
            code = await GameSession.instance.clientTransport.GetJoinCode();
        }
        if (GameSession.instance.serverTransport != null)
        {
            code = await GameSession.instance.serverTransport.GetJoinCode();
        }
        
        joinCode.text = code == "" ? "Use relay" : code;
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