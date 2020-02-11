using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class TileGrid : GameObject {
        private int topOffset = 6;

        private int tilesHorizontal;
        private int tilesVertical;

        private int[,] tiles;
        private int[,] tilesBackground;

        public TileGrid(int tilesVertical) {
            tilesHorizontal = (int) (Globals.WIDTH / Globals.TILE_SIZE);
            this.tilesVertical = tilesVertical;
            tiles = new int[tilesHorizontal, tilesVertical];
            tilesBackground = new int[tilesHorizontal, tilesVertical];

            //GENERATE WORLD
            for (int i = 0; i < tilesHorizontal; i++) {
                for (int j = topOffset; j < tilesVertical; j++) {
                    tilesBackground[i, j] = 4;
                    int precentage = Rand.Range(0, 100);
                    if (precentage <= 90) tiles[i, j] = 1; // dirt
                    else tiles[i, j] = 2;                  // stone
                }
            }

            for (int x = 0; x < 4; x++) {
                tiles[x, topOffset] = 2;
            }

            int spawnLocation = Rand.Range(6, tilesHorizontal - 1);
            tiles[spawnLocation, topOffset - 1] = 3; // player
        }

        void Update() {
            // Remove a random tile
            if (Input.GetKeyDown(Key.SPACE)) {
                int x = Rand.Range(0, tilesHorizontal);
                int y = Rand.Range(0, tilesVertical);
                tiles[x, y] = 0;
            }
        }

        protected override void RenderSelf(GLContext glContext) {
            glContext.SetColor(0xff, 0xff, 0xff, 0xff);
            for (int i = 0; i < tilesHorizontal; i++) {
                for (int j = 0; j < tilesVertical; j++) {
                    float[] verts = {i * Globals.TILE_SIZE, j * Globals.TILE_SIZE, i * Globals.TILE_SIZE + Globals.TILE_SIZE, j * Globals.TILE_SIZE, i * Globals.TILE_SIZE + Globals.TILE_SIZE, j * Globals.TILE_SIZE + Globals.TILE_SIZE, i * Globals.TILE_SIZE, j * Globals.TILE_SIZE + Globals.TILE_SIZE};
                    Tiles.IdToTile[tilesBackground[i, j]].Texture.Bind();
                    glContext.DrawQuad(verts, Globals.QUAD_UV);
                    Tiles.IdToTile[tiles[i, j]].Texture.Bind();
                    glContext.DrawQuad(verts, Globals.QUAD_UV);
                }
            }
        }
    }
}