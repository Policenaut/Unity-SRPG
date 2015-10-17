using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class BoardCreator : MonoBehaviour 
{
	#region Properties
	[SerializeField] GameObject tileViewPrefab;
	[SerializeField] GameObject tileSelectionIndicatorPrefab;

	public Transform marker
	{
		get
		{
			if (_marker == null)
			{
				GameObject instance = Instantiate(tileSelectionIndicatorPrefab) as GameObject;
				_marker = instance.transform;
			}
			return _marker;
		}
	}
	private Transform _marker;

	public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
	public int width = 10;
	public int depth = 10;
	public int height = 8;
	public Point pos;

	public LevelData levelData;
	#endregion

	#region Public
	public void Clear ()
	{
		for (int i = transform.childCount - 1; i >= 0; --i)
			DestroyImmediate(transform.GetChild(i).gameObject);
		tiles.Clear();
	}

	public void Grow ()
	{
		GrowSingle(pos);
	}

	public void Shrink ()
	{
		ShrinkSingle(pos);
	}

	public void GrowArea ()
	{
		Rect r = RandomRect();
		GrowRect(r);
	}

	public void ShrinkArea ()
	{
		Rect r = RandomRect();
		ShrinkRect(r);
	}

	public void UpdateMarker ()
	{
		Tile t = tiles.ContainsKey(pos) ? tiles[pos] : null;
		marker.localPosition = t != null ? t.center : new Vector3(pos.x, 0, pos.y);
	}

	public void Save ()
	{
		string filePath = Application.dataPath + "/Resources/Levels";
		if (!Directory.Exists(filePath))
			CreateSaveDirectory ();
		
		LevelData board = ScriptableObject.CreateInstance<LevelData>();
		board.tiles = new List<Vector3>( tiles.Count );
		foreach (Tile t in tiles.Values)
			board.tiles.Add( new Vector3(t.pos.x, t.height, t.pos.y) );
		
		string fileName = string.Format("Assets/Resources/Levels/{1}.asset", filePath, name);
		AssetDatabase.CreateAsset(board, fileName);
	}

	public void Load ()
	{
		Clear();
		if (levelData == null)
			return;

		foreach (Vector3 v in levelData.tiles)
		{
			Tile t = Create();
			t.Load(v);
			tiles.Add(t.pos, t);
		}
	}
	#endregion

	#region Private
	Tile Create ()
	{
		GameObject instance = Instantiate(tileViewPrefab) as GameObject;
		instance.transform.parent = transform;
		return instance.GetComponent<Tile>();
	}

	Tile GetOrCreate (Point p)
	{
		if (tiles.ContainsKey(p))
			return tiles[p];
		
		Tile t = Create();
		t.Load(p, 0);
		tiles.Add(p, t);
		
		return t;
	}

	void GrowSingle (Point p)
	{
		Tile t = GetOrCreate(p);
		if (t.height < height)
			t.Grow();
	}
	
	void ShrinkSingle (Point p)
	{
		if (!tiles.ContainsKey(p))
			return;
		
		Tile t = tiles[p];
		t.Shrink();
		
		if (t.height <= 0)
		{
			tiles.Remove(p);
			DestroyImmediate(t.gameObject);
		}
	}

	Rect RandomRect ()
	{
		int x = UnityEngine.Random.Range(0, width - 2);
		int y = UnityEngine.Random.Range(0, height - 2);
		int w = UnityEngine.Random.Range(1, width - x);
		int h = UnityEngine.Random.Range(1, depth - y);
		return new Rect(x, y, w, h);
	}

	void GrowRect (Rect rect)
	{
		for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
		{
			for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
			{
				Point p = new Point(x, y);
				GrowSingle(p);
			}
		}
	}

	void ShrinkRect (Rect rect)
	{
		for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
		{
			for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
			{
				Point p = new Point(x, y);
				ShrinkSingle(p);
			}
		}
	}

	void CreateSaveDirectory ()
	{
		string filePath = Application.dataPath + "/Resources";
		if (!Directory.Exists(filePath))
			AssetDatabase.CreateFolder("Assets", "Resources");
		filePath += "/Levels";
		if (!Directory.Exists(filePath))
			AssetDatabase.CreateFolder("Assets/Resources", "Levels");
		AssetDatabase.Refresh();
	}
	#endregion
}