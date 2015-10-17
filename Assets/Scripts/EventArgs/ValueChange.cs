using UnityEngine;
using System;
using System.Collections;

public class ValueChange<T> : EventArgs
{
	public readonly T fromValue;
	public readonly T toValue;

	public ValueChange (T from, T to)
	{
		fromValue = from;
		toValue = to;
	}
}
