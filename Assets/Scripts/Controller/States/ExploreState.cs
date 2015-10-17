using UnityEngine;
using System.Collections;

public class ExploreState : BattleState 
{
	protected override void OnMove (object sender, InfoEventArgs<Point> e)
	{
		owner.MoveCursor(e.info);
		attackerPanel.ShowStats(tile.unit, heroes.Contains(tile.unit));
	}

	protected override void OnFire (object sender, InfoEventArgs<int> e)
	{
		if (e.info == 0)
			owner.ChangeState<CommandSelectionState>();
	}
}