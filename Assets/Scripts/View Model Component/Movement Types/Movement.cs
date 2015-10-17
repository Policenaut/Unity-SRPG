using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Movement : MonoBehaviour
{
	#region Properties
	public int range;
	public int jumpHeight;
	protected Unit unit;
	[SerializeField] protected Transform jumper;
	#endregion

	#region MonoBehaviour
	protected virtual void Awake ()
	{
		unit = GetComponent<Unit>();
	}
	#endregion

	#region Public
	public virtual HashSet<Tile> GetTilesInRange (Board board)
	{
		HashSet<Tile> retValue = board.Search( unit.tile, ExpandSearch );
		Filter(retValue);
		return retValue;
	}

	public virtual IEnumerator Traverse (Tile tile)
	{
		unit.Place(tile);
		yield return null;
	}
	#endregion

	#region Protected
	protected virtual bool ExpandSearch (Tile tile)
	{
		return tile.distance <= range;
	}

	protected virtual void Filter (HashSet<Tile> tiles)
	{
		Tile[] array = new Tile[tiles.Count];
		tiles.CopyTo(array);
		for (int i = array.Length - 1; i >= 0; --i)
			if (array[i].unit != null)
				tiles.Remove(array[i]);
	}

	protected virtual IEnumerator Turn (Directions dir)
	{
		TransformLocalEulerTweener t = (TransformLocalEulerTweener)transform.RotateToLocal(dir.ToEuler(), 0.25f, EasingEquations.EaseInOutQuad);
		
		// When rotating between North and West, we must make an exception so it looks like the unit
		// rotates the most efficient way (since 0 and 360 are treated the same)
		if (Mathf.Approximately(t.startValue.y, 0f) && Mathf.Approximately(t.endValue.y, 270f))
			t.startValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
		else if (Mathf.Approximately(t.startValue.y, 270) && Mathf.Approximately(t.endValue.y, 0))
			t.endValue = new Vector3(t.startValue.x, 360f, t.startValue.z);

		unit.Dir = dir;
		
		while (t != null)
			yield return null;
	}
	#endregion
}