using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class World : GameObject {
        private TileGrid grid;
        private Sprite topBackground;
        private Sprite fuelStation;

        public World() {
            topBackground = new Sprite("data/background_test.jpg", true, false);
            topBackground.Move(0, -2*Globals.TILE_SIZE);
            AddChild(topBackground);
            
            grid = new TileGrid(50);
            AddChild(grid);
            
            fuelStation = new Sprite("data/fuelstation.png", true, false);
            fuelStation.Move(0, 2 * Globals.TILE_SIZE);
            AddChild(fuelStation);
        }
    }
}