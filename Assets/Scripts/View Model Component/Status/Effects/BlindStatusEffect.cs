using UnityEngine;
using System.Collections;

public class BlindStatusEffect : StatusEffect 
{
	void OnEnable ()
	{
		this.AddObserver( OnHitRateStatusCheck, HitRate.StatusCheckNotification );
	}
	
	void OnDisable ()
	{
		this.RemoveObserver( OnHitRateStatusCheck, HitRate.StatusCheckNotification );
	}

	void OnHitRateStatusCheck (object sender, object args)
	{
		Info<Unit, Unit, int> info = args as Info<Unit, Unit, int>;
		Unit owner = GetComponentInParent<Unit>();
		if (owner == info.arg0)
		{
			// The attacker is blind
			info.arg2 += 50;
		}
		else if (owner == info.arg1)
		{
			// The defender is blind
			info.arg2 -= 20;
		}
	}
}