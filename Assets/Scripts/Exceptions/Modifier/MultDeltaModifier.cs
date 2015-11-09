using UnityEngine;
using System.Collections;

public class MultDeltaModifier : ValueModifier
{
	public readonly float toMultiply;

	public MultDeltaModifier(int sortOrder, float toMultiply)
		: base(sortOrder)
	{
		this.toMultiply = toMultiply;
	}

	public override float Modify(float fromValue, float toValue)
	{
		float delta = toValue - fromValue;
		return fromValue + delta * toMultiply;
	}
}
