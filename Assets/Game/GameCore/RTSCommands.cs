using System.Collections.Generic;
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

    public partial class StartGameCommand : RTSCommand
    {
        
    }

    [GenTask(GenTaskFlags.PolymorphicDataPack), GenInLocalFolder]
    public partial class RTSCommand : ZeroLagCommand
    {
    }
}