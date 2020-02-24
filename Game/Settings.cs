using System.Collections.Generic;
using GXPEngine.Core;

namespace Game {
    public class TileDefinition {
        public bool Drillable;
        public Texture2D Texture;
        public int ScoreAmount;
        public float TimeToDrill;
        public float FuelAmount;
    }
    public class OreSpawnChance {
        public int FromY;
        public int ToY;
        public float Chance;
    }
    public class WalkerSettings {
        public int MaxAge;
        public float DiameterVariation;
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

    public static class Settings {
        public const int RenderDistance = 10;
        public const int InitialFuelRefills = 10;
        public const float IdleFuelDepletion = -333f;
        public const float DrillFuelDepletion = -1000f;
        public const float GravityFrequency = 0.085F;
        public const float PlayerMovementThreshold = 0.25F;
        public const float FuelBarIndicatorThresholdMinor = 0.3333333F;
        public const float FuelBarIndicatorThresholdMajor = 0.1F;
        public static class World {
            public const int TopOffset = 6;
            public const int Depth = 400;
            public static float BlockSize = 10f; // 10 meters
            public static float OreChance = 0.1f;
            public static float StoneChance = 0.1f;
            public static List<ObjectType> Ores = new List<ObjectType> {ObjectType.Stone, ObjectType.Coal, ObjectType.Gold, ObjectType.Emerald, ObjectType.Sapphire};
            public static Dictionary<ObjectType, List<OreSpawnChance>> OreDepthSpawning = new Dictionary<ObjectType, List<OreSpawnChance>> {
                {
                    ObjectType.Stone, new List<OreSpawnChance> {
                        new OreSpawnChance {FromY = 0, ToY = 100, Chance = 0f},
                        new OreSpawnChance {FromY = 101, ToY = 200, Chance = 0.2f},
                        new OreSpawnChance {FromY = 201, ToY = 300, Chance = 0.3f},
                        new OreSpawnChance {FromY = 301, ToY = Depth, Chance = 0.5f}
                    }
                },{
                    ObjectType.Coal, new List<OreSpawnChance> {
                        new OreSpawnChance {FromY = 5, ToY = 75, Chance = 0.1f},
                        new OreSpawnChance {FromY = 76, ToY = 150, Chance = 0.06f},
                        new OreSpawnChance {FromY = 151, ToY = 300, Chance = 0.075f},
                        new OreSpawnChance {FromY = 301, ToY = Depth, Chance = 0.125f}
                    }
                }, {
                    ObjectType.Gold, new List<OreSpawnChance> {
                        new OreSpawnChance {FromY = 35, ToY = 100, Chance = 0.04f},
                        new OreSpawnChance {FromY = 101, ToY = 150, Chance = 0.1f},
                        new OreSpawnChance {FromY = 151, ToY = 200, Chance = 0.15f},
                        new OreSpawnChance {FromY = 201, ToY = Depth, Chance = 0.05f}
                    }
                }, {
                    ObjectType.Emerald, new List<OreSpawnChance> {
                        new OreSpawnChance {FromY = 40, ToY = 75, Chance = 0.007f},
                        new OreSpawnChance {FromY = 76, ToY = 150, Chance = 0.05f},
                        new OreSpawnChance {FromY = 151, ToY = 250, Chance = 0.075f},
                        new OreSpawnChance {FromY = 251, ToY = 320, Chance = 0.085f},
                        new OreSpawnChance {FromY = 321, ToY = Depth, Chance = 0.1f}
                    }
                }, {
                    ObjectType.Sapphire, new List<OreSpawnChance> {
                        new OreSpawnChance {FromY = 50, ToY = 100, Chance = 0.0001f},
                        new OreSpawnChance {FromY = 101, ToY = 150, Chance = 0.01f},
                        new OreSpawnChance {FromY = 151, ToY = 225, Chance = 0.04f},
                        new OreSpawnChance {FromY = 226, ToY = 300, Chance = 0.065f},
                        new OreSpawnChance {FromY = 301, ToY = Depth, Chance = 0.08f}
                    }
                }
            };
            public static Dictionary<ObjectType, WalkerSettings> OreWalkerSettings = new Dictionary<ObjectType, WalkerSettings> {
                {ObjectType.Coal, new WalkerSettings {MaxAge = 10, DiameterVariation = 4}},
                {ObjectType.Gold, new WalkerSettings {MaxAge = 8, DiameterVariation = 4}},
                {ObjectType.Emerald, new WalkerSettings {MaxAge = 6, DiameterVariation = 4}},
                {ObjectType.Sapphire, new WalkerSettings {MaxAge = 5, DiameterVariation = 3}},
            };
        }

        public static class Tiles {
            public static TileDefinition Empty = new TileDefinition {Texture = Texture2D.GetInstance("data/empty.png")};
            public static TileDefinition Background = new TileDefinition {Texture = Texture2D.GetInstance("data/newBackground.png")};
            public static TileDefinition Dirt = new TileDefinition {Drillable = true, TimeToDrill = 0.1f, Texture = Texture2D.GetInstance("data/newDirt2.png")};
            public static TileDefinition Stone = new TileDefinition {Texture = Texture2D.GetInstance("data/newStone.png")};
            public static TileDefinition Player = new TileDefinition {Texture = Texture2D.GetInstance("data/motherload_drill.png")};
            public static TileDefinition Coal = new TileDefinition {FuelAmount = 300f, Drillable = true, ScoreAmount = 25, TimeToDrill = 0.25f, Texture = Texture2D.GetInstance("data/mineral4.png")};
            public static TileDefinition Gold = new TileDefinition {Drillable = true, ScoreAmount = 50, TimeToDrill = 0.5f, Texture = Texture2D.GetInstance("data/mineral1.png")};
            public static TileDefinition Emerald = new TileDefinition {Drillable = true, ScoreAmount = 150, TimeToDrill = 0.75f, Texture = Texture2D.GetInstance("data/mineral3.png")};
            public static TileDefinition Sapphire = new TileDefinition {Drillable = true, ScoreAmount = 300, TimeToDrill = 1f, Texture = Texture2D.GetInstance("data/mineral2.png")};
        

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
}