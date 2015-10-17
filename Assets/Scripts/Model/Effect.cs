using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class Effect
{
	public enum HitRates
	{
		AType,
		SType,
		Full // 100%H
	}

	public enum Targets
	{
		Default, // Normal unit targetting, as if you were using the attack command.  Will ignore KOed units, among other things.
		KOedUnit, // Will only target a KOed unit.
		Self, // Will only target the Caster.
		NotSelf, // Will target anything but the caster.
		Enemy, // Will only target an enemy unit.
		Undead, // Will only target Zombies, Vampires or units with the Zombie Status
		NonUndead, // Will only target units that are not Undead.
		Animal, // Will only target monsters.
		NonAnimal, // Will only target units that are not monsters.
		Dragon, // Will only target dragons.
		NonDragon, // Will only target units that are not dragons.
		Gadget, // Has a 50/101 chance of targetting all enemies, and a corresponding 51/101 chance of targetting all enemies.
		EqpSword, // Will only target units wielding a Sword, Blade, Saber, Knightsword, Greatsword, Broadsword, Knife, Rapier or Katana.
		LvlDivBy3, // Will only target units whose level is divisible by 3.
		SmeLvlDay, // Will only target units where the unit figure of their level is identical to the unit figure of the current day of the month.  For example, on day 17 of Bardmoon, units of L7, 17, 27, 37 and 47 will be effected.
		Judge, // Will only target the judge
	}

	public enum Dependencies
	{
		None,
		NonUndead, // Will only target units who are not undead
		LvlDivBy5, // Will only target units whose level is divisible by 5.
		SameLvDigit, // Will only target units where the unit figure of their level is identical to the unit figure of the caster.  For example, a L32 caster can effect units of L2, 12, 22, 32 and 42 with this effect.
		Eye2Eye, // The caster must be attacking the target from the Front.  Any attack from the Side or Rear will cause this effect to fail. 
		Eff1Dep, // This effect will only be allowed to target the unit if Effect 1 succeeded against the target.  In the example, Mug has an AType Hit Rate chance of stealing gil from the target.  If that fails, then Effect 2 (which has an Eff1Dep validity check) will always fail against the target.
		Eff2Dep, // Same as Eff1Dep, except that Effect 2 must succeed for this Effect to be able to hit the target.
	}

	public HitRates rate;
	public Targets target;
	public Dependencies dependency;
	public string result;
}