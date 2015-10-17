using UnityEngine;
using System.Collections;

public static class UnitFactory
{
	static string[] heroJobCategories = new string[]
	{
		"HumanJobs",
		"MoogleJobs",
		"NuMouJobs",
		"VieraJobs"
	};

	public static Unit CreateHero (int lvl)
	{
		return CreateUnit("Hero", RandomHeroCategory(), lvl);
	}

	public static Unit CreateMonster (int lvl)
	{
		return CreateUnit("Monster", "MonsterJobs", lvl);
	}

	static Unit CreateUnit (string prefab, string category, int lvl)
	{
		Unit unit = LoadBaseUnit(prefab);
		unit.job = RandomJob(category);
		unit.SetBaseStats();
		for (int i = 1; i < lvl; ++i)
			unit.LevelUp();
		return unit;
	}

	static Unit LoadBaseUnit (string name)
	{
		GameObject prefab = Resources.Load<GameObject>("Units/" + name);
		GameObject instance = GameObject.Instantiate(prefab) as GameObject;
		return instance.GetComponent<Unit>();
	}

	static string RandomHeroCategory ()
	{
		int index = UnityEngine.Random.Range(0, heroJobCategories.Length);
		return heroJobCategories[index];
	}

	static Job RandomJob (string category)
	{
		Contents jobs = Resources.Load<Contents>("Job Categories/" + category);
		int index = UnityEngine.Random.Range(0, jobs.list.Length);
		string name = jobs.list[index];
		return Resources.Load<Job>("Jobs/" + name);
	}
}