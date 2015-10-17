using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveTargetState : BattleState
{
	Movement mover { get { return current.GetComponent<Movement>(); }}
	HashSet<Tile> tiles;

	public override void Enter ()
	{
		base.Enter ();
		tiles = mover.GetTilesInRange(board);
		board.SelectTiles(tiles);
	}

	public override void Exit ()
	{
		base.Exit ();
		board.DeSelectTiles(tiles);
		tiles = null;
	}

	protected override void OnMove (object sender, InfoEventArgs<Point> e)
	{
		owner.MoveCursor(e.info);
		attackerPanel.ShowStats(tile.unit, heroes.Contains(tile.unit));
	}

	protected override void OnFire (object sender, InfoEventArgs<int> e)
	{
		switch (e.info)
		{
		case 0:
			if (tiles.Contains(tile))
				owner.ChangeState<MoveSequenceState>();
			break;
		case 1:
			owner.ChangeState<CommandSelectionState>();
			break;
		}
	}
}
