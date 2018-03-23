using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace UnityEngine.Tilemaps
{
	[Serializable]
	public class EditorTile : TileBase
	{
		public GameObject prefab;
		public float m_Rot = 0f;

		public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			tileData.transform = Matrix4x4.identity;
            tileData.transform.SetTRS(Vector3.zero,Quaternion.Euler(0,0, m_Rot),Vector3.one);
            tileData.color = Color.white;
			if (prefab != null)
			{
                var _sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                tileData.sprite = _sprite;
                tileData.gameObject = prefab;
            }
		}

#if UNITY_EDITOR
		[MenuItem("Assets/Create/Tiles/Editor Tile")]
		public static void CreateEditorTile()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save Editor Tile", "New Editor Tile", "asset", "Save Editor Tile", "Assets");
			if (path == "")
				return;

			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<EditorTile>(), path);
		}
#endif
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(EditorTile))]
	public class EditorTileEditor : Editor
	{
		private EditorTile tile { get { return (target as EditorTile); } }

		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();

            tile.prefab = (GameObject)EditorGUILayout.ObjectField("Tile prefab", tile.prefab, typeof(GameObject), false); ;

			tile.m_Rot = EditorGUILayout.FloatField("Tile angle", tile.m_Rot);
            
			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(tile);
		}
	}
#endif
}
