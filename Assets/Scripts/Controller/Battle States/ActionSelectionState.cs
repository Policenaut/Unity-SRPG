using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionSelectionState : BaseAbilityMenuState 
{
	public static int category;
	AbilityCatalog catalog;

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
		catalog = turn.unit.GetComponentInChildren<AbilityCatalog>();
		GameObject container = catalog.GetCategory(category);
		menuTitle = container.name;

		int count = catalog.AbilityCount(container);
		if (menuOptions == null)
			menuOptions = new List<string>(count);
		else
			menuOptions.Clear();

		bool[] locks = new bool[count];
		for (int i = 0; i < count; ++i)
		{
			Ability ability = catalog.GetAbility(category, i);
			AbilityMagicCost cost = ability.GetComponent<AbilityMagicCost>();
			if (cost)
				menuOptions.Add(string.Format("{0}: {1}", ability.name, cost.amount));
			else
				menuOptions.Add(ability.name);
			locks[i] = !ability.CanPerform();
		}

		abilityMenuPanelController.Show(menuTitle, menuOptions);
		for (int i = 0; i < count; ++i)
			abilityMenuPanelController.SetLocked(i, locks[i]);
	}

	protected override void Confirm ()
	{
		turn.ability = catalog.GetAbility(category, abilityMenuPanelController.selection);
		owner.ChangeState<AbilityTargetState>();
	}

	protected override void Cancel ()
	{
		owner.ChangeState<CategorySelectionState>();
	}
}