using System.Collections;
using System.Collections.Generic;

public class AbilityTargetState : BattleState 
{
	List<Tile> tiles;
	AbilityRange ar;
	
	public override void Enter ()
	{
		base.Enter ();
		ar = turn.ability.GetComponent<AbilityRange>();
		SelectTiles ();
		statPanelController.ShowPrimary(turn.unit.gameObject);
		if (ar.directionOriented)
			RefreshSecondaryStatPanel(pos);
	}
	
	public override void Exit ()
	{
		base.Exit ();
		board.DeSelectTiles(tiles);
		statPanelController.HidePrimary();
		statPanelController.HideSecondary();
	}
	
	protected override void OnMove (object sender, InfoEventArgs<Point> e)
	{
		if (ar.directionOriented)
		{
			ChangeDirection(e.info);
		}
		else
		{
			SelectTile(e.info + pos);
			RefreshSecondaryStatPanel(pos);
		}
	}
	
	protected override void OnFire (object sender, InfoEventArgs<int> e)
	{
		if (e.info == 0)
		{
			if (ar.directionOriented || tiles.Contains(board.GetTile(pos)))
				owner.ChangeState<ConfirmAbilityTargetState>();
		}
		else
		{
			owner.ChangeState<CategorySelectionState>();
		}
	}
	
	void ChangeDirection (Point p)
	{
		Directions dir = p.GetDirection();
		if (turn.unit.dir != dir)
		{
			board.DeSelectTiles(tiles);
			turn.unit.dir = dir;
			turn.unit.Match();
			SelectTiles ();
		}
	}
	
	void SelectTiles ()
	{
		tiles = ar.GetTilesInRange(board);
		board.SelectTiles(tiles);
	}
}