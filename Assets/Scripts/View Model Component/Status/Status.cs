using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Status : MonoBehaviour
{
	public const string AddedNotification = "Status.AddedNotification";
	public const string RemovedNotification = "Status.RemovedNotification";

	public U Add<T, U> () where T : StatusEffect where U : StatusCondition
	{
		T effect = GetComponentInChildren<T>();

		if (effect == null)
		{
			effect = gameObject.AddChildComponent<T>();
			this.PostNotification(AddedNotification, effect);
		}

		return effect.gameObject.AddChildComponent<U>();
	}

	public void Remove (StatusCondition target)
	{
		StatusEffect effect = target.GetComponentInParent<StatusEffect>();

		target.transform.SetParent(null);
		Destroy(target.gameObject);

		StatusCondition condition = effect.GetComponentInChildren<StatusCondition>();
		if (condition == null)
		{
			effect.transform.SetParent(null);
			Destroy(effect.gameObject);
			this.PostNotification(RemovedNotification, effect);
		}
	}
}