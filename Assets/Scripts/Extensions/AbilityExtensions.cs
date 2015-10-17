using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class AbilityExtensions
{
	#region Events
	public static event EventHandler<Check<int>> attackerSupportCheckEvent;
	public static event EventHandler<Check<int>> attackerStatusCheckEvent;
	public static event EventHandler<Check<int>> attackerEquipmentCheckEvent;
	public static event EventHandler<Check<int>> defenderSupportCheckEvent;
	public static event EventHandler<Check<int>> defenderStatusCheckEvent;
	public static event EventHandler<Check<int>> defenderEquipmentCheckEvent;
	#endregion

	public static HashSet<Tile> GetTilesInRange (this Ability a, Board board, Tile start)
	{
		return board.Search(start, delegate(Tile arg) {
			switch (a.rangeType)
			{
			case Ability.Ranges.Cone:
				return false;
			case Ability.Ranges.Constant:
				return arg.distance <= a.range;
			case Ability.Ranges.Weapon:
				return false;
			case Ability.Ranges.Line:
				return false;
			case Ability.Ranges.DblLine:
				return false;
			}
			return false;
		});
	}

	public static HashSet<Tile> GetTilesInArea (this Ability a, Board board, Tile start)
	{
		if (a.areaType != Ability.Areas.Specify)
			return new HashSet<Tile>();

		return board.Search(start, delegate(Tile arg) {
			return arg.distance <= a.horArea && Mathf.Abs(arg.height - start.height) <= a.verArea;
		});
	}

	public static int PredictDamage (this Ability a, Effect e, Unit owner, Unit target)
	{
		return Damage(a, e, owner, target, true);
	}

	public static int CalculateDamage (this Ability a, Effect e, Unit owner, Unit target)
	{
		return Damage(a, e, owner, target, false);
	}

	static int Damage (Ability a, Effect e, Unit owner, Unit target, bool isPrediction)
	{
		// Step 1. Get base attack power
		int baseAttackPower = (a.powerType == Ability.Powers.MPow) ? owner.GetStat(Stats.MPow) : owner.GetStat(Stats.WAtk);
		
		// Step 1a. Apply the mission item bonus
		baseAttackPower += baseAttackPower * GetMissionItemBonus(owner) / 100;
		
		// Step 2. Attacker's support check
		baseAttackPower = ReturnEvent<int>(a, owner, target, attackerSupportCheckEvent, baseAttackPower);
		
		// Step 3. Attacker's status check
		baseAttackPower = ReturnEvent<int>(a, owner, target, attackerStatusCheckEvent, baseAttackPower);
		
		// Step 4. Attacker's equipment check
		baseAttackPower = ReturnEvent<int>(a, owner, target, attackerEquipmentCheckEvent, baseAttackPower);
		
		// Step 5. Cap
		baseAttackPower = Mathf.Min(baseAttackPower, 999);
		
		// Step 6. Get base defense power
		int baseDefensePower = (a.powerType == Ability.Powers.MPow) ? target.GetStat(Stats.MRes) : target.GetStat(Stats.WDef);
		
		// Step 6.a Apply the mission item bonus
		baseDefensePower += baseDefensePower * GetMissionItemBonus(target) / 100;
		
		// Step 7. Target's support check
		baseDefensePower = ReturnEvent<int>(a, owner, target, defenderSupportCheckEvent, baseDefensePower);
		
		// Step 8. Target's status check
		baseDefensePower = ReturnEvent<int>(a, owner, target, defenderStatusCheckEvent, baseDefensePower);
		
		// Step 9. Target's equipment check
		baseDefensePower = ReturnEvent<int>(a, owner, target, defenderEquipmentCheckEvent, baseDefensePower);
		
		// Step 10. Defense Cap
		baseDefensePower = Mathf.Min(baseDefensePower, 999);
		
		// Step 11. Base Damage
		int damage = Mathf.Max(baseAttackPower - (baseDefensePower / 2), 1);
		
		// Step 12. Get Ability Power
		int power = Mathf.Max(a.power, 1); // TODO: get weapon power if applicable
		
		// Step 13. Apply Power Bonus
		damage = Mathf.Max(power * damage / 100, 1);
		
		// Step 14. Elemental Check
		Elements element = a.element;
		if (element == Elements.None && a.powerType == Ability.Powers.Weapon)
		{
			// TODO: use charge of primary weapon
		}
		
		if (element != Elements.None)
		{
			// 14 b. Retrieve Target's Resistance (native and equipment)
			Resistances baseResistance = Resistances.Normal; // TODO: implement
			
			// 14 c. Element enhancement from equipment
			// 14 d. Geomancy check
			// 14 e. Mission item check
			// 14 f. Apply damage bonus
			switch (baseResistance)
			{
			case Resistances.Weak:
				damage *= 3 / 2;
				break;
			case Resistances.Normal:
				break;
			case Resistances.Resist:
				damage /= 2;
				break;
			case Resistances.Null:
				damage = 0;
				break;
			case Resistances.Absorb:
				damage = -damage;
				break;
			}
		}
		
		// Step 15. Critical Hit
//		if (false) // TODO: Determine critical hit chance
//			damage *= 3 / 2;
		
		// Step 16. Expert Guard
//		if (false) // TODO
//			damage = 0;
		
		// Step 17. Random Variance
		if (true) // Skip when this is a prediction
		{
			int rnd = damage / 10;
			damage += UnityEngine.Random.Range(0, 2 * rnd) - rnd;
		}
		
		// Step 18. Unknown
//		if (false)
//			damage *= 3 / 2;
		
		// Step 19. Weapon Effects
//		if (false) // check weapon drain HP and Undead target
//			damage = -damage;
//		if (false) // check weapon heal on attack
//			damage = -damage;
		
		// Step 20. Cap Damage
		damage = Mathf.Clamp(damage, -999, 999);
		
		return damage;
	}

	static int GetMissionItemBonus (Unit owner)
	{
		// TODO: If Unit is in party and Mission item used
		return 100;
	}

	static T ReturnEvent<T> (Ability owner, Unit attacker, Unit defender, EventHandler<Check<T>> handler, T info)
	{
		if (handler != null)
		{
			Check<T> check = new Check<T>(owner.effects[0], attacker, defender, info);
			handler(owner.effects[0], check);
			return check.info;
		}
		return info;
	}
}