using UnityEngine;
using System;
using System.Collections;

public static class HitRatesExtensions
{
	#region Events
	public static event EventHandler<Check<bool>> automaticHitCheckEvent;
	public static event EventHandler<Check<bool>> automaticMissCheckEvent;
	public static event EventHandler<Check<int>> boostCheckEvent;
	public static event EventHandler<Check<bool>> immunityCheckEvent;
	public static event EventHandler<Check<bool>> vulnerabilityCheckEvent;
	public static event EventHandler<Check<float>> finalCheckEvent;
	#endregion

	#region Public
	public static int Chance (this Ability ability, Unit attacker, Unit defender)
	{
		for (int i = 0; i < ability.effects.Count; ++i)
		{
			Effect effect = ability.effects[i];
			if (CanTarget(effect, attacker, defender))
			{
				switch (effect.rate)
				{
				case Effect.HitRates.AType:
					return HitRateForAType(effect, attacker, defender);
				case Effect.HitRates.SType:
					return HitRateForSType(effect, attacker, defender);
				default:
					return 100;
				}
			}
		}
		return 0;
	}
	#endregion

	#region Private
	static bool CanTarget (Effect effect, Unit attacker, Unit defender)
	{
		// Not implemented yet...
//		switch (effect.target)
//		{
//		case Effect.Targets.Default:
//			break;
//		}
		return true;
	}

	static T ReturnEvent<T> (Effect effect, Unit attacker, Unit defender, EventHandler<Check<T>> handler, T info)
	{
		if (handler != null)
		{
			Check<T> check = new Check<T>(effect, attacker, defender, info);
			handler(effect, check);
			return check.info;
		}
		return info;
	}

	static int HitRateForAType (Effect effect, Unit attacker, Unit defender)
	{
		int retValue;
		
		// Step 1. Automatic Hit Conditions - CAN YOU GUARANTEE A HIT?
		if (ReturnEvent<bool>(effect, attacker, defender, automaticHitCheckEvent, false))
		{
			retValue = 100;
			goto Step8;
		}
		
		// Step 2. Reaction Check - CAN YOU GUARANTEE A MISS?
		if (ReturnEvent<bool>(effect, attacker, defender, automaticMissCheckEvent, false))
		{
			retValue = 0;
			goto Step9;
		}

		// Step 3. Retrieve Target's Evade stat - GET BASE EVADE STAT (FROM JOB & EQUIPMENT)
		int defenderEvade = defender.ModifiedStat( Stats.Evd );
		 
		// Step 4. Work out Relative Facing - MODIFY EVADE STAT DUE TO ANGLE OF ATTACK
		Facings f = attacker.AngleOfAttack(defender);
		if (f == Facings.Side)
			defenderEvade /= 2;
		else if (f == Facings.Back)
			defenderEvade /= 4;

		// Step 5. Status Check - MODIFY EVADE DUE TO STATUS BOOSTS AND AILMENTS
		defenderEvade += ReturnEvent<int>(effect, attacker, defender, boostCheckEvent, 0);
		
		// Step 6. Cap Evade - CAP VALUE TO LEGAL RANGE
		defenderEvade = Mathf.Clamp(defenderEvade, 5, 95);
		
		// Step 7. Work out Hit Rate based on Evade
		retValue = 100 - defenderEvade;
		
		// Step 8. Final Checks
	Step8:
			retValue = Mathf.FloorToInt(retValue * ReturnEvent<float>(effect, attacker, defender, finalCheckEvent, 1));
		
	Step9:
			return retValue;
	}

	static int HitRateForSType (Effect effect, Unit attacker, Unit defender)
	{
		int retValue;

		// Step 1. Automatic Miss Conditions - IS TARGET IMMUNE?
		if (ReturnEvent<bool>(effect, attacker, defender, immunityCheckEvent, false))
		{
			retValue = 0;
			goto Step9;
		}

		// Step 2. Automatic Hit Conditions - CAN YOU GUARANTEE A HIT?
		if (ReturnEvent<bool>(effect, attacker, defender, vulnerabilityCheckEvent, false))
		{
			retValue = 100;
			goto Step8;
		}

		// Step 3. Retrieve Target's Evade stat - GET BASE EVADE STAT (FROM JOB & EQUIPMENT)
		int defenderResistance = defender.ModifiedStat( Stats.SRes );

		// Step 4. Status Check - MODIFY EVADE DUE TO STATUS BOOSTS AND AILMENTS
		defenderResistance += ReturnEvent<int>(effect, attacker, defender, boostCheckEvent, 0);

		// Step 5. Work out Relative Facing - MODIFY RESISTANCE STAT DUE TO ANGLE OF ATTACK
		Facings f = attacker.AngleOfAttack(defender);
		if (f == Facings.Side)
			defenderResistance -= 10;
		else if (f == Facings.Back)
			defenderResistance -= 20;

		// Step 6. Cap Resistance - CAP VALUE TO LEGAL RANGE
		defenderResistance = Mathf.Clamp(defenderResistance, 0, 100);

		// Step 7. Work out Hit Rate based on Resistance
		retValue = 100 - defenderResistance;

		// Step 8. Final Checks
	Step8:
			retValue = Mathf.FloorToInt(retValue * ReturnEvent<float>(effect, attacker, defender, finalCheckEvent, 1));

	Step9:
		return retValue;
	}
	#endregion
}