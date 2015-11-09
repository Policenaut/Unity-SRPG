using UnityEngine;
using System.Collections;

public class AbsorbDamageAbilityEffectTarget : BaseAbilityEffect 
{
	#region Fields
	public int trackedSiblingIndex;
	BaseAbilityEffect effect;
	int amount;
	#endregion

	#region MonoBehaviour
	void Awake ()
	{
		effect = GetTrackedEffect();
	}

	void OnEnable ()
	{
		this.AddObserver(OnEffectHit, BaseAbilityEffect.HitNotification, effect);
		this.AddObserver(OnEffectMiss, BaseAbilityEffect.MissedNotification, effect);
	}

	void OnDisable ()
	{
		this.RemoveObserver(OnEffectHit, BaseAbilityEffect.HitNotification, effect);
		this.RemoveObserver(OnEffectMiss, BaseAbilityEffect.MissedNotification, effect);
	}
	#endregion

	#region Base Ability Effect
	public override int Predict (Tile target)
	{
		return 0;
	}
	
	protected override int OnApply (Tile target)
	{
		Stats s = GetComponentInParent<Stats>();
		s[StatTypes.HP] += amount;
		return amount;
	}
	#endregion

	#region Event Handlers
	void OnEffectHit (object sender, object args)
	{
		amount = (int)args * -1;
	}

	void OnEffectMiss (object sender, object args)
	{
		amount = 0;
	}
	#endregion

	#region Private
	BaseAbilityEffect GetTrackedEffect ()
	{
		Transform owner = GetComponentInParent<Ability>().transform;
		if (trackedSiblingIndex >= 0 && trackedSiblingIndex < owner.childCount)
		{
			Transform sibling = owner.GetChild(trackedSiblingIndex);
			return sibling.GetComponent<BaseAbilityEffect>();
		}
		return null;
	}
	#endregion
}