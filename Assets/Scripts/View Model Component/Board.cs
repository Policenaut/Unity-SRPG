using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
	#region Properties
	public static int level;

	[SerializeField] GameObject tilePrefab;
	Color selectedTileColor = new Color(0, 1, 1, 1);
	Color defaultTileColor = new Color(1, 1, 1, 1);

	public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
	Point[] dirs = new Point[4]
	{
		new Point(0, 1),
		new Point(0, -1),
		new Point(1, 0),
		new Point(-1, 0)
	};
	#endregion
	
	#region MonoBehaviour
	void Awake ()
	{
		CreateBoard ();
//		StartCoroutine("Temp");
	}
	#endregion

	#region Public
	public Tile GetTile (Point p)
	{
		return tiles.ContainsKey(p) ? tiles[p] : null;
	}
	
	public HashSet<Tile> GetTilesInArea (Tile start, int radius, int verticalAllowance)
	{
		if (start == null) return null;
		return Search(start, delegate(Tile arg) { 
			return arg.distance <= radius && (Mathf.Abs(start.height - arg.height) <= verticalAllowance); 
		});
	}
	
//	public HashSet<Tile> GetTilesInRange (Tile start, int radius, int jumpHeight)
//	{
//		if (start == null) return null;
//		return Search(start, delegate(Tile arg) { 
//			return arg.distance <= radius && (Mathf.Abs(arg.prev.height - arg.height) <= jumpHeight);
//		});
//	}

	public HashSet<Tile> Search (Tile start, Func<Tile, bool> addTile)
	{
		ClearSearch();
		start.distance = 0;
		
		HashSet<Tile> retValue = new HashSet<Tile>();
		retValue.Add(start);
		
		Queue<Tile> checkNext = new Queue<Tile>();
		Queue<Tile> checkNow = new Queue<Tile>();
		checkNow.Enqueue(start);
		
		while (checkNow.Count > 0)
		{
			Tile t = checkNow.Dequeue();
			for (int i = 0; i < 4; ++i)
			{
				Tile next = GetTile(t.pos + dirs[i]);
				if (next == null)
					continue;
				
				if (retValue.Contains(next) && t.distance + 1 >= next.distance)
					continue;
				
				next.distance = t.distance + 1;
				next.prev = t;
				
				if (addTile(next))
				{
					checkNext.Enqueue(next);
					retValue.Add(next);
				}
			}
			
			if (checkNow.Count == 0)
				SwapReference(ref checkNow, ref checkNext);
		}
		
		return retValue;
	}

	public void SelectTiles (HashSet<Tile> tiles)
	{
		foreach (Tile t in tiles)
			t.GetComponent<Renderer>().material.color = selectedTileColor;
	}

	public void DeSelectTiles (HashSet<Tile> tiles)
	{
		foreach (Tile t in tiles)
			t.GetComponent<Renderer>().material.color = defaultTileColor;
	}
	#endregion
	
	#region Private
	void CreateBoard ()
	{
		LevelData data = Resources.Load<LevelData>(string.Format("Levels/Level_{0}", level));
		int tileCount = data.tiles.Count;
		
		for (int i = 0; i < tileCount; ++i)
		{
			GameObject instance = Instantiate(tilePrefab) as GameObject;
			Tile t = instance.GetComponent<Tile>();
			t.Load(data.tiles[i]);
			tiles.Add(t.pos, t);
		}
	}
	
//	IEnumerator Temp ()
//	{
//		List<Tile> allTiles = new List<Tile>( tiles.Values );
//
//		while (true)
//		{
//			int rnd = UnityEngine.Random.Range(0, allTiles.Count);
//			Tile random = allTiles[rnd];
//			HashSet<Tile> range = GetTilesInRange(random, 3, 1);
//			foreach (Tile t in range)
//				t.renderer.material = selected;
//			yield return new WaitForSeconds(3);
//			foreach (Tile t in range)
//				t.renderer.material = normal;
//			yield return null;
//		}
//	}

	void SwapReference (ref Queue<Tile> a, ref Queue<Tile> b)
	{
		Queue<Tile> temp = a;
		a = b;
		b = temp;
	}

	void ClearSearch ()
	{
		foreach (Tile t in tiles.Values)
		{
			t.prev = null;
			t.distance = int.MaxValue;
		}
	}
	#endregion
}