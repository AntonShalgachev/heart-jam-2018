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
            tileData.transform.SetRow(0, new Vector4(-0.5f, -0.5f));
            tileData.transform.SetRow(1, new Vector4(-0.5f, -0.5f));
            tileData.transform.SetRow(2, new Vector4(-0.5f, -0.5f));

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
		public static void CreateAnimatedTile()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save Animated Tile", "New Animated Tile", "asset", "Save Animated Tile", "Assets");
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
