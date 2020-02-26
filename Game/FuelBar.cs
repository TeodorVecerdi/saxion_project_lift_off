using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class FuelBar : GameObject {
        private Texture2D border, fuel, background, fuelLowIndicator, fuelLowIndicator2bg, fuelLowIndicator2fg;

        private const float showIndicatorTime = 1f;
        private float showIndicatorTimeLeft = showIndicatorTime;
        private bool shouldShowIndicator1;
        private bool shouldShowIndicator2;
        private bool showingIndicator;

        private float fuelAmount = 60000f;
        private float fuelCapacity = 60000f;

        public FuelBar() {
            border = Texture2D.GetInstance("data/fuelbar_border.png");
            fuel = Texture2D.GetInstance("data/fuelbar_fuel.png");
            background = Texture2D.GetInstance("data/fuelbar_background.png");
            fuelLowIndicator = Texture2D.GetInstance("data/fuel_low_indicator.png");
            fuelLowIndicator2bg = Texture2D.GetInstance("data/fuel_low_indicator2_bg.png");
            fuelLowIndicator2fg = Texture2D.GetInstance("data/fuel_low_indicator2_fg.png");
        }

        public void ChangeFuel(float amount) => fuelAmount += amount;
        public void Refuel() => fuelAmount = fuelCapacity;

        public float FuelAmount {
            get => fuelAmount;
            set => fuelAmount = value;
        }

        public float FuelCapacity {
            get => fuelCapacity;
            set => fuelCapacity = value;
        }

        private void Update() {
            shouldShowIndicator1 = fuelAmount <= Settings.FuelBarIndicatorThresholdMinor * fuelCapacity;
            shouldShowIndicator2 = fuelAmount <= Settings.FuelBarIndicatorThresholdMajor * fuelCapacity;
            if (shouldShowIndicator1) {
                if (showIndicatorTimeLeft <= 0) {
                    showingIndicator = !showingIndicator;
                    showIndicatorTimeLeft = showIndicatorTime;
                }

                showIndicatorTimeLeft -= Time.deltaTime;
            }
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
            DrawFuelLowIndicator(glContext);
        }

        private void DrawFuelLowIndicator(GLContext glContext) {
            if (shouldShowIndicator2) {
                float[] fuelLowVerts = {
                    0f, 0f - 384f,
                    1366f, 0f - 384f,
                    1366f, 768f - 384f,
                    0f, 768f - 384f
                };
                fuelLowIndicator2bg.Bind();
                glContext.DrawQuad(fuelLowVerts, Globals.QUAD_UV);
                if (!showingIndicator)
                    return;
                fuelLowIndicator2fg.Bind();
                glContext.DrawQuad(fuelLowVerts, Globals.QUAD_UV);
            } else {
                if (!shouldShowIndicator1 || !showingIndicator) return;
                float[] fuelLowVerts = {
                    1366f - 78f - 260f - 5f, 768f - 91f - 5f - 384f,
                    1366f - 78f - 5f, 768f - 91f - 5f - 384f,
                    1366f - 78f - 5f, 768f - 5f - 384f,
                    1366f - 78f - 260f - 5f, 768f - 5f - 384f
                };
                fuelLowIndicator.Bind();
                glContext.DrawQuad(fuelLowVerts, Globals.QUAD_UV);    
            }
        }
    }
}