using System;
using GXPEngine;
using GXPEngine.Core;

namespace Game {
    public struct TileDefinition {
        public int Id;
        public bool Passable;
        public Texture2D Texture;
    }

    public static class Tiles {
        public static TileDefinition Dirt =       new TileDefinition {Id=1, Passable = true,  Texture = Texture2D.GetInstance("data/dirt.png")};
        public static TileDefinition Stone =      new TileDefinition {Id=2, Passable = false, Texture = Texture2D.GetInstance("data/stone.png")};
        public static TileDefinition Background = new TileDefinition {Id=0, Passable = true,  Texture = Texture2D.GetInstance("data/background.png")};
    }
}