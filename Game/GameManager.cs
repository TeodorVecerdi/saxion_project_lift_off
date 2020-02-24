using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class GameManager : GameObject {
        public bool ShouldStartPlaying = false;
        public bool ShouldStopPlaying = false;
        private GameLoop grid;
        private StartMenu menu;

        public GameManager() {
            menu = new StartMenu(this);
            AddChild(menu);
        }

        private void Update() {
            if (ShouldStartPlaying) {
                ShouldStartPlaying = false;
                menu.Destroy();
                grid = new GameLoop();
                LateAddChild(grid);
            }
        }
    }

}