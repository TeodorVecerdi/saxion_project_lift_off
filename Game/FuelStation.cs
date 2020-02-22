using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class FuelStation : GameObject {
        private Texture2D fuelStationTexture;
        private int refillPointX, refillPointY;
        private int refillsLeft;

        public FuelStation(string texturePath, int refillPointX, int refillPointY, int maxRefills) {
            fuelStationTexture = Texture2D.GetInstance(texturePath, true);
            this.refillPointX = refillPointX;
            this.refillPointY = refillPointY;
            refillsLeft = maxRefills;
        }
        public bool IsPlayerOnRefillPoint(int playerX, int playerY) => playerX == refillPointX && playerY == refillPointY;
        public bool CanPlayerRefill() => refillsLeft > 0;
        public void ReduceRefillsLeft() => refillsLeft--;

        public void Draw(GLContext glContext) {
            float[] verts = {x, y, x+fuelStationTexture.width, y, x+fuelStationTexture.width, y+fuelStationTexture.height, x, y+fuelStationTexture.height};
            fuelStationTexture.Bind();
            glContext.DrawQuad(verts, Globals.QUAD_UV);
        }
    }
}