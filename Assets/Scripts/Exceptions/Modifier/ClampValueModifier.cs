using UnityEngine;
using System.Collections;

public class ClampValueModifier : ValueModifier
{
	public readonly float min;
	public readonly float max;

	public ClampValueModifier(int sortOrder, float min, float max) : base (sortOrder)
	{
		this.min = min;
		this.max = max;
	}

	public override float Modify(float fromValue, float toValue)
	{
		return Mathf.Clamp(toValue, min, max);
	}
}
