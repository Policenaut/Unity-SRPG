using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turn
{
	public Unit unit;
	public bool hasUnitMoved;
	public bool hasUnitActed;
	public bool lockMove;
	public Ability ability;
	public List<Tile> targets;
	Tile startTile;
	Directions startDir;

	public void Change(Unit current)
	{
		unit = current;
		hasUnitActed = false;
		hasUnitMoved = false;
		lockMove = false;
		startTile = unit.tile;
		startDir = unit.dir;
	}

	public void UndoMove()
	{
		hasUnitMoved = false;
		unit.Place(startTile);
		unit.dir = startDir;
		unit.Match();
	}
}
