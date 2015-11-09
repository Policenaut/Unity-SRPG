using UnityEngine;
using System.Collections;

public class Rank : MonoBehaviour
{
	#region Consts
	public const int minLevel = 1;
	public const int maxLevel = 99;
	public const int maxExperience = 999999;
	#endregion

	#region Fields / Properties
	public int LVL
	{
		get { return stats[StatTypes.LVL]; }
	}

	public int EXP
	{
		get { return stats[StatTypes.EXP]; }
		set { stats[StatTypes.EXP] = value; }
	}

	public float LevelPercent
	{
		get { return (float)(LVL - minLevel) / (float)(maxLevel - minLevel); }
	}

	Stats stats;
	#endregion

	#region MonoBehaviour
	void Awake()
	{
		stats = GetComponent<Stats>();
	}

	void OnEnable()
	{
		this.AddObserver(OnExpWillChange, Stats.WillChangeNotification(StatTypes.EXP), stats);
		this.AddObserver(OnExpDidChange, Stats.DidChangeNotification(StatTypes.EXP), stats);
	}

	void OnDisable()
	{
		this.RemoveObserver(OnExpWillChange, Stats.WillChangeNotification(StatTypes.EXP), stats);
		this.RemoveObserver(OnExpDidChange, Stats.DidChangeNotification(StatTypes.EXP), stats);
	}
	#endregion

	#region Event Handlers
	void OnExpWillChange(object sender, object args)
	{
		ValueChangeException vce = args as ValueChangeException;
		vce.AddModifier(new ClampValueModifier(int.MaxValue, EXP, maxExperience));
	}

	void OnExpDidChange(object sender, object args)
	{
		stats.SetValue(StatTypes.LVL, LevelForExperience(EXP), false);
	}
	#endregion

	#region Public
	public static int ExperienceForLevel(int level)
	{
		float levelPercent = Mathf.Clamp01((float)(level - minLevel) / (float)(maxLevel - minLevel));
		return (int)EasingEquations.EaseInQuad(0, maxExperience, levelPercent);
	}

	public static int LevelForExperience(int exp)
	{
		int lvl = maxLevel;
		for (; lvl >= minLevel; --lvl)
		{
			if (exp >= ExperienceForLevel(lvl))
			{ break; }
		}
		return lvl;
	}

	public void Init(int level)
	{
		stats.SetValue(StatTypes.LVL, level, false);
		stats.SetValue(StatTypes.EXP, ExperienceForLevel(level), false);
	}
	#endregion
}