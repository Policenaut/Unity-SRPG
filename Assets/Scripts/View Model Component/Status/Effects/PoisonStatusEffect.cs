using UnityEngine;
using System.Collections;

public class PoisonStatusEffect : StatusEffect 
{
	Unit owner;

	void OnEnable ()
	{
		owner = GetComponentInParent<Unit>();
		if (owner)
			this.AddObserver(OnNewTurn, TurnOrderController.TurnBeganNotification, owner);
	}

	void OnDisable ()
	{
		this.RemoveObserver(OnNewTurn, TurnOrderController.TurnBeganNotification, owner);
	}

	void OnNewTurn (object sender, object args)
	{
		Stats s = GetComponentInParent<Stats>();
		int currentHP = s[StatTypes.HP];
		int maxHP = s[StatTypes.MHP];
		int reduce = Mathf.Min(currentHP, Mathf.FloorToInt(maxHP * 0.1f));
		s.SetValue(StatTypes.HP, (currentHP - reduce), false);
	}
}
