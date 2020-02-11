using System;
using System.Collections.Generic;
using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public struct TileDefinition {
        public int Id;
        public bool Passable;
        public Texture2D Texture;
    }

    public static class Tiles {
        public static TileDefinition Empty = new TileDefinition {Id=0, Passable = true,  Texture = Texture2D.GetInstance("data/empty.png")};
        public static TileDefinition Dirt =       new TileDefinition {Id=1, Passable = true,  Texture = Texture2D.GetInstance("data/dirt.png")};
        public static TileDefinition Stone =      new TileDefinition {Id=2, Passable = false, Texture = Texture2D.GetInstance("data/stone.png")};
        public static TileDefinition Player = new TileDefinition {Id=3, Passable = true,  Texture = Texture2D.GetInstance("data/motherload_drill.png")};
        public static TileDefinition Background = new TileDefinition {Id=4, Passable = true,  Texture = Texture2D.GetInstance("data/background.png")};

        public static Dictionary<int, TileDefinition> IdToTile = new Dictionary<int, TileDefinition> {
            {0, Empty},
            {1, Dirt},
            {2, Stone},
            {3, Player},
            {4, Background},
        };
    }
}