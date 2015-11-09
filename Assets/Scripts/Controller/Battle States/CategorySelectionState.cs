using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CategorySelectionState : BaseAbilityMenuState 
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
			menuOptions = new List<string>();
		else
			menuOptions.Clear();

		menuTitle = "Action";
		menuOptions.Add("Attack");

		AbilityCatalog catalog = turn.unit.GetComponentInChildren<AbilityCatalog>();
		for (int i = 0; i < catalog.CategoryCount(); ++i)
			menuOptions.Add( catalog.GetCategory(i).name );
		
		abilityMenuPanelController.Show(menuTitle, menuOptions);
	}

	protected override void Confirm ()
	{
		if (abilityMenuPanelController.selection == 0)
			Attack();
		else
			SetCategory(abilityMenuPanelController.selection - 1);
	}
	
	protected override void Cancel ()
	{
		owner.ChangeState<CommandSelectionState>();
	}

	void Attack ()
	{
		turn.ability = turn.unit.GetComponentInChildren<Ability>();
		owner.ChangeState<AbilityTargetState>();
	}

	void SetCategory (int index)
	{
		ActionSelectionState.category = index;
		owner.ChangeState<ActionSelectionState>();
	}
}
