using UnityEngine;
using System.Collections;

public class Check<T> : InfoEventArgs<T> 
{
	public readonly Effect effect;
	public readonly Unit attacker;
	public readonly Unit defender;
	
	public Check (Effect effect, Unit attacker, Unit defender)
	{
		this.effect = effect;
		this.attacker = attacker;
		this.defender = defender;
	}

	public Check (Effect effect, Unit attacker, Unit defender, T info) : base (info)
	{
		this.effect = effect;
		this.attacker = attacker;
		this.defender = defender;
	}
}