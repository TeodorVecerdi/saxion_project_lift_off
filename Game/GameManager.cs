using GXPEngine;

namespace Game {
    public class GameManager : GameObject {
        public static GameManager Instance;
        public bool ShouldShowTutorial;
        public bool ShouldStartPlaying;
        public bool ShouldStopPlaying;
        public bool ShouldShowLeaderboard;
        public bool ShouldShowMenu;
        public bool ShowedTutorial;
        public bool StartedPlaying;
        public bool StoppedPlaying;
        public bool ShowedLeaderboard;
        public bool ShowedMenu;
        private GameLoop grid;
        private StartMenu startMenu;
        private TutorialMenu tutorialMenu;
        private GameOver gameOver;
        private GameOverWin gameOverWin;
        private Leaderboard leaderboard;
        private int score;

        public GameManager() {
            startMenu = new StartMenu();
            AddChild(startMenu);
        }


        private void Update() {
            if (ShouldShowTutorial && !ShowedTutorial) {
                ShowedTutorial = true;
                ShouldShowTutorial = false;
                startMenu.Destroy();
                startMenu = null;
                tutorialMenu = new TutorialMenu();
                LateAddChild(tutorialMenu);
                ShowedMenu = false;
            }

            if (ShouldStartPlaying && !StartedPlaying) {
                StartedPlaying = true;
                ShouldStartPlaying = false;
                tutorialMenu.Destroy();
                tutorialMenu = null;
                grid = new GameLoop();
                LateAddChild(grid);
                ShowedMenu = false;
            }

            if (ShouldStopPlaying && !StoppedPlaying) {
                StoppedPlaying = true;
                ShouldStopPlaying = false;
                score = grid.Score;
                if (grid.GotTreasure) {
                    gameOverWin = new GameOverWin(score);
                    AddChild(gameOverWin);
                } else {
                    gameOver = new GameOver(score);
                    AddChild(gameOver);
                }
                grid.Destroy();
                grid = null;
                SoundManager.Instance.Stop("fuelLow");
                SoundManager.Instance.Stop("ambient");
                SoundManager.Instance.Stop("drilling");
                ShowedMenu = false;
            }

            if (ShouldShowMenu && !ShowedMenu) {
                ShouldShowMenu = false;
                ShowedMenu = true;
                ShowedTutorial = false;
                StartedPlaying = false;
                StoppedPlaying = false;
                ShowedLeaderboard = false;
                score = -1;
                gameOver?.Destroy();
                leaderboard?.Destroy();
                startMenu = new StartMenu();
                AddChild(startMenu);
            }

            if (ShouldShowLeaderboard && !ShowedLeaderboard) {
                ShouldShowLeaderboard = false;
                ShowedLeaderboard = true;
                gameOverWin.Destroy();
                leaderboard = new Leaderboard(score);
                AddChild(leaderboard);
            }
        }
    }
}