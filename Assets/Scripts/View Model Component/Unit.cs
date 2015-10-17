using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Any person or monster on the battlefield
/// </summary>
public class Unit : MonoBehaviour
{
	public static event EventHandler<StatChange> statChanged;

	public int Lvl 
	{ 
		get { return GetStat(Stats.Lvl); }
		set { SetStat(Stats.Lvl, value); }
	}

	public int HP
	{ 
		get { return GetStat(Stats.HP); }
		set { SetStat(Stats.HP, value); }
	}
	
	public int MaxHP
	{ 
		get { return GetStat(Stats.MHP); }
		set { SetStat(Stats.MHP, value); }
	}

	public int MP
	{ 
		get { return GetStat(Stats.MP); }
		set { SetStat(Stats.MP, value); }
	}

	public int MaxMP
	{ 
		get { return GetStat(Stats.MMP); }
		set { SetStat(Stats.MMP, value); }
	}

	public int WAtk
	{ 
		get { return GetStat(Stats.WAtk); }
		set { SetStat(Stats.WAtk, value); }
	}

	public int WDef
	{
		get { return GetStat(Stats.WDef); }
		set { SetStat(Stats.WDef, value); }
	}

	public int MPow
	{
		get { return GetStat(Stats.MPow); }
		set { SetStat(Stats.MPow, value); }
	}

	public int MRes
	{
		get { return GetStat(Stats.MRes); }
		set { SetStat(Stats.MRes, value); }
	}

	public int Spd
	{
		get { return GetStat(Stats.Spd); }
		set { SetStat(Stats.Spd, value); }
	}

	public int Mov
	{
		get { return GetStat(Stats.Mov); }
		set { SetStat(Stats.Mov, value); }
	}

	public int Jmp
	{
		get { return GetStat(Stats.Jmp); }
		set { SetStat(Stats.Jmp, value); }
	}

	public int Evd
	{
		get { return GetStat(Stats.Evd); }
		set { SetStat(Stats.Evd, value); }
	}

	public int SRes
	{
		get { return GetStat(Stats.SRes); }
		set { SetStat(Stats.SRes, value); }
	}

	public Alliances alliance;
	public Directions Dir;
	public Tile tile;
	public List<SkillSet> capability = new List<SkillSet>();
	public Job job;
	int[] _stats = new int[ (int)Stats.Count ];
	
	public bool IsAlly (Unit other)
	{
		return alliance == other.alliance;
	}

	public void Place (Tile target)
	{
		// Make sure old tile location is not still pointing to this unit
		if (tile != null && tile.unit == this)
			tile.unit = null;

		// Link unit and tile references
		tile = target;

		if (target != null)
			target.unit = this;
	}

	public void Match ()
	{
		transform.localPosition = tile.center;
		transform.localEulerAngles = Dir.ToEuler();
	}

	public int GetStat (Stats type)
	{
		int index = (int)type;
		return _stats[index];
	}

	public void SetStat (Stats type, int value)
	{
		int index = (int)type;
		if (_stats[index] == value)
			return;

		int fromValue = _stats[index];
		_stats[index] = value;
		if (statChanged != null)
			statChanged(this, new StatChange(type, fromValue, value));
	}

	public void SetBaseStats ()
	{
		for (int i = 0; i < Job.statOrder.Length; ++i)
		{
			Stats s = Job.statOrder[i];
			SetStat(s, job.GetBaseStat(s));
		}
		Lvl = 1;
		HP = MaxHP;
		MP = MaxMP;
	}

	public void LevelUp ()
	{
		for (int i = 0; i < Job.statOrder.Length; ++i)
		{
			Stats s = Job.statOrder[i];
			int current = GetStat(s);
			int boost = job.GetBaseGrowth(s);
			int bonus = UnityEngine.Random.value <= job.GetBonusGrowth(s) ? 1 : 0;
			int total = current + boost + bonus;
			SetStat(s, total);
		}
		Lvl++;
		HP = MaxHP;
		MP = MaxMP;
	}
}