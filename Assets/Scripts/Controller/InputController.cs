using UnityEngine;
using System;
using System.Collections;

public class InputController : MonoBehaviour 
{
	#region Events
	public static event EventHandler<InfoEventArgs<int>> fireEvent;
	public static event EventHandler<InfoEventArgs<Point>> moveEvent;
	#endregion

	#region Properties
	const float _repeatThreshold = 0.5f;
	const float _repeatRate = 0.25f;
	const float _tapRate = 0.1f;
	float _horNext, _verNext;
	bool _horHold, _verHold;
	string[] _buttons = new string[] {"Fire1", "Fire2", "Fire3"};
	#endregion

	#region MonoBehaviour
	void Update () 
	{
		float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");

		int x = 0, y = 0;

		if (!Mathf.Approximately(h, 0))
		{
			if (Time.time > _horNext)
			{
				x = (h < 0f) ? -1 : 1;
				_horNext = Time.time + (_horHold ? _repeatRate : _repeatThreshold);
				_horHold = true;
			}
		}
		else
		{
			_horHold = false;
			_horNext = 0;
		}

		if (!Mathf.Approximately(v, 0))
		{
			if (Time.time > _verNext)
			{
				y = (v < 0f) ? -1 : 1;
				_verNext = Time.time + (_verHold ? _repeatRate : _repeatThreshold);
				_verHold = true;
			}
		}
		else
		{
			_verHold = false;
			_verNext = 0;
		}

		if (x != 0 || y != 0)
			Move (new Point(x, y));

		for (int i = 0; i < 3; ++i)
		{
			if (Input.GetButtonUp(_buttons[i]))
				Fire (i);
		}
	}
	#endregion

	#region Private
	void Fire (int i)
	{
		if (fireEvent != null)
			fireEvent(this, new InfoEventArgs<int>(i));
	}

	void Move (Point p)
	{
		if (moveEvent != null)
			moveEvent(this, new InfoEventArgs<Point>(p));
	}
	#endregion
}