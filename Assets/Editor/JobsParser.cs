using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

public class JobsParser : EditorWindow 
{
	[MenuItem("Pre Production/Parse Jobs")]
	public static void Parse()
	{
		CreateJobs();
		CreateCategories();
		
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	static void CreateJobs ()
	{
		string readPath = string.Format("{0}/Settings/JobsFAQ.txt", Application.dataPath);
		if (!File.Exists(readPath))
		{
			Debug.LogError("Missing Data: " + readPath);
			return;
		}
		
		string[] readText = File.ReadAllLines(readPath);
		for (int i = 0; i < readText.Length; ++i)
			CreateJob(readText[i]);
	}

	static void CreateCategories ()
	{
		string[] names = new string[]
		{
			"HumanJobs",
			"MiscJobs",
			"MonsterJobs",
			"MoogleJobs",
			"NuMouJobs",
			"VieraJobs"
		};

		for (int i = 0; i < names.Length; ++i)
		{
			string readPath = string.Format("{0}/Settings/{1}FAQ.txt", Application.dataPath, names[i]);
			if (!File.Exists(readPath))
			{
				Debug.LogError("Missing Data: " + readPath);
				return;
			}

			Contents contents = ScriptableObject.CreateInstance<Contents>();
			contents.list = File.ReadAllLines(readPath);

			string path = string.Format("Assets/Resources/Job Categories/{0}.asset", names[i]);
			AssetDatabase.CreateAsset(contents, path);
		}
	}

	static void CreateJob (string line)
	{
		Job job = ScriptableObject.CreateInstance<Job>();

		job.name = line.Substring(0, 15).Trim();
		ParseStat(line.Substring(15, 7), job, Stats.MHP);
		ParseStat(line.Substring(22, 9), job, Stats.MMP);
		ParseStat(line.Substring(31, 9), job, Stats.WAtk);
		ParseStat(line.Substring(40, 9), job, Stats.WDef);
		ParseStat(line.Substring(49, 9), job, Stats.MPow);
		ParseStat(line.Substring(58, 9), job, Stats.MRes);
		ParseStat(line.Substring(67, 9), job, Stats.Spd);

		string path = string.Format("Assets/Resources/Jobs/{0}.asset", job.name);
		AssetDatabase.CreateAsset(job, path);
	}

	static void ParseStat (string line, Job job, Stats type)
	{
		string[] elements = line.Split('/');
		job.SetBaseStat( type, Convert.ToInt32( elements[0].Trim() ) );
		string[] elements2 = elements[1].Split('.');
		job.SetBaseGrowth( type, Convert.ToInt32( elements2[0].Trim() ) );
		job.SetBonusGrowth( type, Convert.ToSingle( elements2[1].Trim() ) / 10f );
	}
}