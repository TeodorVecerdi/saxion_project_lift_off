using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class FuelBar : GameObject {
        private Texture2D border, fuel, background;
        private float fuelAmount = 100000f; // in mL
        private float fuelCapacity = 100000f; // in mL

        public FuelBar() {
            border = Texture2D.GetInstance("data/fuelbar_border.png");
            fuel = Texture2D.GetInstance("data/fuelbar_fuel.png");
            background = Texture2D.GetInstance("data/fuelbar_background.png");
        }

        public void ChangeFuel(float amount) => fuelAmount += amount;
        public void SetFuelCapacity(float amount) => fuelCapacity = amount;
        public void Refuel() => fuelAmount = fuelCapacity;

        public float FuelAmount {
            get => fuelAmount;
            set => fuelAmount = value;
        }


        protected override void RenderSelf(GLContext glContext) {
            float[] verts = {1288f, -384F, 1366f, -384F, 1366f, 384F, 1288f, 384F};
            var offset = Math.Map(fuelAmount, 0f, fuelCapacity, 768f, 0f);
            // var offset = fuelAmount * 768;
            float[] verts_fuel = {1288f, -384F + offset, 1366f, -384F + offset, 1366f, 384F + offset, 1288f, 384F + offset};
            background.Bind();
            glContext.DrawQuad(verts, Globals.QUAD_UV);
            fuel.Bind();
            glContext.DrawQuad(verts_fuel, Globals.QUAD_UV);
            border.Bind();
            glContext.DrawQuad(verts, Globals.QUAD_UV);
        }
    }
}