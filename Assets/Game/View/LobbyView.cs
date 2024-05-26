using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameCore;
using Game.GameCore.GameControllers;
using GameKit.Unity.UnityNetork;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
    
    public Button readyButton;
    public TextMeshProUGUI readyButtonText;

    public Connections showConnections = new Connections();
    private Func<GameModel> gmGetter;
    public GameModel gmOnInit;
    
    private ReactiveCollectionImitator<ControlData,ControlData> playerPresenter;
    public short serverPlayerId => GameSession.instance.clientController.serverPlayerId;
    public bool isReady
    {
        get
        {
            if (gmGetter == null) return false;
            if (gmGetter().readyPlayers.Count == 0) return false;
            
            if (lastMyIndex != -1 && gmGetter().readyPlayers.AtSafe(lastMyIndex, short.MaxValue) == serverPlayerId) return true;
            lastMyIndex = gmGetter().readyPlayers.IndexOf(serverPlayerId);
            return lastMyIndex != -1;
        }
    }

    private int lastMyIndex = -1;
    
    private Cell<int> stateButtonCell = new Cell<int>(0);
    private StateButton stateButton;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;

        stateButton = readyButton.SetupButtonStates((b, label, i) =>
        {
            if (i == 0)
            {
                var block = b.colors;
                block.normalColor = Color.green;
                b.colors = block;
                label.text = "I am ready";
            }
            else if (i == 1)
            {
                var block = b.colors;
                block.normalColor = Color.red;
                b.colors = block;
                label.text = "Cancel ready";
            }
        }, readyButtonText, stateButtonCell);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        instance = null;
    }

    public async void Show(Func<GameModel> modelGetter)
    {
        panel.SetActiveSafe(true);
        
        showConnections.DisconnectAll();
        gmGetter = modelGetter;
        gmOnInit = modelGetter();

        panel.SetActiveSafe(true);
        playerPresenter = new ReactiveCollectionImitator<ControlData, ControlData>(gmGetter().controlData);

        PlayerLobbyView localPlayerView = null;
        
        showConnections += playerPresenter.data.PresentInScrollWithLayout(playersRect,
            PrefabRef<PlayerLobbyView>.Auto(), async (data, view) =>
            {
                await view.Show(modelGetter, data.globalPlayerId, modelGetter().GetControlDataByServerPlayerId(serverPlayerId).globalPlayerId);
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

        
        showConnections += readyButton.Subscribe(() =>
        {
            var cd = modelGetter().GetControlDataByServerPlayerId(serverPlayerId);
            if (isReady == false)
            {
                GameSession.instance.SendRTSCommand(new SetReadyCommand()
                {
                    globalPlayerId = cd.globalPlayerId,
                    factionType = (FactionType)Enum.Parse(typeof(FactionType),
                        localPlayerView.factionTypeDropdown.options[localPlayerView.factionTypeDropdown.value].text),
                    factionSlot = (FactionSlot)Enum.Parse(typeof(FactionSlot),
                        localPlayerView.factionSlotDropdown.options[localPlayerView.factionSlotDropdown.value].text)
                });
            }
            else
            {
                GameSession.instance.SendRTSCommand(new CancelReadyCommand()
                {
                    globalPlayerId = cd.globalPlayerId
                });
            }
        });

        string code = "";
        if (GameSession.instance.clientTransport != null)
        {
            code = await GameSession.instance.clientTransport.GetJoinCode();
        }
        else if (GameSession.instance.serverTransport != null)
        {
            code = await GameSession.instance.serverTransport.GetJoinCode();
        }
        
        joinCode.text = code == "" ? "Use relay" : code;
    }

    private void Update()
    {
        if (gmGetter?.Invoke() == null) return;
        
        playerPresenter.UpdateFrom(gmGetter().controlData);
        
        stateButtonCell.value = isReady ? 1 : 0;

        if (panel.gameObject.activeSelf && gmGetter().gameState.value != GameState.NotStarted)
            Hide();
    }

    public void Hide()
    {
        showConnections.DisconnectAll();
        panel.SetActiveSafe(false);
    }
}