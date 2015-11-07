using UnityEngine;
using System.Collections;

public class PerformAbilityState : BattleState 
{
	public override void Enter ()
	{
		base.Enter ();
		HasUnitActed = true;
		if (HasUnitMoved)
			LockMove = true;
		StartCoroutine(Animate());
	}

	IEnumerator Animate ()
	{
		yield return null;
		// TODO play animations, etc
		yield return null;

		for (int i = 0; i < targets.Count; ++i)
		{
			int damage = ability.CalculateDamage(ability.effects[0], current, targets[i]);
			int hp = targets[i].GetStat(StatTypes.HP);
			int maxHP = targets[i].GetStat(StatTypes.MHP);
			int result = Mathf.Clamp(hp - damage, 0, maxHP);
			targets[i].SetStat(StatTypes.HP, result);
			
			Vector3 dmgTextLocation = targets[i].transform.position;
			dmgTextLocation.y += 1;

			GameObject dmgText = new GameObject();

			TextMesh mesh = dmgText.AddComponent<TextMesh>();
			mesh.fontSize = 100;
			mesh.color = Color.red;
			mesh.text = damage.ToString();

			ConstantForce force = dmgText.AddComponent<ConstantForce>();
			force.force = new Vector3(0f, 13f, 0f);

			dmgText.transform.localScale  = new Vector3(0.1f, 0.1f, 1f);
			dmgText.transform.rotation = Quaternion.LookRotation(dmgTextLocation - Camera.main.transform.position);
			dmgText.transform.position = dmgTextLocation;
			GameObject.Destroy(dmgText, 1f);
		}

		int resultingMP = owner.current.MP - ability.mpCost;
		owner.current.SetStat(StatTypes.MP, resultingMP);

		int resultingAP = owner.current.AP - ability.apCost;
		owner.current.SetStat(StatTypes.AP, resultingAP);

		if (HasUnitMoved && owner.current.HP > 0)
		{ owner.ChangeState<EndFacingState>(); }
		else if (owner.current.HP > 0)
		{ owner.ChangeState<CommandSelectionState>(); }
		else
		{ owner.CompletedTurn(); }
			
	}
}
