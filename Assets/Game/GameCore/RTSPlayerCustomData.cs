using System;
using UnityEngine;
using ZeroLag.MultiplayerTools.Modules.Database;

[Serializable]
public class GlobalPlayerData : CustomPlayerData
{
    public string playerAvatar;
    public int level;
}