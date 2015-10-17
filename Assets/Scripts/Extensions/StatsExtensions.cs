using UnityEngine;
using System;
using System.Collections;

public static class StatsExtensions
{
	public static event EventHandler<InfoEventArgs<int>> statCheckEvent;

	public static int BaseStat (this Unit unit, Stats type)
	{
		switch (type)
		{
		case Stats.HP:
			return unit.HP;
		case Stats.MP:
			return unit.MP;
		case Stats.WAtk:
			return unit.WAtk;
		case Stats.WDef:
			return unit.WDef;
		case Stats.MPow:
			return unit.MPow;
		case Stats.MRes:
			return unit.MRes;
		case Stats.Spd:
			return unit.Spd;
		case Stats.Mov:
			return unit.Mov;
		case Stats.Jmp:
			return unit.Jmp;
		case Stats.Evd:
			return unit.Evd;
		case Stats.SRes:
			return unit.SRes;
		}
		return 0;
	}
	
	public static int ModifiedStat (this Unit unit, Stats type)
	{
		if (statCheckEvent != null)
		{
			InfoEventArgs<int> e = new InfoEventArgs<int>( unit.BaseStat(type) );
			statCheckEvent(unit, e);
			return e.info;
		}
		return unit.BaseStat(type);
	}
}
