using UnityEngine;
using System.Collections;

public class EndFacingState : BattleState 
{
	Directions startDir;

	public override void Enter ()
	{
		base.Enter ();
		SnapToCurrent();
		startDir = current.Dir;
		facingIndicator.transform.localScale = Vector3.one;
		facingIndicator.transform.localPosition = current.tile.center;
		facingIndicator.SetDirection(current.Dir);
	}

	public override void Exit ()
	{
		base.Exit ();
		facingIndicator.transform.localScale = Vector3.zero;
	}

	protected override void OnMove (object sender, InfoEventArgs<Point> e)
	{
		current.Dir = e.info.GetDirection();
		current.Match();
		facingIndicator.SetDirection(current.Dir);
	}

	protected override void OnFire (object sender, InfoEventArgs<int> e)
	{
		switch (e.info)
		{
		case 0:
			owner.CompletedTurn();
			break;
		case 1:
			current.Dir = startDir;
			owner.ChangeState<CommandSelectionState>();
			break;
		}
	}
}
