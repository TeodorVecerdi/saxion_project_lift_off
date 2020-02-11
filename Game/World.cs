using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class World : GameObject {
        private Texture2D testTexture;
        private float tileSize;
        private int tilesHorizontal;
        private int tilesVertical;

        private int[,] tiles;
        public World(float tileSize = 64f) {
            this.tileSize = tileSize;
            tilesHorizontal = (int)(Globals.WIDTH / this.tileSize);
            tilesVertical = (int)(Globals.HEIGHT / this.tileSize);
            tiles = new int[tilesHorizontal,tilesVertical];
            
            //GENERATE WORLD
            for (int i = 0; i < tilesHorizontal; i++) {
                for (int j = 0; j < tilesVertical; j++) {
                    int precentage = Rand.Range(0, 100);
                    if (precentage <= 90) tiles[i, j] = 1; // dirt
                    else tiles[i, j] = 2; //stone
                }
            }
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
                    Tiles.Background.Texture.Bind();
                    float[] verts = {i*tileSize, j*tileSize, i*tileSize + tileSize, j*tileSize, i*tileSize + tileSize, j*tileSize + tileSize, i*tileSize, j*tileSize + tileSize};
                    glContext.DrawQuad(verts, Globals.QUAD_UV);
                    if (tiles[i, j] == 1) {        //dirt
                        Tiles.Dirt.Texture.Bind();
                        glContext.DrawQuad(verts, Globals.QUAD_UV);
                    } else if (tiles[i, j] == 2) { //stone
                        Tiles.Stone.Texture.Bind();
                        glContext.DrawQuad(verts, Globals.QUAD_UV);
                    }
                }
            }
        }
    }
}