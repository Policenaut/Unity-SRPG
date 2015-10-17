using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HitSuccessGauge : MonoBehaviour 
{
	[SerializeField] Image filler;
	[SerializeField] Text label;

	public void SetHitRate (int rate, int damage)
	{
		filler.fillAmount = (float)rate / 100f;
		label.text = string.Format("{0}% {1}pt(s)", rate, damage);
	}
}
