using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The round controller manages the order that
/// units can take turns.  Each round, multiple
/// units could take turns based on their turn order
/// counter reaching a predetermined activation value
/// </summary>
public class RoundController : MonoBehaviour
{
	#region Constants
	const int turnActivation = 1000;
	const int turnCost = 500;
	const int moveCost = 300;
	const int actionCost = 200;
	#endregion

	#region Events
	public static event EventHandler roundBeginEvent;
	public static event EventHandler roundEndEvent;
	public static event EventHandler turnCompleteEvent;
	#endregion

	#region Properties
	BattleController bc;
	List<TurnOrder> order = new List<TurnOrder>();
	#endregion

	#region MonoBehaviour
	void Awake ()
	{
		bc = GetComponent<BattleController>();
	}

	void OnEnable ()
	{
		Death.deathEvent += OnDeathEvent;
		Death.reviveEvent += OnReviveEvent;
	}

	void OnDisable ()
	{
		Death.deathEvent -= OnDeathEvent;
		Death.reviveEvent -= OnReviveEvent;
	}
	#endregion

	#region Event Handlers
	void OnDeathEvent (object sender, EventArgs e)
	{
		RemoveUnit( sender as Unit );
	}

	void OnReviveEvent (object sender, EventArgs e)
	{
		AddUnit( sender as Unit );
	}
	#endregion

	#region Public
	public IEnumerator Tick ()
	{
		while (true)
		{
			// TODO:
			// During the status check phase, each active time-dependent status 
			// effect has its clocktick countdown decreased by 1.  Status effects whose
			// clocktick countdowns have reached zero are removed.
			if (roundBeginEvent != null)
				roundBeginEvent(this, EventArgs.Empty);

			// Now loop through all units and apply a "tick" where their counter is increased
			// by their speed.
			for (int i = 0; i < order.Count; ++i)
				order[i].Tick();

			// Sort the units here
			order.Sort( (a,b) => a.counter.CompareTo(b.counter) );

			// Make a temporary list of units to move, just in case the 
			// order list changes while executing moves.
			List<TurnOrder> toMove = new List<TurnOrder>();
			for (int i = order.Count - 1; i >= 0; --i)
				if (order[i].counter >= turnActivation)
					toMove.Add(order[i]);

			// Each unit with a counter above the action line will 
			// be iterated over and told to take its turn
			for (int i = toMove.Count - 1; i >= 0; --i)
			{
				TurnOrder t = toMove[i];
				if (toMove[i].unit.GetComponent<Death>())
					continue;

				yield return t.unit;

				t.counter -= turnCost;
				if (bc.HasUnitMoved)
					t.counter -= moveCost;
				if (bc.HasUnitActed)
					t.counter -= actionCost;

				if (turnCompleteEvent != null)
					turnCompleteEvent(this, EventArgs.Empty);
			}

			if (roundEndEvent != null)
				roundEndEvent(this, EventArgs.Empty);
		}
	}

	public void AddUnit (Unit unit)
	{
		int index = GetTurnOrderIndex(unit);
		if (index == -1)
			order.Add(new TurnOrder(unit));
	}

	public void RemoveUnit (Unit unit)
	{
		int index = GetTurnOrderIndex(unit);
		if (index != -1)
			order.RemoveAt(index);
	}
	#endregion

	#region Private
	int GetTurnOrderIndex (Unit unit)
	{
		for (int i = order.Count - 1; i >= 0; --i)
			if (order[i].unit == unit)
				return i;
		return -1;
	}
	#endregion
}