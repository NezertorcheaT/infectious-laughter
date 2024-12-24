using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CustomHelper
{
    public static partial class Helper
    {
        public static string ByComma<T>(this IEnumerable<T> enumerable) =>
            enumerable.Select(i => i.ToString()).Aggregate((a, b) => $"{a}, {b}");

        public static Vector2Int ToInt(this Vector2 a) => new Vector2Int((int)a.x, (int)a.y);
        public static Vector3Int ToInt(this Vector3 a) => new Vector3Int((int)a.x, (int)a.y, (int)a.z);
        public static Vector3Int ToVector3Int(this Vector2Int a) => new Vector3Int((int)a.x, (int)a.y);
        public static Vector3Int ToVector3Int(this Vector2 a) => new Vector3Int((int)a.x, (int)a.y);
        public static Vector2Int ToVector2Int(this Vector2 a) => new Vector2Int((int)a.x, (int)a.y);
        public static Vector3Int ToVector3Int(this Vector3 a) => new Vector3Int((int)a.x, (int)a.y);
        public static Vector2Int ToVector2Int(this Vector3Int a) => new Vector2Int((int)a.x, (int)a.y);
        public static Vector2Int ToVector2Int(this Vector3 a) => new Vector2Int((int)a.x, (int)a.y);

        public static bool Contains2D(this BoundsInt a, Vector3Int point) =>
            point.x >= a.xMin &&
            point.x <= a.xMax &&
            point.y >= a.yMin &&
            point.y <= a.yMax;

        public static bool Contains2D(this Bounds a, Vector3 point) =>
            point.x >= a.min.x &&
            point.x <= a.max.x &&
            point.y >= a.min.y &&
            point.y <= a.max.y;

        //гениально
        public static bool Intersects2D(this BoundsInt a, BoundsInt b)
        {
            if (a.Contains2D(b.center.ToVector3Int()))
                return true;
            if (b.Contains2D(a.center.ToVector3Int()))
                return true;
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
            return false;
        }

        //гениально 2
        public static bool Intersects2D(this Bounds a, Bounds b)
        {
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

        public static Vector3 ToVector3(this Vector2 vec) => new Vector3(vec.x, vec.y, 0);
        public static Vector3 ToVector3(this Vector2Int vec) => new Vector3(vec.x, vec.y, 0);
        public static Vector3 ToVector3(this Vector3Int vec) => new Vector3(vec.x, vec.y, vec.z);
        public static Vector2Int ToVector2(this Vector2Int vec) => new Vector2Int(vec.x, vec.y);

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
            var excluded = exclude as TileBase[] ?? exclude.ToArray();
            for (var i = 0; i < distance; i++)
            {
                var pos = (gridPosition.ToVector2() + direction * i).ToVector3Int();
                if (tilemap.HasTile(pos) && !excluded.Contains(tilemap.GetTile(pos)))
                    return new GridRayHit(tilemap.GetTile(pos), i, pos.ToVector2Int());
            }

            return new GridRayHit(null, distance, (gridPosition + direction * distance).ToVector2Int());
        }

        /// <summary>
        /// вставить в тайловую карту, другую тайловую карту, с отступом
        /// </summary>
        /// <param name="receiver">куда вставлять</param>
        /// <param name="tilemap">что вставлять</param>
        /// <param name="offset">отступ в пространстве сетки</param>
        /// <param name="voidTile">тайл пустоты</param>
        public static void Insert(this Tilemap receiver, Tilemap tilemap, Vector2Int offset, TileBase voidTile = null)
        {
            foreach (var mapPos in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.HasTile(mapPos))
                    receiver.SetTile(mapPos + offset.ToVector3Int(), tilemap.GetTile(mapPos));

                if (tilemap.GetTile(mapPos) == voidTile)
                    receiver.SetTile(mapPos + offset.ToVector3Int(), null);
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