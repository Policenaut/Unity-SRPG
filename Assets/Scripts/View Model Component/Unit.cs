using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
	public Tile tile { get; protected set; }
	public Directions dir;

	public void Place(Tile target)
	{
		// Make sure old tile location is not still pointing to this unit
		if (tile != null && tile.content == gameObject)
		{ tile.content = null; }

		// Link unit and tile references
		tile = target;

		if (target != null)
		{ target.content = gameObject; }
	}

	public void Match()
	{
		transform.localPosition = tile.center;
		transform.localEulerAngles = dir.ToEuler();
	}
}