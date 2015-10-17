using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

public class AbilitiesParser : EditorWindow 
{
	static HashSet<string> names = new HashSet<string>();
	static Dictionary<string, List<string>> categories = new Dictionary<string, List<string>>();

	[MenuItem("Pre Production/Parse Abilities")]
	public static void Parse()
	{
		names.Clear();
		categories.Clear();

		CreateAbilities();
		CreateCategories();

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	static void CreateAbilities ()
	{
		string readPath = string.Format("{0}/Settings/AbilitiesFAQ.txt", Application.dataPath);
		if (!File.Exists(readPath))
		{
			Debug.LogError("Missing Data: " + readPath);
			return;
		}
		
		string[] readText = File.ReadAllLines(readPath);
		for (int i = 0; i < readText.Length; ++i)
		{
			if (readText[i].StartsWith("+="))
			{
				for (int j = i + 1; j < readText.Length; ++j)
				{
					if (string.IsNullOrEmpty(readText[j]))
					{
						Entry(readText, i, j);
						i = j;
						break;
					}
				}
			}
//			break;
		}
	}

	static void CreateCategories ()
	{
		foreach (string key in categories.Keys)
		{
			Contents contents = ScriptableObject.CreateInstance<Contents>();
			contents.name = key;
			
			List<string> list = categories[key];
			contents.list = new string[list.Count];
			list.CopyTo(contents.list);
			
			string path = string.Format("Assets/Resources/Ability Categories/{0}.asset", key);
			AssetDatabase.CreateAsset(contents, path);
		}
	}

	static void Entry (string[] lines, int from, int to)
	{
		Ability ability = ScriptableObject.CreateInstance<Ability>();

		// Step 1: Parse the name from the title section
		string name, title;
		if (!ParseTitle(lines[from + 1], out name, out title)) return;
		ability.name = name;
		ability.title = title;

		// Step 2: Parse the Power, MP Cost, and Range
		ParseMain1(lines[from + 3], ability);

		// Step 3: Parse the Element and Area of effect
		ParseMain2(lines[from + 4], ability);

		// Step 4: Parse the effects
		ParseEffect(lines[from + 6], ability);
		ParseEffect(lines[from + 7], ability);
		ParseEffect(lines[from + 8], ability);
		ParseEffect(lines[from + 9], ability);

		// Step 5: Parse the notes
		ParseNotes1(lines[from + 11], ability);
		ParseNotes2(lines[from + 12], ability);

		string path = string.Format("Assets/Settings/Resources/Abilities/{0}.asset", name);
		AssetDatabase.CreateAsset(ability, path);
	}

	static bool ParseTitle (string line, out string name, out string title)
	{
		line = line.Replace("|", "");
		int index = line.IndexOf("  ");
		name = title = line.Substring(1, index - 1);

		// If the name contains invalid characters, remove them
		name = name.Replace(": ", "").Replace("?", "");

		// If the name is a duplicate, make it sequential
		if (names.Contains(name))
		{
			int seq = 0;
			string testName;
			do {
				seq++;
				testName = string.Format("{0}{1}", name, seq);
			} while (names.Contains(testName));
			name = testName;
		}

		string remainder = line.Substring(index).Trim();
		string[] cats = remainder.Split(',');
		for (int i = 0; i < cats.Length; ++i)
			cats[i] = cats[i].Trim().Replace("/", "-");

		// Catalog the name and category for later
		names.Add(name);
		foreach (string s in cats)
			AddCategory(s, name);

		return true;
	}

	static void ParseMain1 (string line, Ability ability)
	{
		line = line.Replace("|", "");
		int i1 = line.IndexOf("Power:");
		int i2 = line.IndexOf("MP:");
		int i3 = line.IndexOf("Range:");
		ParsePower(line.Substring(i1 + 6, i2 - (i1 + 6)).Trim(), ability);
		ParseMP(line.Substring(i2 + 3, i3 - (i2 + 3)).Trim(), ability);
		ParseRange(line.Substring(i3 + 6).Trim(), ability);
	}

	static void ParseMain2 (string line, Ability ability)
	{
		line = line.Replace("|", "");
		int i1 = line.IndexOf("Elem:");
		int i2 = line.IndexOf("Area:");
		ParseElem(line.Substring(i1 + 5, i2 - (i1 + 5)).Trim(), ability);
		ParseArea(line.Substring(i2 + 5).Trim(), ability);
	}

	static void ParseEffect (string line, Ability ability)
	{
		line = line.Replace("|", "");
		line = line.Substring(line.IndexOf(":") + 1).Trim();
		if (line.StartsWith("---"))
			return;

		Effect effect = new Effect();
		if (line.StartsWith("AType"))
		{
			effect.rate = Effect.HitRates.AType;
			line = line.Replace("AType,", "").Trim();
		}
		else if (line.StartsWith("100%H"))
		{
			effect.rate = Effect.HitRates.Full;
			line = line.Replace("100%H,", "").Trim();
		}
		else if (line.StartsWith("SType"))
		{
			effect.rate = Effect.HitRates.SType;
			line = line.Replace("SType,", "").Trim();
		}
		else
		{
			Debug.LogError("Unrecognized hit type: " + line);
			return;
		}

		int next = line.IndexOf(",");
		if (next == -1)
			goto FINISH;

		string target = line.Substring(0, next).Replace(" ", "");
		Effect.Targets t = (Effect.Targets)Enum.Parse(typeof(Effect.Targets), target);
		if (!Enum.IsDefined(typeof(Effect.Targets), t))
		{
			Debug.LogError("Unrecognized target type: " + line);
			return;
		}
		effect.target = t;

		line = line.Substring(next + 1).Trim();
		if (line.Contains(":"))
		{
			string dep = line.Substring(0, line.IndexOf(":"));
			dep = dep.Replace("-", "");
			Effect.Dependencies d = (Effect.Dependencies)Enum.Parse(typeof(Effect.Dependencies), dep);
			if (!Enum.IsDefined(typeof(Effect.Dependencies), d))
			{
				Debug.LogError("Unrecognized dependency type: " + d);
				return;
			}
			effect.dependency = d;
			line = line.Substring(line.IndexOf(":") + 1);
		}

	FINISH:
		effect.result = line;
		ability.effects.Add(effect);
	}

	static void ParseNotes1 (string line, Ability ability)
	{
		if (line.Contains("Offensive"))
			ability.notes.Add(Ability.Notes.Offensive);
		if (line.Contains("Reflectable"))
			ability.notes.Add(Ability.Notes.Reflectable);
		if (line.Contains("Ignore-Reaction"))
			ability.notes.Add(Ability.Notes.ByPass);
		if (line.Contains("Silencable"))
			ability.notes.Add(Ability.Notes.Silenceable);
	}

	static void ParseNotes2 (string line, Ability ability)
	{
		if (line.Contains("Stealable"))
			ability.notes.Add(Ability.Notes.Stealable);
		if (line.Contains("R-Ab:Return Magic"))
			ability.notes.Add(Ability.Notes.Counter);
		if (line.Contains("R-Ab:Absorb MP"))
			ability.notes.Add(Ability.Notes.Absorb);
	}

	static void AddCategory (string category, string ability)
	{
		if (!categories.ContainsKey(category))
			categories.Add(category, new List<string>());
		categories[category].Add(ability);
	}

	static void ParsePower (string line, Ability ability)
	{
		if (line.Contains("(MPow)"))
		{
			line = line.Replace("(MPow)", "").Trim();
			ability.powerType = Ability.Powers.MPow;
			Int32.TryParse(line, out ability.power);
		}
		else if (line.Contains("(WAtk)"))
		{
			line = line.Replace("(WAtk)", "").Trim();
			ability.powerType = Ability.Powers.WAtk;
			Int32.TryParse(line, out ability.power);
		}
		else if (line.Contains("---"))
		{
			ability.powerType = Ability.Powers.None;
			ability.power = 0;
		}
		else if (line.Contains("As Weapon"))
		{
			ability.powerType = Ability.Powers.Weapon;
			ability.power = 0;
		}
		else
		{
			Debug.LogError("New Power Type: " + line);
		}
	}

	static void ParseMP (string line, Ability ability)
	{
		if (line.Contains("---")) ability.mpCost = 0;
		else if (Int32.TryParse(line, out ability.mpCost)) {}
		else Debug.LogError("New MP Cost Type: " + line);
	}

	static void ParseRange (string line, Ability ability)
	{
		if (line.Contains("As Weapon"))
			ability.rangeType = Ability.Ranges.Weapon;
		else if (line.Contains("Self"))
			ability.rangeType = Ability.Ranges.Self;
		else if (line.Contains("Dbl-Line"))
			ability.rangeType = Ability.Ranges.DblLine;
		else if (line.Contains("Line"))
			ability.rangeType = Ability.Ranges.Line;
		else if (line.Contains("Cone"))
			ability.rangeType = Ability.Ranges.Cone;
		else if (line.Contains("Infinite"))
			ability.rangeType = Ability.Ranges.Infinite;
		else
		{
			ability.rangeType = Ability.Ranges.Constant;
			if (!Int32.TryParse(line, out ability.range))
				Debug.LogError("New Range Type: " + line);
		}
	}

	static void ParseElem (string line, Ability ability)
	{
		if (line.Contains("---"))
			ability.charge = Ability.Charges.None;
		else if (line.Contains("As Weapon"))
			ability.charge = Ability.Charges.Weapon;
		else
		{
			ability.charge = Ability.Charges.Elemental;
			try 
			{
				Elements e = (Elements)Enum.Parse(typeof(Elements), line);
				if (Enum.IsDefined(typeof(Elements), e))
					ability.element = e;
				else
					Debug.LogError("Unrecognized value: " + line);
			}
			catch (ArgumentException)
			{
				Debug.LogError("Unrecognized value: " + line);
			}
		}
	}

	static void ParseArea (string line, Ability ability)
	{
		if (line.Contains("(Not Self)"))
		{
			line = line.Replace("(Not Self)", "").Trim();
			ability.excludeSelf = true;
		}
		if (line.Contains("/"))
		{
			ability.areaType = Ability.Areas.Specify;
			string[] elements = line.Split('/');
			if (!Int32.TryParse(elements[0].Replace("r", ""), out ability.horArea))
				Debug.LogError("Cant Parse area: " + line);
			if (!Int32.TryParse(elements[1].Replace("v", ""), out ability.verArea))
				Debug.LogError("Cant Parse area: " + line);
		}
		else if (line.Contains("One Unit"))
			ability.areaType = Ability.Areas.OneUnit;
		else if (line.Contains("Infinite"))
			ability.areaType = Ability.Areas.Infinite;
		else if (line.Contains("Self"))
			ability.areaType = Ability.Areas.Self;
		else
		{
			Debug.Log("Unrecognized type: " + line);
		}
	}
}

