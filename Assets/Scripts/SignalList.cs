using deVoid.Utils;

namespace SpaceInvaders
{
    namespace Project.Game
    {
        public class AliensSpawnedSignal : ASignal<AlienSpawner.SpawnedAliens> { }
        public class MoveAlienSignal : ASignal { }
        public class MoveAlienCompletedSignal : ASignal { }
        public class DirectionReversedSignal : ASignal { }
        public class AlienReachedBoundarySignal : ASignal<Boundary> { }
        public class NoMoreLivesSignal : ASignal { }
        public class LoadGameSignal : ASignal { }
        public class ResetGameSignal : ASignal { }
    }

    namespace Project.SceneManager
    {
        public class LoadGameSignal : ASignal { }
        public class HighScoreSignal : ASignal { }
        public class MainMenuSignal : ASignal { }
        public class GamePausedSignal : ASignal<bool> { }
        public class ResetGameSignal : ASignal { }
    }

    namespace Project.HighScores
    {
        public class OnBackPressedSignal : ASignal { }
    }
    namespace Project.MainMenu
    {
        public class PlaySignal : ASignal { }
        public class QuitSignal : ASignal { }
        public class OnHighScoresPressedSignal : ASignal { }
    }
}

