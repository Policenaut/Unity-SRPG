using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilitySelectionState : BattleState 
{
	List<string> names;
	
	public override void Enter ()
	{
		base.Enter ();
		SnapToCurrent();
		LoadActions();
		abilityMenuPanel.Load(skillSet.name, names.ToArray());
		abilityMenuPanel.Toggle(true);
	}
	
	public override void Exit ()
	{
		base.Exit ();
		abilityMenuPanel.Toggle(false);
	}

	protected override void AddListeners ()
	{
		base.AddListeners ();
		owner.abilityMenuPanel.selectEvent += OnMenuSelection;
		owner.abilityMenuPanel.cancelEvent += OnMenuCancel;
	}

	protected override void RemoveListeners ()
	{
		base.RemoveListeners ();
		owner.abilityMenuPanel.selectEvent -= OnMenuSelection;
		owner.abilityMenuPanel.cancelEvent -= OnMenuCancel;
	}
	
	void OnMenuSelection (object sender, InfoEventArgs<int> e)
	{
		ability = skillSet.skills[ e.info ];
		if (ability.rangeType == Ability.Ranges.Infinite || ability.rangeType == Ability.Ranges.Self)
			owner.ChangeState<ConfirmAbilityTargetState>();
		else
			owner.ChangeState<AbilityTargetState>();
	}
	
	void OnMenuCancel (object sender, System.EventArgs e)
	{
		owner.ChangeState<CategorySelectionState>();
	}
	
	void LoadActions ()
	{
		names = new List<string>();
		for (int i = 0; i < skillSet.skills.Count; ++i)
			names.Add(skillSet.skills[i].title);
	}
}
