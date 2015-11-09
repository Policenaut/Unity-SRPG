using UnityEngine;
using System.Collections;

public abstract class ValueModifier : Modifier
{
	public ValueModifier(int sortOrder) : base(sortOrder) {}
	public abstract float Modify(float fromValue, float toValue);
}
