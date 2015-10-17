using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// This script tracks events which would lead to
/// a win or loss condition for the game.
/// </summary>
public class VictoryController : MonoBehaviour 
{
	#region Events
	public static event EventHandler victoryEvent;
	#endregion

	#region Properties
	public Alliances victor;
	List<VictoryCondition> conditions = new List<VictoryCondition>();
	#endregion

	#region MonoBehaviour
	void Awake ()
	{
		conditions.Add( new AnnihilationCondition() );
	}

	void OnEnable ()
	{
		RoundController.turnCompleteEvent += OnTurnCompleted;
	}

	void OnDisable ()
	{
		RoundController.turnCompleteEvent -= OnTurnCompleted;
	}
	#endregion

	#region Event Handlers
	void OnTurnCompleted (object sender, EventArgs e)
	{
		for (int i = 0; i < conditions.Count; ++i)
		{
			victor = conditions[i].Check();
			if (victor != Alliances.Neutral)
			{
				if (victoryEvent != null)
					victoryEvent(this, EventArgs.Empty);
				Debug.Log("Battle concluded with victor: " + victor.ToString());
				break;
			}
		}
	}
	#endregion
}
