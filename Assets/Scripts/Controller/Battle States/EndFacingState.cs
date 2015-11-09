using UnityEngine;
using System.Collections;

public class EndFacingState : BattleState 
{
	Directions startDir;

	public override void Enter ()
	{
		base.Enter ();
		startDir = turn.unit.dir;
		SelectTile(turn.unit.tile.pos);
	}
	
	protected override void OnMove (object sender, InfoEventArgs<Point> e)
	{
		turn.unit.dir = e.info.GetDirection();
		turn.unit.Match();
	}
	
	protected override void OnFire (object sender, InfoEventArgs<int> e)
	{
		switch (e.info)
		{
		case 0:
			owner.ChangeState<SelectUnitState>();
			break;
		case 1:
			turn.unit.dir = startDir;
			turn.unit.Match();
			owner.ChangeState<CommandSelectionState>();
			break;
		}
	}
}