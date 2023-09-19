using deVoid.Utils;

namespace SpaceInvaders
{
    namespace Project.Game
    {
        public class AliensSpawnedSignal : ASignal<AlienSpawner.SpawnedAliens> { }
        public class MoveAlienSignal : ASignal { }
        public class MoveAlienCompletedSignal : ASignal { }
        public class DirectionReversedSignal : ASignal { }
        public class SetSpeedSignal : ASignal<float> { }
        public class AlienReachedBoundarySignal : ASignal<Boundary> { }
        public class NoMoreLivesSignal : ASignal { }
        public class LoadGameSignal : ASignal { }
        public class ResetGameSignal : ASignal { }
        public class AlienKilledSignal : ASignal<Alien> { }
        public class UFOKilledSignal : ASignal<UFOAlien> { }
        public class ScoreUpdatedSignal : ASignal<int> { }
        public class HighScoreUpdatedSignal : ASignal<int> { }
        public class LivesChangedSignal : ASignal<int> { }
        public class LoadNextLevelSignal : ASignal { }
    }

    namespace Project.SceneManager
    {
        public class LoadGameSignal : ASignal { }
        public class HighScoreSignal : ASignal { }
        public class MainMenuSignal : ASignal { }
        public class GamePausedSignal : ASignal<bool> { }
        public class ResetGameSignal : ASignal { }
        public class OnResetGameCompleteSignal : ASignal { }
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
    namespace Project.Input
    {
        public class OnEnableEscapeSignal : ASignal<bool> { }
    }
}

