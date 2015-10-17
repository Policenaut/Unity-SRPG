using UnityEngine;
using System.Collections;

public class StatChange : ValueChange<int>
{
	public readonly Stats type;

	public StatChange (Stats type, int from, int to) : base (from, to)
	{
		this.type = type;
	}
}