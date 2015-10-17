using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// If all heroes have died, the enemy party wins
/// Else If all enemies have died, the hero party wins
/// Else no party has won
/// </summary>
public class AnnihilationCondition : VictoryCondition 
{
	BattleController bc { get { return BattleController.instance; }}

	public override Alliances Check ()
	{
		if (AllUnitsDead(bc.heroes))
			return Alliances.Monster;
		if (AllUnitsDead(bc.enemies))
			return Alliances.Hero;
		return Alliances.Neutral;
	}

	bool AllUnitsDead (List<Unit> list)
	{
		for (int i = 0; i < list.Count; ++i)
			if (list[i].GetComponent<Death>() == null)
				return false;
		return true;
	}
}
