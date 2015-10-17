using UnityEngine;
using System.Collections;

public class ConfirmAbilityTargetState : BattleState 
{
	int index;
	int HitRate { get { return ability.Chance(current, targets[index]); }}
	int Damage { get { return ability.PredictDamage(ability.effects[0], current, targets[index]); }}

	public override void Enter ()
	{
		base.Enter ();
		hitSuccessGauge.gameObject.SetActive(true);
		owner.ShowAttackerStats(current);
		SelectTarget(0);
	}

	public override void Exit ()
	{
		base.Exit ();
		owner.ShowDefenderStats(null);
		hitSuccessGauge.gameObject.SetActive(false);
	}

	protected override void OnMove (object sender, InfoEventArgs<Point> e)
	{
		if (e.info.x > 1 || e.info.y > 1)
			SelectTarget( (index + 1) % targets.Count );
		else
			SelectTarget( (index == 0) ? targets.Count - 1 : index - 1 );
	}

	protected override void OnFire (object sender, InfoEventArgs<int> e)
	{
		switch (e.info)
		{
		case 0:
			owner.ChangeState<PerformAbilityState>();
			break;
		case 1:
			owner.ChangeState<AbilitySelectionState>();
			break;
		}
	}

	void SelectTarget (int t)
	{
		index = Mathf.Clamp(t, 0, targets.Count);
		owner.SetCursor(targets[index].tile);
		hitSuccessGauge.SetHitRate( HitRate, Damage );
		owner.ShowDefenderStats(targets[index]);
	}
}
