using GXPEngine;

namespace Game.WorldGen {
    public static class Walker {
        private static float advanceChance = 0.55f;
        private static float alterDiameterChance = 0.85f;
        // private static float alterDirectionChance = 0.95f;

        public static void Start(int x, int y, ObjectType type, TileGrid grid, int topOffset) {
            var position = new Vector2Int(x, y);
            var diameter = Rand.Range(1, Settings.World.OreWalkerSettings[type].DiameterVariation);
            var maxAge = Settings.World.OreWalkerSettings[type].MaxAge;
            var age = 0;
            var direction = Vector2Int.From(Rand.RangeInclusive(-1, 1), Rand.RangeInclusive(-1, 1));
            while (age < maxAge) {
                Paint(position, diameter, grid, type, topOffset);
                var chance = Rand.Value;
                if (chance <= advanceChance) {
                    position.Add(direction.x, direction.y);
                } else if (chance <= alterDiameterChance) {
                    diameter = Rand.Range(1, Settings.World.OreWalkerSettings[type].DiameterVariation);
                } else  {
                    direction = Vector2Int.From(Rand.RangeInclusive(-1, 1), Rand.RangeInclusive(-1, 1));
                }
                age++;
            }
        }
        
        public static void Start2(int x, int y, ObjectType type, TileGrid grid, int topOffset) {
            var advanceChance = 0.65f;
            var position = new Vector2Int(x, y);
            var diameter = Rand.Range(1, Settings.World.OreWalkerSettings[type].DiameterVariation);
            var maxAge = Settings.World.OreWalkerSettings[type].MaxAge;
            var age = 0;
            var direction = Vector2Int.From(Rand.RangeInclusive(-1, 1), Rand.RangeInclusive(-1, 1));
            while (age < maxAge) {
                grid.Tiles[position.x, position.y + topOffset] = type;
                var chance = Rand.Value;
                if (chance <= advanceChance) {
                    position.Add(direction.x, direction.y);
                    position.x = Mathf.Clamp(position.x, 0, grid.TilesHorizontal - 1);
                    position.y = Mathf.Clamp(position.y, 0, grid.TilesVertical - 1);
                } else  {
                    direction = Vector2Int.From(Rand.RangeInclusive(-1, 1), Rand.RangeInclusive(-1, 1));
                }
                age++;
            }
        }

        private static void Paint(Vector2Int position, float diameter, TileGrid grid, ObjectType type, int topOffset) {
            var realDiameter = Mathf.RoundToInt(diameter);
            for (var x = position.x - realDiameter / 2; x < position.x + realDiameter / 2; x++) {
                for (var y = position.y - realDiameter / 2; y < position.y + realDiameter / 2; y++) {
                    var newx = Mathf.Clamp(x, 0, grid.TilesHorizontal - 1);
                    var newy = Mathf.Clamp(y, 0, grid.TilesVertical - 1);
                    grid.Tiles[newx, newy + topOffset] = type;
                }
            }
        }
    }
}