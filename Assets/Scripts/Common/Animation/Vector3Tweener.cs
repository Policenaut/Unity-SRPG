using UnityEngine;
using System.Collections;

public abstract class Vector3Tweener : Tweener
{
	public Vector3 startValue;
	public Vector3 endValue;
	public Vector3 currentValue { get; private set; }
	
	protected override void OnUpdate (object sender, System.EventArgs e)
	{
		currentValue = (endValue - startValue) * easingControl.currentValue + startValue;
	}
}
