using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttackPanel : SidePanel 
{
	[SerializeField] GameObject blueBacker;
	[SerializeField] GameObject redBacker;
	[SerializeField] Text nameLabel;
	[SerializeField] Text hpLabel;
	[SerializeField] Text mpLabel;
	[SerializeField] Text lvLabel;

	public void ShowStats (Unit unit, bool isAlly)
	{
		if (unit != null)
		{
			blueBacker.SetActive(isAlly);
			redBacker.SetActive(!isAlly);
			nameLabel.text = unit.name;
			hpLabel.text = string.Format("HP {0}/{1}", unit.HP, unit.MaxHP);
			mpLabel.text = string.Format("MP {0}/{1}", unit.MP, unit.MaxMP);
			lvLabel.text = string.Format("LV. {0}", unit.Lvl);
		}
		Toggle(unit != null);
	}
}