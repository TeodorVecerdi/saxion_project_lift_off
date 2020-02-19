using System;
using GXPEngine;

namespace Game {
    public static class ExtensionMethods {
        public static Vector2Int ToInt(this Vector2 a) => new Vector2Int((int) a.x, (int) a.y);
        public static Vector2Int ToVec2Int(this Vector2 a) => a.ToInt();
        public static Vector2 ToFloat(this Vector2Int a) => new Vector2(a.x, a.y);
        public static Vector2 ToVec2(this Vector2Int a) => a.ToFloat();
        public static Vector2 ToGrid(this Vector2 a) => (a / Globals.TILE_SIZE).ToInt();
        public static Vector2Int ToGrid(this Vector2Int a) => a / Globals.TILE_SIZE;
        public static Vector2 ToWorld(this Vector2 a) => a * Globals.TILE_SIZE;
        public static Vector2Int ToWorld(this Vector2Int a) => a * Globals.TILE_SIZE;
        public static ValueTuple<int, int> Unpack(this Vector2Int a) => (a.x, a.y);
        public static ValueTuple<float, float> Unpack(this Vector2 a) => (a.x, a.y);
        public static Vector2 Add(this Vector2 a, float x, float y) => new Vector2(a.x + x, a.y + y);
        public static Vector2 Move(this Vector2 a, float x, float y) {
            a.x += x;
            a.y += y;
            return a;
        }

        public static Vector2 Move(this Vector2 a, Vector2 b) {
            a.x += b.x;
            a.y += b.y;
            return a;
        }

        public static Vector2Int Add(this Vector2Int a, int x, int y) => new Vector2Int(a.x + x, a.y + y);
        public static Vector2Int Move(this Vector2Int a, int x, int y) {
            a.x += x;
            a.y += y;
            return a;
        }

        public static Vector2Int Move(this Vector2Int a, Vector2Int b) {
            a.x += b.x;
            a.y += b.y;
            return a;
        }
    }
}