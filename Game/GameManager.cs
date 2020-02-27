using GXPEngine;

namespace Game {
    public class GameManager : GameObject {
        public bool ShouldShowTutorial;
        public bool ShouldStartPlaying;
        public bool ShouldStopPlaying;
        private bool showedTutorial;
        private bool startedPlaying;
        private bool stoppedPlaying;
        private GameLoop grid;
        private StartMenu startMenu;
        private TutorialMenu tutorialMenu;
        private GameOver gameOver;

        public GameManager() {
            startMenu = new StartMenu(this);
            AddChild(startMenu);
        }

        private void Update() {
            if (ShouldShowTutorial && !showedTutorial) {
                showedTutorial = true;
                ShouldShowTutorial = false;
                startMenu.Destroy();
                tutorialMenu = new TutorialMenu(this);
                LateAddChild(tutorialMenu);
            }
            
            if (ShouldStartPlaying && !startedPlaying) {
                startedPlaying = true;
                ShouldStartPlaying = false;
                tutorialMenu.Destroy();
                grid = new GameLoop(this);
                LateAddChild(grid);
            }

            if (ShouldStopPlaying && !stoppedPlaying) {
                SoundManager.Instance.Stop("fuelLow");
                SoundManager.Instance.Stop("ambient");
                SoundManager.Instance.Stop("drilling");
                stoppedPlaying = true;
                ShouldStopPlaying = false;
                gameOver = new GameOver(grid.Score);
                grid.Destroy();
                AddChild(gameOver);
            }
        }
    }

}