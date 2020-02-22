using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class World : GameObject {
        private GameLoop grid;
        private Sprite topBackground;
        // private Sprite fuelStation;
     
        public World() {
            topBackground = new Sprite("data/background_test.jpg", true, false);
            topBackground.Move(0, -2*Globals.TILE_SIZE);
            topBackground.SetScaleXY(0.711458333f);
            grid = new GameLoop();
            
            AddChild(topBackground);
            // AddChild(fuelStation);
            AddChild(grid);
        }
    }

}