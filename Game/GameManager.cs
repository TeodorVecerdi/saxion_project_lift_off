using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class GameManager : GameObject {
        public bool ShouldStartPlaying;
        public bool ShouldStopPlaying;
        private GameLoop grid;
        private StartMenu menu;
        private GameOver gameOver;

        public GameManager() {
            menu = new StartMenu(this);
            AddChild(menu);
        }

        private void Update() {
            if (ShouldStartPlaying) {
                ShouldStartPlaying = false;
                menu.Destroy();
                grid = new GameLoop(this);
                LateAddChild(grid);
            }

            if (ShouldStopPlaying) {
                ShouldStopPlaying = false;
                gameOver = new GameOver(grid.Score);
                grid.Destroy();
                LateAddChild(gameOver);
            }
        }
    }

}