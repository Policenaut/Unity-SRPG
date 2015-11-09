using UnityEngine;
using System.Collections;

public abstract class Modifier
{
	public readonly int sortOrder;

	public Modifier(int sortOrder)
	{
		this.sortOrder = sortOrder;
	}
}