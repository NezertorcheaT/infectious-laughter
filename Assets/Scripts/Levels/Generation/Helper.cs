using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Levels.Generation;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CustomHelper
{
    public static partial class Helper
    {
        public static string ByComma<T>(this IEnumerable<T> enumerable) =>
            enumerable.Select(i => i.ToString()).ByComma();

        public static string ByComma(this IEnumerable<GameObject> enumerable) =>
            enumerable.Select(i => i.name).ByComma();

        public static string ByComma(this IEnumerable<Entity.Entity> enumerable) =>
            enumerable.Select(i => i.gameObject.name).ByComma();

        public static string ByComma(this IEnumerable<string> enumerable)
        {
            var array = enumerable as string[] ?? enumerable.ToArray();
            return array.Length == 0
                ? string.Empty
                : array.Aggregate((a, b) => $"{a}, {b}");
        }

        public static bool IsPrefab(this GameObject gameObject) => gameObject.scene.name is null;
        public static bool IsOnPrefab(this Component component) => component.gameObject.scene.name is null;

        public static int Area(this Vector2Int value) => value.x * value.y;
        public static float Area(this Vector2 value) => value.x * value.y;

        public static Vector2 Swap(this Vector2 a) => new(a.y, a.x);

        public static Vector2Int ToInt(this Vector2 a) => new((int)a.x, (int)a.y);
        public static Vector3Int ToInt(this Vector3 a) => new((int)a.x, (int)a.y, (int)a.z);
        public static Vector3Int ToVector3Int(this Vector2Int a, int def = 0) => new(a.x, a.y, def);
        public static Vector3Int ToVector3Int(this Vector2 a, int def = 0) => new((int)a.x, (int)a.y, def);
        public static Vector2Int ToVector2Int(this Vector2 a) => new((int)a.x, (int)a.y);
        public static Vector3Int ToVector3Int(this Vector3 a) => new((int)a.x, (int)a.y, (int)a.z);
        public static Vector2Int ToVector2Int(this Vector3 a) => new((int)a.x, (int)a.y);
        public static Vector2 ToVector2(this Vector3 a) => new(a.x, a.y);
        public static Vector2Int ToVector2Int(this Vector3Int a) => new(a.x, a.y);

        public static bool Contains2D(this BoundsInt a, Vector3Int point) => a.Contains2D(point.ToVector3());

        public static bool Contains2D(this BoundsInt a, Vector3 point) =>
            point.x >= a.xMin &&
            point.x <= a.xMax &&
            point.y >= a.yMin &&
            point.y <= a.yMax;

        public static bool Contains2D(this Bounds a, Vector3Int point) => a.Contains2D(point.ToVector3());

        public static bool Contains2D(this Bounds a, Vector3 point) =>
            point.x >= a.min.x &&
            point.x <= a.max.x &&
            point.y >= a.min.y &&
            point.y <= a.max.y;

        public static Vector2Int Inverse(this Vector2Int a) => new(a.y, a.x);
        public static Vector2 Inverse(this Vector2 a) => new(a.y, a.x);

        public const float Intersects2DContract = 0.5f;

        public static bool IntersectsMany2D(this BoundsInt b, IEnumerable<BoundsInt> enumerable,
            bool cutCorners = false) =>
            enumerable.Any(a => b.Intersects2D(a, cutCorners));

        //гениально
        public static bool Intersects2D(this BoundsInt a, BoundsInt b, bool corners = false)
        {
            if (a.Contains2D(b.center.ToVector3Int()))
                return true;
            if (b.Contains2D(a.center.ToVector3Int()))
                return true;

            if (corners)
            {
                if (a.Contains2D(b.min + Intersects2DContract.ToVector3()))
                    return true;
                if (a.Contains2D(b.max - Intersects2DContract.ToVector3()))
                    return true;
                if (a.Contains2D(new Vector3(b.xMax - Intersects2DContract, b.yMin + Intersects2DContract)))
                    return true;
                if (a.Contains2D(new Vector3(b.xMin + Intersects2DContract, b.yMax - Intersects2DContract)))
                    return true;
                if (b.Contains2D(a.min + Intersects2DContract.ToVector3()))
                    return true;
                if (b.Contains2D(a.max - Intersects2DContract.ToVector3()))
                    return true;
                if (b.Contains2D(new Vector3(a.xMax - Intersects2DContract, a.yMin + Intersects2DContract)))
                    return true;
                if (b.Contains2D(new Vector3(a.xMin + Intersects2DContract, a.yMax - Intersects2DContract)))
                    return true;
            }
            else
            {
                if (a.Contains2D(b.min))
                    return true;
                if (a.Contains2D(b.max))
                    return true;
                if (a.Contains2D(new Vector3Int(b.xMax, b.yMin)))
                    return true;
                if (a.Contains2D(new Vector3Int(b.xMin, b.yMax)))
                    return true;
                if (b.Contains2D(a.min))
                    return true;
                if (b.Contains2D(a.max))
                    return true;
                if (b.Contains2D(new Vector3Int(a.xMax, a.yMin)))
                    return true;
                if (b.Contains2D(new Vector3Int(a.xMin, a.yMax)))
                    return true;
            }

            return false;
        }

        public static bool IntersectsMany2D(this IEnumerable<BoundsInt> enumerable, BoundsInt b,
            bool cutCorners = false) =>
            enumerable.Any(a => b.Intersects2D(a, cutCorners));

        public static bool IntersectsMany2D(this Bounds b, IEnumerable<Bounds> enumerable, bool cutCorners = false) =>
            enumerable.Any(a => b.Intersects2D(a, cutCorners));

        public static bool IntersectsMany2D(this IEnumerable<Bounds> enumerable, Bounds b, bool cutCorners = false) =>
            enumerable.Any(a => b.Intersects2D(a, cutCorners));

        //гениально 2
        public static bool Intersects2D(this Bounds a, Bounds b, bool corners = false)
        {
            if (corners)
                a = new Bounds(a.center, a.size - Intersects2DContract.ToVector3());

            if (a.Contains2D(b.center.ToVector3Int()))
                return true;
            if (b.Contains2D(a.center.ToVector3Int()))
                return true;
            if (a.Contains2D(b.min))
                return true;
            if (a.Contains2D(b.max))
                return true;
            if (a.Contains2D(new Vector3(b.max.x, b.min.y)))
                return true;
            if (a.Contains2D(new Vector3(b.min.x, b.max.y)))
                return true;
            if (b.Contains2D(a.min))
                return true;
            if (b.Contains2D(a.max))
                return true;
            if (b.Contains2D(new Vector3(a.max.x, a.min.y)))
                return true;
            if (b.Contains2D(new Vector3(a.min.x, a.max.y)))
                return true;
            return false;
        }

        public static Vector2Int ToVector2Int(this int a) => new(a, a);
        public static Vector2Int ToVector2Int(this float a) => new Vector2(a, a).ToVector2Int();
        public static Vector2 ToVector2(this int a) => new(a, a);
        public static Vector2 ToVector2(this float a) => new(a, a);

        public static Vector3Int ToVector3Int(this int a) => new(a, a, a);
        public static Vector3Int ToVector3Int(this float a) => new Vector3(a, a, a).ToVector3Int();
        public static Vector3 ToVector3(this int a) => new(a, a, a);
        public static Vector3 ToVector3(this float a) => new(a, a, a);

        public static Vector3 ToVector3(this Vector2 vec) => new(vec.x, vec.y, 0);
        public static Vector3 ToVector3(this Vector2Int vec) => new(vec.x, vec.y, 0);
        public static Vector3 ToVector3(this Vector3Int vec) => new(vec.x, vec.y, vec.z);
        public static Vector2Int ToVector2(this Vector2Int vec) => new(vec.x, vec.y);

        /// <summary>
        /// Пускает луч по тайловой карте, не поддерживает изометрию
        /// </summary>
        /// <param name="tilemap">тайловая карта</param>
        /// <param name="gridPosition">позиция начала луча</param>
        /// <param name="direction">направление луча</param>
        /// <returns></returns>
        public static GridRayHit GridRay(this Tilemap tilemap, Vector2Int gridPosition, Vector2 direction) =>
            tilemap.GridRay(
                gridPosition,
                direction,
                Array.Empty<TileBase>()
            );

        /// <summary>
        /// Пускает луч по тайловой карте, не поддерживает изометрию
        /// </summary>
        /// <param name="tilemap">тайловая карта</param>
        /// <param name="gridPosition">позиция начала луча</param>
        /// <param name="direction">направление луча</param>
        /// <param name="distance">расстояние луча</param>
        /// <returns></returns>
        public static GridRayHit GridRay(this Tilemap tilemap, Vector2Int gridPosition, Vector2 direction,
            int distance) =>
            tilemap.GridRay(
                gridPosition,
                direction,
                distance,
                Array.Empty<TileBase>()
            );

        /// <summary>
        /// Пускает луч по тайловой карте, не поддерживает изометрию
        /// </summary>
        /// <param name="tilemap">тайловая карта</param>
        /// <param name="gridPosition">позиция начала луча</param>
        /// <param name="direction">направление луча</param>
        /// <param name="exclude">тайлы, которые учитываться не будут</param>
        /// <returns></returns>
        public static GridRayHit GridRay(this Tilemap tilemap, Vector2Int gridPosition, Vector2 direction,
            IEnumerable<TileBase> exclude) =>
            tilemap.GridRay(
                gridPosition,
                direction,
                tilemap.cellBounds.size.ToVector2Int().sqrMagnitude,
                exclude
            );

        /// <summary>
        /// Пускает луч по тайловой карте, не поддерживает изометрию
        /// </summary>
        /// <param name="tilemap">тайловая карта</param>
        /// <param name="gridPosition">позиция начала луча</param>
        /// <param name="direction">направление луча</param>
        /// <param name="distance">расстояние луча</param>
        /// <param name="exclude">тайлы, которые учитываться не будут</param>
        /// <returns></returns>
        private static GridRayHit GridRay(
            this Tilemap tilemap,
            Vector2Int gridPosition,
            Vector2 direction,
            int distance,
            IEnumerable<TileBase> exclude
        )
        {
            direction = direction.normalized;
            var excluded = exclude as TileBase[] ?? exclude.ToArray();
            for (var i = 0; i < distance; i++)
            {
                var pos = (gridPosition.ToVector2() + direction * i).ToVector3Int();
                if (tilemap.HasTile(pos) && (excluded.Length == 0 || !excluded.Contains(tilemap.GetTile(pos))))
                    return new GridRayHit(tilemap.GetTile(pos), i, pos.ToVector2Int());
            }

            return new GridRayHit(null, distance, (gridPosition + direction * distance).ToVector2Int());
        }

        public static RoomPrefab.Port.Direction Inverse(this RoomPrefab.Port.Direction direction) => direction switch
        {
            RoomPrefab.Port.Direction.Up => RoomPrefab.Port.Direction.Down,
            RoomPrefab.Port.Direction.Down => RoomPrefab.Port.Direction.Up,
            RoomPrefab.Port.Direction.Right => RoomPrefab.Port.Direction.Left,
            RoomPrefab.Port.Direction.Left => RoomPrefab.Port.Direction.Right,
            _ => direction
        };

        /// <summary>
        /// вставить в тайловую карту, другую тайловую карту, с отступом
        /// </summary>
        /// <param name="receiver">куда вставлять</param>
        /// <param name="tilemap">что вставлять</param>
        /// <param name="offset">отступ в пространстве сетки</param>
        public static void Insert(this Tilemap receiver, Tilemap tilemap, Vector2Int offset)
        {
            foreach (var mapPos in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.HasTile(mapPos))
                    receiver.SetTile(mapPos + offset.ToVector3Int(), tilemap.GetTile(mapPos));
            }
        }
    }
}

public readonly struct GridRayHit
{
    public readonly Vector2Int point;
    public readonly int distance;
    [CanBeNull] public readonly TileBase tile;

    public bool isHit => tile is not null;

    public GridRayHit([CanBeNull] TileBase tile, int distance, Vector2Int point)
    {
        this.tile = tile;
        this.distance = distance;
        this.point = point;
    }
}