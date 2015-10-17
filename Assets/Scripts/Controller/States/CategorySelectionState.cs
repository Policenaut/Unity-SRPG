using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CategorySelectionState : BattleState 
{
	string menuTitle = "Action";
	List<string> list;
	
	public override void Enter ()
	{
		base.Enter ();
		SnapToCurrent();
		LoadCategories();
		abilityMenuPanel.Load(menuTitle, list.ToArray());
		abilityMenuPanel.Toggle(true);
	}
	
	public override void Exit ()
	{
		base.Exit ();
		owner.abilityMenuPanel.Toggle(false);
	}

	protected override void AddListeners ()
	{
		base.AddListeners ();
		abilityMenuPanel.selectEvent += OnMenuSelection;
		abilityMenuPanel.cancelEvent += OnMenuCancel;
	}

	protected override void RemoveListeners ()
	{
		base.RemoveListeners ();
		owner.abilityMenuPanel.selectEvent -= OnMenuSelection;
		owner.abilityMenuPanel.cancelEvent -= OnMenuCancel;
	}
	
	void OnMenuSelection (object sender, InfoEventArgs<int> e)
	{
		if (e.info == 0)
		{
			Debug.Log("Attack is not implemented");
		}
		else
		{
			owner.skillSet = owner.current.capability[e.info - 1];
			owner.ChangeState<AbilitySelectionState>();
		}
	}
	
	void OnMenuCancel (object sender, System.EventArgs e)
	{
		owner.ChangeState<CommandSelectionState>();
	}
	
	void LoadCategories ()
	{
		list = new List<string>();
		list.Add("Attack");
		for (int i = 0; i < current.capability.Count; ++i)
			list.Add(current.capability[i].name);
	}
}
