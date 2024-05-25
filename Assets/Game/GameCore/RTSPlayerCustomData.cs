using System;
using UnityEngine;
using ZergRush.CodeGen;
using ZeroLag.MultiplayerTools.Modules.Database;

[GenInLocalFolder, GenTask(GenTaskFlags.SimpleDataPack)]
public partial class RTSPlayerCustomData : CustomPlayerData
{
    public string playerAvatar;
    public int level;
}

public class RTSPlayerData : PlayerData<RTSPlayerCustomData>{}