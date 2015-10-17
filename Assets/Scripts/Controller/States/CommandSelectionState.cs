using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommandSelectionState : BattleState 
{
	string menuTitle = "Commands";
	string[] menuOptions = new string[] { "Move", "Action", "Wait" };

	public override void Enter ()
	{
		base.Enter ();
		SnapToCurrent();
		abilityMenuPanel.Load(menuTitle, menuOptions);
		abilityMenuPanel.SetLocked(0, HasUnitMoved);
		abilityMenuPanel.SetLocked(1, HasUnitActed);
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
		abilityMenuPanel.selectEvent += OnMenuSelection;
		abilityMenuPanel.cancelEvent += OnMenuCancel;
	}

	protected override void RemoveListeners ()
	{
		base.RemoveListeners ();
		abilityMenuPanel.selectEvent -= OnMenuSelection;
		abilityMenuPanel.cancelEvent -= OnMenuCancel;
	}

	void OnMenuSelection (object sender, InfoEventArgs<int> e)
	{
		switch (e.info)
		{
		case 0:
			owner.ChangeState<MoveTargetState>();
			break;
		case 1:
			owner.ChangeState<CategorySelectionState>();
			break;
		case 2:
			owner.ChangeState<EndFacingState>();
			break;
		}
	}

	void OnMenuCancel (object sender, System.EventArgs e)
	{
		if (HasUnitMoved && !LockMove)
		{
			owner.UndoMove();
			abilityMenuPanel.SetLocked(0, false);
			SnapToCurrent();
		}
		else
			owner.ChangeState<ExploreState>();
	}
}