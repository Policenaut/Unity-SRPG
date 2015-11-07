using UnityEngine;
using System;
using System.Collections;

public static class StatsExtensions
{
	public static event EventHandler<InfoEventArgs<int>> statCheckEvent;

	public static int BaseStat (this Unit unit, StatTypes type)
	{
		switch (type)
		{
		case StatTypes.HP:
			return unit.HP;
		case StatTypes.MP:
			return unit.MP;
		case StatTypes.WAtk:
			return unit.WAtk;
		case StatTypes.WDef:
			return unit.WDef;
		case StatTypes.MPow:
			return unit.MPow;
		case StatTypes.MRes:
			return unit.MRes;
		case StatTypes.Spd:
			return unit.Spd;
		case StatTypes.AP:
			return unit.AP;
		case StatTypes.Mov:
			return unit.Mov;
		case StatTypes.Jmp:
			return unit.Jmp;
		case StatTypes.Evd:
			return unit.Evd;
		case StatTypes.SRes:
			return unit.SRes;
		}
		return 0;
	}
	
	public static int ModifiedStat (this Unit unit, StatTypes type)
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
