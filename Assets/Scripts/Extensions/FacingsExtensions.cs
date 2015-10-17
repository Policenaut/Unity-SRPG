using UnityEngine;
using System.Collections;

public static class FacingsExtensions
{
	#region Public
	public static Facings AngleOfAttack (this Unit attacker, Unit defender)
	{
		if (FromFront(attacker, defender))
			return Facings.Front;
		else if (FromSide(attacker, defender))
			return Facings.Side;
		return Facings.Back;
	}
	#endregion
	
	#region Private
	static bool FromFront (Unit attacker, Unit defender)
	{
		Point offset = GetOffset(defender.Dir);
		Point delta = attacker.tile.pos - defender.tile.pos;
		return WithinAngle(offset, delta);
	}
	
	static bool FromSide (Unit attacker, Unit defender)
	{
		Directions s1, s2;
		GetSides(defender.Dir, out s1, out s2);
		Point delta = attacker.tile.pos - defender.tile.pos;
		return WithinAngle(GetOffset(s1), delta) || WithinAngle(GetOffset(s2), delta);
	}
	
	static bool WithinAngle (Point offset, Point delta)
	{
		if (offset.x == 0)
		{
			int min = Mathf.Min(delta.y, -delta.y);
			int max = Mathf.Max(delta.y, -delta.y);
			return ((offset.y < 0 && delta.y < 0) || (offset.y > 0 && delta.y > 0)) && delta.x >= min && delta.x <= max;
		}
		else
		{
			int min = Mathf.Min(delta.x, -delta.x);
			int max = Mathf.Max(delta.x, -delta.x);
			return ((offset.x < 0 && delta.x < 0) || (offset.x > 0 && delta.x > 0)) && delta.y >= min && delta.y <= max;
		}
	}
	
	static void GetSides (Directions dir, out Directions s1, out Directions s2)
	{
		if (dir == Directions.North || dir == Directions.South)
		{
			s1 = Directions.East;
			s2 = Directions.West;
		}
		else
		{
			s1 = Directions.North;
			s2 = Directions.South;
		}
	}
	
	static Point GetOffset (Directions dir)
	{
		switch (dir)
		{
		case Directions.North:
			return new Point(0, 1);
		case Directions.East:
			return new Point(1, 0);
		case Directions.South:
			return new Point(0, -1);
		default: // Directions.West:
			return new Point(-1, 0);
		}
	}
	#endregion
}
