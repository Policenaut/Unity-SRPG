using UnityEngine;
using System.Collections;

public class MagicalAbilityPower : BaseAbilityPower
{
	public int level;

	protected override int GetBaseAttack ()
	{
		return GetComponentInParent<Stats>()[StatTypes.MAT];
	}

	protected override int GetBaseDefense (Unit target)
	{
		return target.GetComponent<Stats>()[StatTypes.MDF];
	}

	protected override int GetPower ()
	{
		return level;
	}
}