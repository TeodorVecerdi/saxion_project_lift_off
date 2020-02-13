using System.Collections.Generic;
using GXPEngine.Core;

namespace Game {
    public class TileDefinition {
        public bool Passable;
        public bool Drillable;
        public Texture2D Texture;
        public int ScoreAmount;
        public float TimeToDrill = 0f;
    }

    public enum ObjectType {
        Empty,
        Background,
        Dirt,
        Stone,
        Player,
        Coal,
        Gold,
        Emerald,
        Sapphire
    }

    public static class Tiles {
        public static TileDefinition Empty = new TileDefinition {Drillable = false, Passable = true, Texture = Texture2D.GetInstance("data/empty.png"), ScoreAmount = 0};
        public static TileDefinition Dirt = new TileDefinition {Drillable = true, Passable = true, Texture = Texture2D.GetInstance("data/dirt.png"), ScoreAmount = 0, TimeToDrill = 0.1f};
        public static TileDefinition Stone = new TileDefinition {Drillable = false, Passable = false, Texture = Texture2D.GetInstance("data/stone.png"), ScoreAmount = 0};
        public static TileDefinition Player = new TileDefinition {Drillable = false, Passable = true, Texture = Texture2D.GetInstance("data/motherload_drill.png"), ScoreAmount = 0};
        public static TileDefinition Background = new TileDefinition {Drillable = false, Passable = true, Texture = Texture2D.GetInstance("data/background.png"), ScoreAmount = 0};
        public static TileDefinition Coal = new TileDefinition {Drillable = true, Passable = true, Texture = Texture2D.GetInstance("data/coal.png"), ScoreAmount = 25, TimeToDrill = 0.25f};
        public static TileDefinition Gold = new TileDefinition {Drillable = true, Passable = true, Texture = Texture2D.GetInstance("data/gold.png"), ScoreAmount = 50, TimeToDrill = 0.5f};
        public static TileDefinition Emerald = new TileDefinition {Drillable = true, Passable = true, Texture = Texture2D.GetInstance("data/emerald.png"), ScoreAmount = 75, TimeToDrill = 0.75f};
        public static TileDefinition Sapphire = new TileDefinition {Drillable = true, Passable = true, Texture = Texture2D.GetInstance("data/sapphire.png"), ScoreAmount = 100, TimeToDrill = 1f};

        public static Dictionary<ObjectType, TileDefinition> TypeToTile = new Dictionary<ObjectType, TileDefinition> {
            {ObjectType.Empty, Empty},
            {ObjectType.Dirt, Dirt},
            {ObjectType.Stone, Stone},
            {ObjectType.Player, Player},
            {ObjectType.Background, Background},
            {ObjectType.Coal, Coal},
            {ObjectType.Gold, Gold},
            {ObjectType.Emerald, Emerald},
            {ObjectType.Sapphire, Sapphire},
        };
    }
}