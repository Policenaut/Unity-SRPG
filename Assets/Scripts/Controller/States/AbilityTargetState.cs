using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityTargetState : BattleState 
{
	HashSet<Tile> tiles;

	public override void Enter ()
	{
		base.Enter ();
		tiles = ability.GetTilesInRange(board, current.tile);
		board.SelectTiles(tiles);
	}

	public override void Exit ()
	{
		base.Exit ();
		board.DeSelectTiles(tiles);
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
			GetTargets();
			break;
		case 1:
			owner.ChangeState<AbilitySelectionState>();
			break;
		}
	}

	void GetTargets ()
	{
		if (tiles.Contains(tile))
		{
			targets.Clear();
			HashSet<Tile> targetTiles = ability.GetTilesInArea(board, tile);
			
			foreach (Tile t in targetTiles)
				if (t.unit != null)
					targets.Add(t.unit);
			
			if (targets.Count > 0)
				owner.ChangeState<ConfirmAbilityTargetState>();
		}
	}
}
