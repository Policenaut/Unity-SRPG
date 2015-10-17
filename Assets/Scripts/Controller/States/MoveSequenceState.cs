using UnityEngine;
using System.Collections;

public class MoveSequenceState : BattleState 
{
	public override void Enter ()
	{
		base.Enter ();
		HasUnitMoved = true;
		StartCoroutine("Sequence");
	}

	IEnumerator Sequence ()
	{
		Movement m = current.GetComponent<Movement>();
		yield return StartCoroutine(m.Traverse(tile));
		if (HasUnitActed == false)
			owner.ChangeState<CommandSelectionState>();
		else
			owner.ChangeState<EndFacingState>();
	}
}