using System.Linq;
using UnityEngine;

namespace Game.GameCore
{
    public partial class GameModel
    {
        public void SetReady(SetReadyCommand cmd)
        {
            if (gameState.value != GameState.NotStarted) return;

            var cd = controlData.FirstOrDefault(c => c.globalPlayerId == cmd.globalPlayerId);
            if (cd == default)
            {
                Debug.LogError($"Player with id {cmd.globalPlayerId} is not connected");
                return;
            }

            cd.factionSlot = cmd.factionSlot;
            GetFactionBySlot(cmd.factionSlot).factionType = cmd.factionType;

            if (readyPlayers.Exists(ready => ready == cmd.serverPlayerId) == false)
                readyPlayers.Add(cd.serverPlayerId);

            CheckPlayersReadiness();
        }

        private void CancelReady(CancelReadyCommand cancelReadyCommand)
        {
            if (gameState.value != GameState.NotStarted) return;

            var cd = controlData.FirstOrDefault(c => c.globalPlayerId == cancelReadyCommand.globalPlayerId);
            if (cd == default)
            {
                Debug.LogError($"Player with id {cancelReadyCommand.globalPlayerId} is not connected");
                return;
            }

            readyPlayers.Remove(cd.serverPlayerId);
        }

        private void UpdateLobbyPlayer(UpdateLobbyPlayerCommand cmd)
        {
            if (gameState.value != GameState.NotStarted) return;

            var cd = controlData.FirstOrDefault(c => c.globalPlayerId == cmd.globalPlayerId);
            if (cd == default)
            {
                Debug.LogError($"Player with id {cmd.globalPlayerId} is not connected");
                return;
            }

            cd.factionSlot = cmd.factionSlot;
            GetFactionBySlot(cmd.factionSlot).factionType = cmd.factionType;
        }

        private void CheckPlayersReadiness()
        {
            if (controlData.Count(cd => cd.serverPlayerId != -1) == readyPlayers.Count)
            {
                GameStart();
            }
        }
    }
}