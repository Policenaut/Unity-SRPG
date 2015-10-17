using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ability : ScriptableObject
{
	public enum Powers
	{
		None, // Not applicable
		MPow, // Power based on magical constant
		WAtk, // Power based on physical constant
		Weapon, // Power based on equipped weapon
	}

	public enum Charges
	{
		None, // Not applicable
		Elemental, // Charge is a specified constant
		Weapon, // Charge is based on equipped weapon
	}

	public enum Ranges
	{
		Self, // Range is limited to the caster
		Cone, // Range grows in a conical fashion from caster
		Constant, // Range is a specified constant distance from caster
		Weapon, // Range is based on the equipped weapon's range
		Line, // Range extends in a line from the caster in the facing direction
		DblLine, // Range extends in a line in front and behind the casters facing direction
		Infinite, // Range covers entire field
	}

	public enum Areas
	{
		Self,
		OneUnit,
		Specify, // ex: r1/v1 
		Infinite,
	}
	
	public enum Notes
	{
		Offensive, // considered an attack ability (used in laws)
		Reflectable, // reflect status sends ability back on caster
		ByPass, // Ignore-Reaction: This ability never triggers any Reaction, no matter what the ability does.
		Silenceable, // cannot be used under silence status
		Stealable, // Steal: Ability can be used on this skill, *PROVIDING* the caster could learn it normally in one of his/her jobs.  Some abilities are Stealable, but no unit can do so since Thieves can only be Humans and Moogles only.
		Counter, // R-Ab:Return Magic: The Reaction Ability Return Magic will work against this ability.
		Absorb, // R-Ab:Absorb MP: The Reaction Ability Absorb MP will work against this ability.
	}

	public string title;
	public Powers powerType;
	public int power;
	public int mpCost;
	public Charges charge;
	public Elements element;
	public Ranges rangeType;
	public int range;
	public Areas areaType;
	public int horArea;
	public int verArea;
	public bool excludeSelf;
	public List<Effect> effects = new List<Effect>();
	public List<Notes> notes = new List<Notes>();
}