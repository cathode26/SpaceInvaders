using deVoid.Utils;

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

    namespace Project.SceneManager
    {
        public class PlaySignal : ASignal { }
        public class HighScoreSignal : ASignal { }
        public class MainMenuSignal : ASignal { }
        public class QuitSignal : ASignal { }
    }

    namespace Project.HighScores
    {
        public class OnBackPressedSignal : ASignal { }
    }
    namespace Project.MainMenu
    {
        public class OnHighScoresPressedSignal : ASignal { }
    }
}

