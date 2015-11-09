using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineAbilityRange : AbilityRange 
{
	public override bool directionOriented { get { return true; }}
	
	public override List<Tile> GetTilesInRange (Board board)
	{
		Point startPos = unit.tile.pos;
		Point endPos;
		List<Tile> retValue = new List<Tile>();
		
		switch (unit.dir)
		{
		case Directions.North:
			endPos = new Point(startPos.x, board.max.y);
			break;
		case Directions.East:
			endPos = new Point(board.max.x, startPos.y);
			break;
		case Directions.South:
			endPos = new Point(startPos.x, board.min.y);
			break;
		default: // West
			endPos = new Point(board.min.x, startPos.y);
			break;
		}
		
		while (startPos != endPos)
		{
			if (startPos.x < endPos.x) startPos.x++;
			else if (startPos.x > endPos.x) startPos.x--;
			
			if (startPos.y < endPos.y) startPos.y++;
			else if (startPos.y > endPos.y) startPos.y--;
			
			Tile t = board.GetTile(startPos);
			if (t != null && Mathf.Abs(t.height - unit.tile.height) <= vertical)
				retValue.Add(t);
		}
		
		return retValue;
	}
}