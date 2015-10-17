using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardCreator))]
public class BoardCreatorInspector : Editor 
{
	public BoardCreator current
	{
		get
		{
			return (BoardCreator)target;
		}
	}

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Clear"))
			current.Clear();
		if (GUILayout.Button("Grow"))
			current.Grow();
		if (GUILayout.Button("Shrink"))
			current.Shrink();
		if (GUILayout.Button("Grow Area"))
			current.GrowArea();
		if (GUILayout.Button("Shrink Area"))
			current.ShrinkArea();
		if (GUILayout.Button("Save"))
			current.Save();
		if (GUILayout.Button("Load"))
			current.Load();

		if (GUI.changed)
			current.UpdateMarker ();
	}
}