using UnityEngine;
using System.Collections;

public class StatChange : ValueChange<int>
{
	public readonly StatTypes type;

	public StatChange (StatTypes type, int from, int to) : base (from, to)
	{
		this.type = type;
	}
}