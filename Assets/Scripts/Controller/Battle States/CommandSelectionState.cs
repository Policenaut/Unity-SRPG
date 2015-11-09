using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommandSelectionState : BaseAbilityMenuState 
{
	public override void Enter ()
	{
		base.Enter ();
		statPanelController.ShowPrimary(turn.unit.gameObject);
	}

	public override void Exit ()
	{
		base.Exit ();
		statPanelController.HidePrimary();
	}

	protected override void LoadMenu ()
	{
		if (menuOptions == null)
		{
			menuTitle = "Commands";
			menuOptions = new List<string>(3);
			menuOptions.Add("Move");
			menuOptions.Add("Action");
			menuOptions.Add("Wait");
		}

		abilityMenuPanelController.Show(menuTitle, menuOptions);
		abilityMenuPanelController.SetLocked(0, turn.hasUnitMoved);
		abilityMenuPanelController.SetLocked(1, turn.hasUnitActed);
	}

	protected override void Confirm ()
	{
		switch (abilityMenuPanelController.selection)
		{
		case 0: // Move
			owner.ChangeState<MoveTargetState>();
			break;
		case 1: // Action
			owner.ChangeState<CategorySelectionState>();
			break;
		case 2: // Wait
			owner.ChangeState<EndFacingState>();
			break;
		}
	}

	protected override void Cancel ()
	{
		if (turn.hasUnitMoved && !turn.lockMove)
		{
			turn.UndoMove();
			abilityMenuPanelController.SetLocked(0, false);
			SelectTile(turn.unit.tile.pos);
		}
		else
		{
			owner.ChangeState<ExploreState>();
		}
	}
}