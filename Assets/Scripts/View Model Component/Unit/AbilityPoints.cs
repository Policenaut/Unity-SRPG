using UnityEngine;
using System.Collections;

public class AbilityPoints : MonoBehaviour
{
	#region Fields
	public int AP
	{
		get { return stats[StatTypes.AP]; }
		set { stats[StatTypes.AP] = value; }
	}

	public int MAP
	{
		get { return stats[StatTypes.MAP]; }
		set { stats[StatTypes.MAP] = value; }
	}

	Unit unit;
	Stats stats;
	#endregion

	#region MonoBehaviour
	void Awake()
	{
		stats = GetComponent<Stats>();
		unit = GetComponent<Unit>();
	}

	void OnEnable()
	{
		this.AddObserver(OnAPWillChange, Stats.WillChangeNotification(StatTypes.AP), stats);
		this.AddObserver(OnMAPDidChange, Stats.DidChangeNotification(StatTypes.MAP), stats);
		this.AddObserver(OnTurnBegan, TurnOrderController.TurnBeganNotification, unit);
	}

	void OnDisable()
	{
		this.RemoveObserver(OnAPWillChange, Stats.WillChangeNotification(StatTypes.AP), stats);
		this.RemoveObserver(OnMAPDidChange, Stats.DidChangeNotification(StatTypes.MAP), stats);
		this.RemoveObserver(OnTurnBegan, TurnOrderController.TurnBeganNotification, unit);
	}
	#endregion

	#region Event Handlers
	void OnAPWillChange(object sender, object args)
	{
		ValueChangeException vce = args as ValueChangeException;
		vce.AddModifier(new ClampValueModifier(int.MaxValue, 0, stats[StatTypes.MAP]));
	}

	void OnMAPDidChange(object sender, object args)
	{
		int oldMAP = (int)args;
		if (MAP > oldMAP)
		{ AP += MAP - oldMAP; }
		else
		{ AP = Mathf.Clamp(AP, 0, MAP); }
	}

	void OnTurnBegan(object sender, object args)
	{
		if (AP < MAP)
		{ AP += Mathf.Max(Mathf.FloorToInt(MAP * 0.1f), 1); }
	}
	#endregion
}
