using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class World : GameObject {
        private GameLoop grid;
        public GameOver GameOver;
        private Sprite topBackground;
        // private Sprite fuelStation;
     
        public World() {
            topBackground = new Sprite("data/background_test.jpg", true, false);
            topBackground.Move(0, -2*Globals.TILE_SIZE);
            topBackground.SetScaleXY(0.711458333f);
            
            grid = new GameLoop(this);
            GameOver = new GameOver(grid);
            AddChild(topBackground);
            AddChild(grid);
            AddChild(GameOver);
        }
    }

}