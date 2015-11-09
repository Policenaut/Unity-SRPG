using UnityEngine;
using System.Collections;

public class PerformAbilityState : BattleState 
{
	public override void Enter ()
	{
		base.Enter ();
		turn.hasUnitActed = true;
		if (turn.hasUnitMoved)
			turn.lockMove = true;
		StartCoroutine(Animate());
	}
	
	IEnumerator Animate ()
	{
		// TODO play animations, etc
		yield return null;
		ApplyAbility();
		
		if (turn.hasUnitMoved)
			owner.ChangeState<EndFacingState>();
		else
			owner.ChangeState<CommandSelectionState>();
	}

	void ApplyAbility ()
	{
		turn.ability.Perform(turn.targets);
	}
}