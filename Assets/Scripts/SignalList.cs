using deVoid.Utils;
using System;
using System.Collections.Generic;


namespace Project.Game
{
    public class MoveAlienSignal : ASignal { }
    public class MoveAlienCompletedSignal : ASignal { }
    public class DirectionReversedSignal : ASignal { }
    public class OnAlienReachedBoundarySignal : ASignal<Boundary> { }
}

namespace Project.Audio
{
    public class SymbolSoundsSignal : ASignal<List<string>, float> { }
    public class StopSymbolSoundsSignal : ASignal { }
    public class SetSpinModeSignal : ASignal<string> { }
}

namespace Project.ModeManager
{
    public class ManageSpinModeSignal : ASignal<string> { }
    public class SetSpinModeSignal : ASignal<string> { }
    public class SetPopupModeSignal : ASignal<bool> { }
}

namespace Project.UI
{
    public class GetCoinsNumberSignal : ASignal<Action<long>> { }
    public class LoadStickySymbolsSignal : ASignal<string[]> { }
}


