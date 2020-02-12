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
            topBackground.SetScaleXY(0.711458333f);
            AddChild(topBackground);
            
            fuelStation = new Sprite("data/fuel_station.png", true, false);
            fuelStation.Move(0, 2 * Globals.TILE_SIZE);
            AddChild(fuelStation);
            
            grid = new TileGrid(400);
            AddChild(grid);
        }
    }
}