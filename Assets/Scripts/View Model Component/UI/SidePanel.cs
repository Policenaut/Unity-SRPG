using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// A UI Panel which slides on and off the screen along the side
/// of the window.
/// </summary>
public class SidePanel : MonoBehaviour 
{
	#region Properties
	public bool visible { get; protected set; }
	protected EasingControl easingControl;
	protected RectTransform rt;
	#endregion

	#region MonoBehaviour
	protected virtual void Awake ()
	{
		easingControl = gameObject.AddComponent<EasingControl>();
		easingControl.duration = 0.5f;
		easingControl.loopType = EasingControl.LoopType.PingPong;
		easingControl.equation = EasingEquations.EaseOutExpo;
		rt = transform as RectTransform;
	}

	protected virtual void OnEnable ()
	{
		easingControl.updateEvent += OnEasingUpdate;
	}
	
	protected virtual void OnDisable ()
	{
		easingControl.updateEvent -= OnEasingUpdate;
	}
	#endregion

	#region Event Handlers
	protected virtual void OnEasingUpdate (object sender, EventArgs e)
	{
		if (!easingControl.IsPlaying)
			return;
		
		Vector3 pos = transform.localPosition;
		pos.x = (1f - easingControl.currentValue) * rt.rect.width;
		if (Mathf.Approximately(rt.pivot.x, 0))
			pos.x *= -1f;
		rt.localPosition = pos;
	}

	protected virtual void OnMove (object sender, InfoEventArgs<Point> e)
	{

	}
	
	protected virtual void OnFire (object sender, InfoEventArgs<int> e)
	{

	}
	#endregion

	#region Public
	public void Toggle (bool visible)
	{
		if (this.visible == visible)
			return;

		if (visible)
		{
			easingControl.Play();
			InputController.moveEvent += OnMove;
			InputController.fireEvent += OnFire;
		}
		else
		{
			easingControl.Reverse();
			InputController.moveEvent -= OnMove;
			InputController.fireEvent -= OnFire;
		}

		this.visible = visible;
	}
	#endregion
}
