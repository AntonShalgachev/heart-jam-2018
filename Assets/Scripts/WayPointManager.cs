using Assets.Scripts.Common.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public class WayPointManager : SingletonMonoBehaviour<WayPointManager>
    {
        private Tilemap tilemap;
        private List<Vector3Int> sides;
        void Start()
        {
            GetComponent<TilemapRenderer>().enabled = false;
            tilemap = GetComponent<Tilemap>();
            sides = new List<Vector3Int>();
        }

        public Vector3 getNextPoint(Vector3 position, Vector3 direction)
        {
            Vector3 _res = Vector3.zero;
            Vector3Int _pos = tilemap.WorldToCell(position);
            Vector3Int _dir = Vector3Int.RoundToInt(direction.normalized);
            if(_dir  == Vector3Int.zero)
            {
                getSides(Vector3Int.zero, ref sides);
                foreach (Vector3Int dir in sides)
                {
                    if(tilemap.GetTile(_pos + dir) != null)
                    {
                        _dir = dir;
                        break;
                    }
                }
            }
            var _next = tilemap.GetTile(_pos + _dir);
            if (_next != null)
            {
                _res = _dir;
            }
            else
            {
                getSides(_dir, ref sides);
                foreach (Vector3Int dir in sides)
                {
                    if (tilemap.GetTile(_pos + dir) != null)
                    {
                        _res = dir;
                        break;
                    }
                }
            }
            return _res;
        }

        public bool onTheWay(Vector3 position)
        {
            var _tile = tilemap.GetTile(tilemap.WorldToCell(position));
            if (_tile != null) return true;
            return false;
        }

        private void getSides(Vector3Int dir, ref List<Vector3Int> _sides)
        {
            _sides.Clear();
            do
            {
                if (dir == Vector3Int.up || dir == Vector3Int.down)
                {
                    _sides.Add(Vector3Int.right);
                    _sides.Add(Vector3Int.left);
                    break;
                }
                if (dir == Vector3Int.right || dir == Vector3Int.left)
                {
                    _sides.Add(Vector3Int.up);
                    _sides.Add(Vector3Int.down);
                    break;
                }

                _sides.Add(Vector3Int.right);
                _sides.Add(Vector3Int.left);
                _sides.Add(Vector3Int.up);
                _sides.Add(Vector3Int.down);
            } while (false);
        }
    }
}
