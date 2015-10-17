using UnityEngine;
using System.Collections;

public class Job : ScriptableObject 
{
	public static Stats[] statOrder = new Stats[] 
	{
		Stats.MHP,
		Stats.MMP,
		Stats.WAtk,
		Stats.WDef,
		Stats.MPow,
		Stats.MRes,
		Stats.Spd,
		Stats.MAP
	};

	public int[] baseStats = new int[statOrder.Length];
	public int[] baseGrowth = new int[statOrder.Length];
	public float[] bonusGrowth = new float[statOrder.Length];

	public int GetBaseStat (Stats type)
	{
		int index = IndexForStat(type);
		return index != -1 ? baseStats[index] : 0;
	}

	public void SetBaseStat (Stats type, int value)
	{
		int index = IndexForStat(type);
		if (index != -1)
			baseStats[index] = value;
	}

	public int GetBaseGrowth (Stats type)
	{
		int index = IndexForStat(type);
		return index != -1 ? baseGrowth[index] : 0;
	}

	public void SetBaseGrowth (Stats type, int value)
	{
		int index = IndexForStat(type);
		if (index != -1)
			baseGrowth[index] = value;
	}

	public float GetBonusGrowth (Stats type)
	{
		int index = IndexForStat(type);
		return index != -1 ? bonusGrowth[index] : 0;
	}
	
	public void SetBonusGrowth (Stats type, float value)
	{
		int index = IndexForStat(type);
		if (index != -1)
			bonusGrowth[index] = value;
	}

	int IndexForStat (Stats type)
	{
		switch (type)
		{
		case Stats.MHP:
			return 0;
		case Stats.MMP:
			return 1;
		case Stats.WAtk:
			return 2;
		case Stats.WDef:
			return 3;
		case Stats.MPow:
			return 4;
		case Stats.MRes:
			return 5;
		case Stats.Spd:
			return 6;
		case Stats.MAP:
			return 7;
		}
		return -1;
	}
}