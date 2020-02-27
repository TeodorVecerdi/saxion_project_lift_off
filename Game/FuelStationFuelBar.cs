using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public class FuelStationFuelBar : GameObject {
        private Texture2D border, fuel, background;
        private FuelStation fuelStation;

        public FuelStationFuelBar(FuelStation fuelStation) {
            this.fuelStation = fuelStation;
            border = Texture2D.GetInstance("data/fuelbar/fuelStationFuelBarBorder.png");
            fuel = Texture2D.GetInstance("data/fuelbar/fuelStationFuelBarFuel.png");
            background = Texture2D.GetInstance("data/fuelbar/fuelStationFuelBarBackground.png");
        }

        public void Draw(GLContext glContext) {
            var verts = background.TextureVertices(1f, fuelStation.position + position);
            var (fuel_verts, fuel_uvs) = CalculateFuelVertices();
            background.Bind();
            glContext.DrawQuad(verts, Globals.QUAD_UV);
            fuel.Bind();
            glContext.DrawQuad(fuel_verts, fuel_uvs);
            border.Bind();
            glContext.DrawQuad(verts, Globals.QUAD_UV);
        }

        private (float[], float[]) CalculateFuelVertices() {
            var offset = Math.Map(fuelStation.FuelAmount, 0f, fuelStation.FuelCapacity, 1f, 0f);
            var x = fuelStation.x + position.x;
            var y = fuelStation.y + position.y;
            var width = (float)fuel.width;
            var height = (float) fuel.height;
            float[] vertices = {
                x, y + height*offset,
                x + width, y + height*offset,
                x + width, y + height,
                x, y + height
            };
            float[] uvs = {
                0, 0,
                1, 0,
                1, 1 - offset,
                0, 1 - offset
            };
            return (vertices, uvs);
        }
    }
}