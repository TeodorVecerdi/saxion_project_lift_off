using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using GXPEngine.Core;
using Newtonsoft.Json;

namespace Game {
    public class TileDefinition {
        public bool Drillable;
        public string TexturePath;
        public int ScoreAmount;
        public float TimeToDrill;
        public float FuelAmount;
        public Texture2D Texture;
    }

    public class OreSpawnChance {
        public int FromY;
        public int ToY;
        public float Chance;
    }

    public enum ObjectType {
        Empty,
        Background,
        Player,
        Dirt,
        MediumDirt,
        HardDirt,
        Stone,
        MediumStone,
        HardStone,
        Coal,
        MediumCoal,
        HardCoal,
        Gold,
        MediumGold,
        HardGold,
        Emerald,
        MediumEmerald,
        HardEmerald,
        Sapphire,
        MediumSapphire,
        HardSapphire,
        DrillingSpeedUpgrade,
        ViewDistanceUpgrade,
        FuelCapacityUpgrade,
        MediumDrillingSpeedUpgrade,
        HardDrillingSpeedUpgrade,
        MediumViewDistanceUpgrade,
        HardViewDistanceUpgrade,
        MediumFuelCapacityUpgrade,
        HardFuelCapacityUpgrade,
        Treasure
    }

    public class Settings {
        private static Settings instance;
        public static Settings Instance {
            get {
                if (instance == null) {
                    var json = File.ReadAllText("data/settings.json");
                    instance = JsonConvert.DeserializeObject<Settings>(json);
                    instance.Initialize();
                }
                return instance;
            }
        }
        public int RenderDistance;
        public float FirstFuelStationFuel;
        public float SecondFuelStationFuel;
        public float IdleFuelConsumption;
        public float DrillOnFuelConsumption;
        public float DrillingFuelConsumption;
        public float TreasureFuelConsumption;
        public float GravityFrequency;
        public float PlayerMovementThreshold;
        public float FuelBarIndicatorThresholdMinor;
        public float FuelBarIndicatorThresholdMajor;
        public float DrillSpeedUpgradeMultiplier;
        public float ViewDistanceUpgradeMultiplier;
        public float FuelCapacityUpgradeMultiplier;
        public World World;
        public Tiles Tiles;

        public void Initialize() {
            Tiles.TypeToTile = new Dictionary<ObjectType, TileDefinition> {
                {ObjectType.Empty, Tiles.Empty},
                {ObjectType.Background, Tiles.Background},
                {ObjectType.Player, Tiles.Player},
                {ObjectType.Dirt, Tiles.Dirt},
                {ObjectType.MediumDirt, Tiles.MediumDirt},
                {ObjectType.HardDirt, Tiles.HardDirt},
                {ObjectType.Stone, Tiles.Stone},
                {ObjectType.MediumStone, Tiles.MediumStone},
                {ObjectType.HardStone, Tiles.HardStone},
                {ObjectType.Coal, Tiles.Coal},
                {ObjectType.MediumCoal, Tiles.MediumCoal},
                {ObjectType.HardCoal, Tiles.HardCoal},
                {ObjectType.Gold, Tiles.Gold},
                {ObjectType.MediumGold, Tiles.MediumGold},
                {ObjectType.HardGold, Tiles.HardGold},
                {ObjectType.Emerald, Tiles.Emerald},
                {ObjectType.MediumEmerald, Tiles.MediumEmerald},
                {ObjectType.HardEmerald, Tiles.HardEmerald},
                {ObjectType.Sapphire, Tiles.Sapphire},
                {ObjectType.MediumSapphire, Tiles.MediumSapphire},
                {ObjectType.HardSapphire, Tiles.HardSapphire},
                {ObjectType.DrillingSpeedUpgrade, Tiles.DrillingSpeedUpgrade},
                {ObjectType.MediumDrillingSpeedUpgrade, Tiles.MediumDrillingSpeedUpgrade},
                {ObjectType.HardDrillingSpeedUpgrade, Tiles.HardDrillingSpeedUpgrade},
                {ObjectType.ViewDistanceUpgrade, Tiles.ViewDistanceUpgrade},
                {ObjectType.MediumViewDistanceUpgrade, Tiles.MediumViewDistanceUpgrade},
                {ObjectType.HardViewDistanceUpgrade, Tiles.HardViewDistanceUpgrade},
                {ObjectType.FuelCapacityUpgrade, Tiles.FuelCapacityUpgrade},
                {ObjectType.MediumFuelCapacityUpgrade, Tiles.MediumFuelCapacityUpgrade},
                {ObjectType.HardFuelCapacityUpgrade, Tiles.HardFuelCapacityUpgrade},
                {ObjectType.Treasure, Tiles.Treasure},
            };
            Tiles.TypeToTile.Values.ToList().ForEach(definition => definition.Texture = Texture2D.GetInstance(definition.TexturePath, true));
            Tiles.HardnessToTile = new Dictionary<ObjectType, List<ObjectType>> {
                {ObjectType.Dirt, new List<ObjectType> {ObjectType.Dirt, ObjectType.MediumDirt, ObjectType.HardDirt}},
                {ObjectType.Stone, new List<ObjectType> {ObjectType.Stone, ObjectType.MediumStone, ObjectType.HardStone}},
                {ObjectType.Coal, new List<ObjectType> {ObjectType.Coal, ObjectType.MediumCoal, ObjectType.HardCoal}},
                {ObjectType.Gold, new List<ObjectType> {ObjectType.Gold, ObjectType.MediumGold, ObjectType.HardGold}},
                {ObjectType.Emerald, new List<ObjectType> {ObjectType.Emerald, ObjectType.MediumEmerald, ObjectType.HardEmerald}},
                {ObjectType.Sapphire, new List<ObjectType> {ObjectType.Sapphire, ObjectType.MediumSapphire, ObjectType.HardSapphire}},
                {ObjectType.DrillingSpeedUpgrade, new List<ObjectType> {ObjectType.DrillingSpeedUpgrade, ObjectType.MediumDrillingSpeedUpgrade, ObjectType.HardDrillingSpeedUpgrade}},
                {ObjectType.ViewDistanceUpgrade, new List<ObjectType> {ObjectType.ViewDistanceUpgrade, ObjectType.MediumViewDistanceUpgrade, ObjectType.HardViewDistanceUpgrade}},
                {ObjectType.FuelCapacityUpgrade, new List<ObjectType> {ObjectType.FuelCapacityUpgrade, ObjectType.MediumFuelCapacityUpgrade, ObjectType.HardFuelCapacityUpgrade}},
            };
        }
    }

    public class World {
        public int TopOffset;
        public int MediumDirtStartDepth;
        public int HardDirtStartDepth;
        public int Depth;
        public int UpgradeCount;
        public float BlockSize;
        public float OreChance;
        public float StoneChance;
        public List<ObjectType> UpgradeTypes;
        public Dictionary<ObjectType, List<OreSpawnChance>> OreDepthSpawning;
    }

    public class Tiles {
        public TileDefinition Empty;
        public TileDefinition Background;
        public TileDefinition Player;
        public TileDefinition Dirt;
        public TileDefinition MediumDirt;
        public TileDefinition HardDirt;
        public TileDefinition Stone;
        public TileDefinition MediumStone;
        public TileDefinition HardStone;
        public TileDefinition Coal;
        public TileDefinition MediumCoal;
        public TileDefinition HardCoal;
        public TileDefinition Gold;
        public TileDefinition MediumGold;
        public TileDefinition HardGold;
        public TileDefinition Emerald;
        public TileDefinition MediumEmerald;
        public TileDefinition HardEmerald;
        public TileDefinition Sapphire;
        public TileDefinition MediumSapphire;
        public TileDefinition HardSapphire;
        public TileDefinition DrillingSpeedUpgrade;
        public TileDefinition MediumDrillingSpeedUpgrade;
        public TileDefinition HardDrillingSpeedUpgrade;
        public TileDefinition ViewDistanceUpgrade;
        public TileDefinition MediumViewDistanceUpgrade;
        public TileDefinition HardViewDistanceUpgrade;
        public TileDefinition FuelCapacityUpgrade;
        public TileDefinition MediumFuelCapacityUpgrade;
        public TileDefinition HardFuelCapacityUpgrade;
        public TileDefinition Treasure;

        public Dictionary<ObjectType, TileDefinition> TypeToTile;
        public Dictionary<ObjectType, List<ObjectType>> HardnessToTile;
    }
}