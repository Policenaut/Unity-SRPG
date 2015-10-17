using UnityEngine;
using System;
using System.Collections;

public class Death : MonoBehaviour 
{
	#region Events
	public static event EventHandler deathEvent;
	public static event EventHandler reviveEvent;
	#endregion

	#region Static Constructor
	static Death ()
	{
		Unit.statChanged += OnStatChanged;
	}
	#endregion

	#region MonoBehaviour
	void OnEnable ()
	{
		GetComponent<Unit>().Place(null);
		transform.localScale = Vector3.zero;
		if (deathEvent != null)
			deathEvent(this, EventArgs.Empty);
	}
	
	void OnDisable ()
	{
		transform.localScale = Vector3.one;
		if (reviveEvent != null)
			reviveEvent(this, EventArgs.Empty);
	}
	#endregion

	#region Event Handlers
	static void OnStatChanged (object sender, StatChange e)
	{
		if (e.type == Stats.HP)
		{
			if (e.toValue <= 0)
				Inflict(sender as Unit);
			else if (e.fromValue <= 0 && e.toValue > 0)
				Release(sender as Unit);
		}
	}
	#endregion

	#region Private
	static void Inflict (Unit unit)
	{
		Death d = unit.GetComponent<Death>();
		if (d == null)
			unit.gameObject.AddComponent<Death>();
	}

	static void Release (Unit unit)
	{
		Death d = unit.GetComponent<Death>();
		if (d != null)
			Destroy(d);
	}
	#endregion
}
