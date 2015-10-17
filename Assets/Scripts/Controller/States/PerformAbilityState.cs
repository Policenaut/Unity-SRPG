using UnityEngine;
using System.Collections;

public class PerformAbilityState : BattleState 
{
	public override void Enter ()
	{
		base.Enter ();
		HasUnitActed = true;
		if (HasUnitMoved)
			LockMove = true;
		StartCoroutine(Animate());
	}

	IEnumerator Animate ()
	{
		yield return null;
		// TODO play animations, etc
		yield return null;

		for (int i = 0; i < targets.Count; ++i)
		{
			int damage = ability.CalculateDamage(ability.effects[0], current, targets[i]);
			int hp = targets[i].GetStat(Stats.HP);
			int maxHP = targets[i].GetStat(Stats.MHP);
			int result = Mathf.Clamp(hp - damage, 0, maxHP);
			targets[i].SetStat(Stats.HP, result);
		}

		int resultingMP = owner.current.MP - ability.mpCost;
		owner.current.SetStat(Stats.MP, resultingMP);

		int resultingAP = owner.current.AP - ability.apCost;
		owner.current.SetStat(Stats.AP, resultingAP);

		if (HasUnitMoved)
			owner.ChangeState<EndFacingState>();
		else
			owner.ChangeState<CommandSelectionState>();
	}
}
