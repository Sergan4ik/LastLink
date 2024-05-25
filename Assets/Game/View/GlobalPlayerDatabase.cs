using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalPlayerDatabase", menuName = "Game/GlobalPlayerDatabase")]
public class GlobalPlayerDatabase : ScriptableObject
{
    public List<RTSPlayerData> players;
    
    public RTSPlayerData GetPlayer(long globalPlayerId)
    {
        return players.FirstOrDefault(p => p.playerId == globalPlayerId);
    }
}