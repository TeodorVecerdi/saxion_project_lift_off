using System.Collections.Generic;
using GXPEngine.Core;

namespace Game {
    public class TileDefinition {
        public bool Passable;
        public Texture2D Texture;
        public int ScoreAmount;
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
        public static TileDefinition Empty = new TileDefinition {Passable = true, Texture = Texture2D.GetInstance("data/empty.png"), ScoreAmount = 0};
        public static TileDefinition Dirt = new TileDefinition {Passable = true, Texture = Texture2D.GetInstance("data/dirt.png"), ScoreAmount = 0};
        public static TileDefinition Stone = new TileDefinition {Passable = false, Texture = Texture2D.GetInstance("data/stone.jpg"), ScoreAmount = 0};
        public static TileDefinition Player = new TileDefinition {Passable = true, Texture = Texture2D.GetInstance("data/motherload_drill.png"), ScoreAmount = 0};
        public static TileDefinition Background = new TileDefinition {Passable = true, Texture = Texture2D.GetInstance("data/background.png"), ScoreAmount = 0};
        public static TileDefinition Coal = new TileDefinition {Passable = true, Texture = Texture2D.GetInstance("data/coal.png"), ScoreAmount = 25};
        public static TileDefinition Gold = new TileDefinition {Passable = true, Texture = Texture2D.GetInstance("data/gold.png"), ScoreAmount = 50};
        public static TileDefinition Emerald = new TileDefinition {Passable = true, Texture = Texture2D.GetInstance("data/emerald.png"), ScoreAmount = 75};
        public static TileDefinition Sapphire = new TileDefinition {Passable = true, Texture = Texture2D.GetInstance("data/sapphire.png"), ScoreAmount = 100};
        // (0.5000000000000001, 1.0000000000000002, 1.5, 2.0000000000000004)
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