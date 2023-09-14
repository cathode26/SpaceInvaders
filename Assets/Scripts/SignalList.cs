using deVoid.Utils;
using System;
using System.Collections.Generic;

namespace SpaceInvaders
{
    namespace Project.Game
    {
        public class AliensSpawnedSignal : ASignal<AlienSpawner.SpawnedAliens> { }
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

    namespace Project.SceneManager
    {
        public class PlaySignal : ASignal { }
        public class HighScoreSignal : ASignal { }
        public class MainMenuSignal : ASignal { }
        public class QuitSignal : ASignal { }
    }

    namespace Project.UI
    {
        public class GetCoinsNumberSignal : ASignal<Action<long>> { }
        public class LoadStickySymbolsSignal : ASignal<string[]> { }
    }
}

