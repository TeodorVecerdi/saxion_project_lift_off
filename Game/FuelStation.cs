using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class FuelStation : GameObject {
        private Texture2D fuelStationTexture;
        private FuelStationFuelBar fuelStationFuelBar;
        private int refillPointX, refillPointY;
        private float fuelAmount;
        private float fuelCapacity;

        public float FuelAmount {
            get => fuelAmount;
            set => fuelAmount = value;
        }
        
        public float FuelCapacity {
            get => fuelCapacity;
            set => fuelCapacity = value;
        }

        public FuelStation(string texturePath, int refillPointX, int refillPointY, float fuelCapacity) {
            fuelStationTexture = Texture2D.GetInstance(texturePath, true);
            this.refillPointX = refillPointX;
            this.refillPointY = refillPointY;
            this.fuelCapacity = fuelCapacity;
            fuelAmount = fuelCapacity;
            fuelStationFuelBar = new FuelStationFuelBar(this);
            AddChild(fuelStationFuelBar);
        }
        
        public bool IsPlayerOnRefillPoint(int playerX, int playerY) => playerX == refillPointX && playerY == refillPointY;
        public void Refuel(FuelBar fuelBar) {
            var requiredFuel = fuelBar.FuelCapacity - fuelBar.FuelAmount;
            var availableFuel = fuelAmount;
            if (availableFuel > requiredFuel) availableFuel = requiredFuel;
            fuelBar.FuelAmount += availableFuel;
            fuelAmount -= availableFuel;
            Debug.Log(availableFuel);
        }

        public void Draw(GLContext glContext) {
            float[] verts = {x, y, x+fuelStationTexture.width, y, x+fuelStationTexture.width, y+fuelStationTexture.height, x, y+fuelStationTexture.height};
            fuelStationTexture.Bind();
            glContext.DrawQuad(verts, Globals.QUAD_UV);
            fuelStationFuelBar.Draw(glContext);
        }
    }
}