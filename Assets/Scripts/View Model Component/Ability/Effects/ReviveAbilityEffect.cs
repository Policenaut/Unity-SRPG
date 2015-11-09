using UnityEngine;
using System.Collections;

public class ReviveAbilityEffect : BaseAbilityEffect 
{
	public float percent;

	public override int Predict (Tile target)
	{
		Stats s = target.content.GetComponent<Stats>();
		return Mathf.FloorToInt(s[StatTypes.MHP] * percent);
	}

	protected override int OnApply (Tile target)
	{
		Stats s = target.content.GetComponent<Stats>();
		int value = s[StatTypes.HP] = Predict(target);
		return value;
	}
}