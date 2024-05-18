using System.Collections.Generic;
using Game.GameCore;
using UnityEngine;
using ZergRush.CodeGen;
using ZeroLag;

namespace Game.GameCore
{
    public partial class InputCommand : RTSCommand
    {
        public RTSInput input;
    }
    
    public partial class LogCommand : RTSCommand
    {
        public string message;
    }

    public partial class ConnectCommand : RTSCommand
    {
        public FactionSlot slot;
        public long globalPlayerId;
    }

    public partial class StartGameCommand : RTSCommand
    {
        
    }

    [GenTask(GenTaskFlags.PolymorphicDataPack), GenInLocalFolder]
    public partial class RTSCommand : ZeroLagCommand
    {
    }
}
// public partial class GeneratedCommand : RTSCommand
// {
//     public int commandId;
//     public byte[] args;
//     public int objId;
// }
