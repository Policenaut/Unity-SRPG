using UnityEngine;
using System.Collections;

public class PreTurnState : BattleState 
{
	public override void Enter ()
	{
		base.Enter ();
		SnapToCurrent();
		owner.MarkPlacement();
		StartCoroutine("Sequence");
	}

	IEnumerator Sequence ()
	{
		yield return null;
		owner.ChangeState<CommandSelectionState>();
	}
}
