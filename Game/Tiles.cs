using System.Collections.Generic;
using GXPEngine.Core;

namespace Game {
    public class TileDefinition {
        public bool Passable;
        public Texture2D Texture;
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
        Saphire

    }

    public static class Tiles {
        public static TileDefinition Empty = new TileDefinition {Passable = true, Texture = Texture2D.GetInstance("data/empty.png")};
        public static TileDefinition Dirt = new TileDefinition {Passable = true, Texture = Texture2D.GetInstance("data/dirt.png")};
        public static TileDefinition Stone = new TileDefinition {Passable = false, Texture = Texture2D.GetInstance("data/stone.png")};
        public static TileDefinition Player = new TileDefinition {Passable = true, Texture = Texture2D.GetInstance("data/motherload_drill.png")};
        public static TileDefinition Background = new TileDefinition {Passable = true, Texture = Texture2D.GetInstance("data/background.png")};
        public static TileDefinition Coal = new TileDefinition { Passable = false, Texture = Texture2D.GetInstance("data/coal.png") };
        public static TileDefinition Gold = new TileDefinition { Passable = false, Texture = Texture2D.GetInstance("data/gold.png") };
        public static TileDefinition Emerald = new TileDefinition { Passable = false, Texture = Texture2D.GetInstance("data/emerald.png") };
        public static TileDefinition Saphire = new TileDefinition { Passable = false, Texture = Texture2D.GetInstance("data/saphire.png") };

        public static Dictionary<ObjectType, TileDefinition> TypeToTile = new Dictionary<ObjectType, TileDefinition> {
            {ObjectType.Empty, Empty},
            {ObjectType.Dirt, Dirt},
            {ObjectType.Stone, Stone},
            {ObjectType.Player, Player},
            {ObjectType.Background, Background},
            {ObjectType.Coal, Coal},
            {ObjectType.Gold, Gold},
            {ObjectType.Emerald, Emerald},
            {ObjectType.Saphire, Saphire},
        };
    }
}