using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkMovement : Movement 
{
	#region Protected
	protected override bool ExpandSearch (Tile from, Tile to)
	{
		// Skip if the distance in height between the two tiles is more than the unit can jump
		if ((Mathf.Abs(from.height - to.height) > jumpHeight))
			return false;

		// Skip if the tile is occupied by an enemy
		if (to.content != null)
			return false;

		return base.ExpandSearch(from, to);
	}
	
	public override IEnumerator Traverse (Tile tile)
	{
		unit.Place(tile);

		// Build a list of way points from the unit's 
		// starting tile to the destination tile
		List<Tile> targets = new List<Tile>();
		while (tile != null)
		{
			targets.Insert(0, tile);
			tile = tile.prev;
		}

		// Move to each way point in succession
		for (int i = 1; i < targets.Count; ++i)
		{
			Tile from = targets[i-1];
			Tile to = targets[i];

			Directions dir = from.GetDirection(to);
			if (unit.dir != dir)
			{ yield return StartCoroutine(Turn(dir)); }

			if (from.height == to.height)
			{ yield return StartCoroutine(Walk(to)); }
			else
			{	yield return StartCoroutine(Jump(to)); }
		}

		yield return null;
	}
	#endregion

	#region Private
	IEnumerator Walk (Tile target)
	{
		Tweener tweener = transform.MoveTo(target.center, 0.5f, EasingEquations.Linear);
		while (tweener != null)
		{ yield return null; }
	}

	IEnumerator Jump (Tile to)
	{
		Tweener tweener = transform.MoveTo(to.center, 0.5f, EasingEquations.Linear);

		Tweener t2 = jumper.MoveToLocal(new Vector3(0, Tile.stepHeight * 2f, 0), tweener.duration / 2f, EasingEquations.EaseOutQuad);
		t2.loopCount = 1;
		t2.loopType = EasingControl.LoopType.PingPong;

		while (tweener != null)
		{ yield return null; }
	}
	#endregion
}