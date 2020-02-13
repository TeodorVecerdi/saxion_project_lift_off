using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class World : GameObject {
        public int VerticalTiles = 400;
        private TileGrid grid;
        private Sprite topBackground;
        private Sprite fuelStation;
        private Sprite gameOver;
        

        public World() {
            topBackground = new Sprite("data/background_test.jpg", true, false);
            topBackground.Move(0, -2*Globals.TILE_SIZE);
            topBackground.SetScaleXY(0.711458333f);
            AddChild(topBackground);

            

            fuelStation = new Sprite("data/fuel_station.png", true, false);
            fuelStation.Move(0, 2 * Globals.TILE_SIZE);
            AddChild(fuelStation);
            
            grid = new TileGrid(VerticalTiles);
            AddChild(grid);


            gameOver = new Sprite("data/gameover.png", true, false);
            gameOver.SetScaleXY(2.732f, 2.85f);
            gameOver.visible = false;
            AddChild(gameOver);
        }
        void Update()
        {
            if (grid.FuelBar.FuelAmount <= 0)

            {

                gameOver.visible = true;

            }
        }
    }

}