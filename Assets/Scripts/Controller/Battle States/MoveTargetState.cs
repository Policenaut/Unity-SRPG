using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveTargetState : BattleState
{
	List<Tile> tiles;
	
	public override void Enter ()
	{
		base.Enter ();
		Movement mover = turn.unit.GetComponent<Movement>();
		tiles = mover.GetTilesInRange(board);
		board.SelectTiles(tiles);
		RefreshPrimaryStatPanel(pos);
	}
	
	public override void Exit ()
	{
		base.Exit ();
		board.DeSelectTiles(tiles);
		tiles = null;
		statPanelController.HidePrimary();
	}
	
	protected override void OnMove (object sender, InfoEventArgs<Point> e)
	{
		SelectTile(e.info + pos);
		RefreshPrimaryStatPanel(pos);
	}
	
	protected override void OnFire (object sender, InfoEventArgs<int> e)
	{
		if (e.info == 0)
		{
			if (tiles.Contains(owner.currentTile))
				owner.ChangeState<MoveSequenceState>();
		}
		else
		{
			owner.ChangeState<CommandSelectionState>();
		}
	}
}