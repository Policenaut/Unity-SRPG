using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public class InflictAbilityEffect : BaseAbilityEffect 
{
	public string statusName;
	public int duration;

	public override int Predict (Tile target)
	{
		return 0;
	}

	protected override int OnApply (Tile target)
	{
		Type statusType = Type.GetType(statusName);
		if (statusType == null || !statusType.IsSubclassOf(typeof(StatusEffect)))
		{
			Debug.LogError("Invalid Status Type");
			return 0;
		}

		MethodInfo mi = typeof(Status).GetMethod("Add");
		Type[] types = new Type[]{ statusType, typeof(DurationStatusCondition) };
		MethodInfo constructed = mi.MakeGenericMethod(types);

		Status status = target.content.GetComponent<Status>();
		object retValue = constructed.Invoke(status, null);

		DurationStatusCondition condition = retValue as DurationStatusCondition;
		condition.duration = duration;
		return 0;
	}
}