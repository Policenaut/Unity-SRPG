using UnityEngine;
using System.Collections;

public class Job : ScriptableObject 
{
	public static StatTypes[] statOrder = new StatTypes[] 
	{
		StatTypes.MHP,
		StatTypes.MMP,
		StatTypes.WAtk,
		StatTypes.WDef,
		StatTypes.MPow,
		StatTypes.MRes,
		StatTypes.Spd,
		StatTypes.MAP
	};

	public int[] baseStats = new int[statOrder.Length];
	public int[] baseGrowth = new int[statOrder.Length];
	public float[] bonusGrowth = new float[statOrder.Length];

	public int GetBaseStat (StatTypes type)
	{
		int index = IndexForStat(type);
		return index != -1 ? baseStats[index] : 0;
	}

	public void SetBaseStat (StatTypes type, int value)
	{
		int index = IndexForStat(type);
		if (index != -1)
			baseStats[index] = value;
	}

	public int GetBaseGrowth (StatTypes type)
	{
		int index = IndexForStat(type);
		return index != -1 ? baseGrowth[index] : 0;
	}

	public void SetBaseGrowth (StatTypes type, int value)
	{
		int index = IndexForStat(type);
		if (index != -1)
			baseGrowth[index] = value;
	}

	public float GetBonusGrowth (StatTypes type)
	{
		int index = IndexForStat(type);
		return index != -1 ? bonusGrowth[index] : 0;
	}
	
	public void SetBonusGrowth (StatTypes type, float value)
	{
		int index = IndexForStat(type);
		if (index != -1)
			bonusGrowth[index] = value;
	}

	int IndexForStat (StatTypes type)
	{
		switch (type)
		{
		case StatTypes.MHP:
			return 0;
		case StatTypes.MMP:
			return 1;
		case StatTypes.WAtk:
			return 2;
		case StatTypes.WDef:
			return 3;
		case StatTypes.MPow:
			return 4;
		case StatTypes.MRes:
			return 5;
		case StatTypes.Spd:
			return 6;
		case StatTypes.MAP:
			return 7;
		}
		return -1;
	}
}