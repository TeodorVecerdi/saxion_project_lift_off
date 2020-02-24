using System.Collections.Generic;
using System.Linq;

namespace Game.WorldGen {
    internal class WeightedChance {
        public ObjectType Type;
        public float Chance;
    }

    public class WeightedRandomizer {
        private List<WeightedChance> chances;
        private List<WeightedChance> normalizedChances;
        private List<WeightedChance> adjustedChances;

        public WeightedRandomizer() {
            chances = new List<WeightedChance>();
        }

        public void AddChance(ObjectType type, float chance) {
            chances.Add(new WeightedChance {Type = type, Chance = chance});
        }

        public ObjectType GetValue() {
            Normalize();
            Adjust();
            var chance = Rand.Value;
            var foundChance = adjustedChances.FirstOrDefault(adjustedChance => chance < adjustedChance.Chance);
            if (foundChance == null) return ObjectType.Dirt;
            return foundChance.Type;
        }

        private void Normalize() {
            normalizedChances = new List<WeightedChance>();
            var multiplier = 1f / chances.Sum(chance => chance.Chance);
            foreach (var chance in chances) {
                normalizedChances.Add(new WeightedChance {Type = chance.Type, Chance = chance.Chance * multiplier});
            }
        }

        private void Adjust() {
            adjustedChances = new List<WeightedChance>();
            for (var i = 0; i < normalizedChances.Count; i++) {
                var chance = normalizedChances[i];
                adjustedChances.Add(new WeightedChance {Type = chance.Type, Chance = i == 0 ? chance.Chance : chance.Chance + adjustedChances[i - 1].Chance});
            }
        }
    }
}