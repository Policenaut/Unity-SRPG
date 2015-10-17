using UnityEngine;
using System.Collections;

public static class DirectionsExtensions
{
	public static Directions GetDirection (this Tile t1, Tile t2)
	{
		if (t1.pos.y < t2.pos.y)
			return Directions.North;
		if (t1.pos.x < t2.pos.x)
			return Directions.East;
		if (t1.pos.y > t2.pos.y)
			return Directions.South;
		return Directions.West;
	}

	public static Directions GetDirection (this Point p)
	{
		if (p.y > 0)
			return Directions.North;
		else if (p.x > 0)
			return Directions.East;
		else if (p.y < 0)
			return Directions.South;
		else
			return Directions.West;
	}

	public static Directions ToDirection (this Vector3 v)
	{
		return (Directions)((int)v.y / 90);
	}

	public static Vector3 ToEuler (this Directions d)
	{
		return new Vector3(0, (int)d * 90, 0);
	}
}