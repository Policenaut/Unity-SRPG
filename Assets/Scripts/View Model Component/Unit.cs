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
		get { return GetStat(StatTypes.Lvl); }
		set { SetStat(StatTypes.Lvl, value); }
	}

	public int HP
	{ 
		get { return GetStat(StatTypes.HP); }
		set { SetStat(StatTypes.HP, value); }
	}
	
	public int MaxHP
	{ 
		get { return GetStat(StatTypes.MHP); }
		set { SetStat(StatTypes.MHP, value); }
	}

	public int MP
	{ 
		get { return GetStat(StatTypes.MP); }
		set { SetStat(StatTypes.MP, value); }
	}

	public int MaxMP
	{ 
		get { return GetStat(StatTypes.MMP); }
		set { SetStat(StatTypes.MMP, value); }
	}

	public int AP
	{
		get { return GetStat(StatTypes.AP); }
		set { SetStat(StatTypes.AP, value); }
	}

	public int MaxAP
	{
		get { return GetStat(StatTypes.MAP); }
		set { SetStat(StatTypes.MAP, value); }
	}

	public int WAtk
	{ 
		get { return GetStat(StatTypes.WAtk); }
		set { SetStat(StatTypes.WAtk, value); }
	}

	public int WDef
	{
		get { return GetStat(StatTypes.WDef); }
		set { SetStat(StatTypes.WDef, value); }
	}

	public int MPow
	{
		get { return GetStat(StatTypes.MPow); }
		set { SetStat(StatTypes.MPow, value); }
	}

	public int MRes
	{
		get { return GetStat(StatTypes.MRes); }
		set { SetStat(StatTypes.MRes, value); }
	}

	public int Spd
	{
		get { return GetStat(StatTypes.Spd); }
		set { SetStat(StatTypes.Spd, value); }
	}

	public int Mov
	{
		get { return GetStat(StatTypes.Mov); }
		set { SetStat(StatTypes.Mov, value); }
	}

	public int Jmp
	{
		get { return GetStat(StatTypes.Jmp); }
		set { SetStat(StatTypes.Jmp, value); }
	}

	public int Evd
	{
		get { return GetStat(StatTypes.Evd); }
		set { SetStat(StatTypes.Evd, value); }
	}

	public int SRes
	{
		get { return GetStat(StatTypes.SRes); }
		set { SetStat(StatTypes.SRes, value); }
	} 


	public Alliances alliance;
	public Directions Dir;
	public Tile tile;
	public List<SkillSet> capability = new List<SkillSet>();
	public Job job;
	int[] _stats = new int[ (int)StatTypes.Count ];
	
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

	public int GetStat (StatTypes type)
	{
		int index = (int)type;
		return _stats[index];
	}

	public void SetStat (StatTypes type, int value)
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
			StatTypes s = Job.statOrder[i];
			SetStat(s, job.GetBaseStat(s));
		}
		Lvl = 1;
		HP = MaxHP;
		MP = MaxMP;
		AP = MaxAP;
	}

	public void LevelUp ()
	{
		for (int i = 0; i < Job.statOrder.Length; ++i)
		{
			StatTypes s = Job.statOrder[i];
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