using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatPanel : MonoBehaviour
{
	public Panel panel;
	public Sprite allyBackground;
	public Sprite enemyBackground;
	public Image background;
	public Image avatar;
	public Text nameLabel;
	public Text hpLabel;
	//public Text apLabel;
	public Text mpLabel;
	public Text lvLabel;

	public void Display(GameObject obj)
	{
		// TEMP Need a component for this
		background.sprite = UnityEngine.Random.value > 0.5f ? enemyBackground : allyBackground;
		// avatar.sprite = null; Need a component for this
		nameLabel.text = obj.name;
		Stats stats = obj.GetComponent<Stats>();
		if (stats)
		{
			hpLabel.text = string.Format("HP {0} / {1}", stats[StatTypes.HP], stats[StatTypes.MHP]);
			//apLabel.text = string.Format("MP {0} / {1}", stats[StatTypes.AP], stats[StatTypes.MAP]);
			mpLabel.text = string.Format("MP {0} / {1}", stats[StatTypes.MP], stats[StatTypes.MMP]);
			lvLabel.text = string.Format("LV. {0}", stats[StatTypes.LVL]);
		}
	}
}
