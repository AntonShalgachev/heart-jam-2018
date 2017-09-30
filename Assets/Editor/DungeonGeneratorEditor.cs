using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var changed = DrawDefaultInspector();

		var gen = (DungeonGenerator)target;
		var tileGen = gen.gameObject.GetComponent<TileGenerator>();

		if (changed)
			tileGen.GenerateTiles();

		if (GUILayout.Button("Generate"))
			tileGen.GenerateTiles();
	}
}
