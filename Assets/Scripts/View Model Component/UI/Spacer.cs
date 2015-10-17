using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

[RequireComponent(typeof(LayoutElement))]
[RequireComponent(typeof(EasingControl))]
public class Spacer : MonoBehaviour 
{
	public Point min;
	public Point max;
	LayoutElement _layoutElement;
	EasingControl _easingControl;

	void Awake ()
	{
		_layoutElement = GetComponent<LayoutElement>();
		_easingControl = GetComponent<EasingControl>();
		_easingControl.loopType = EasingControl.LoopType.PingPong;
	}

	void OnEnable ()
	{
		_easingControl.updateEvent += OnEasingUpdate;
	}

	void OnDisable ()
	{
		_easingControl.updateEvent -= OnEasingUpdate;
	}

	public void Toggle (bool isOpen)
	{
		if (isOpen)
			_easingControl.Play();
		else
			_easingControl.Reverse();
	}

	void OnEasingUpdate (object sender, EventArgs e)
	{
		_layoutElement.minWidth = Mathf.Lerp(min.x, max.x, _easingControl.currentValue);
		_layoutElement.minHeight = Mathf.Lerp(min.y, max.y, _easingControl.currentValue);
	}
}