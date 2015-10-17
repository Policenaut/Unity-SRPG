using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConversationPanel : MonoBehaviour
{
	#region Events
	public event EventHandler willAppearEvent;
	public event EventHandler didAppearEvent;
	public event EventHandler willDisappearEvent;
	public event EventHandler didDisappearEvent;
	#endregion

	#region Properties
	[SerializeField] Image _avatar;
	[SerializeField] Text _label;
	[SerializeField] GameObject _readMoreArrow;
	Queue<string> _pages;
	RectTransform _rt;
	Vector3 _offscreen, _onScreen;
	bool _isOnScreen;
	#endregion

	#region MonoBehaviour
	void Awake ()
	{
		_rt = transform as RectTransform;
		_offscreen = new Vector3(_rt.rect.width, 0, 0);
		_onScreen = new Vector3(0, 0, 0);
	}
	#endregion

	#region Event Handlers
	public void OnSubmit ()
	{
		if (!_isOnScreen)
			return;

		if (_pages.Count > 0)
			NextPage();
		else
			Hide ();
	}
	#endregion

	#region Public
	public void Display (Queue<string> pages)
	{
		_pages = pages;
		NextPage();
		Show();
	}
	#endregion
	
	#region Private
	void NextPage ()
	{
		_label.text = (_pages != null && _pages.Count > 0) ? _pages.Dequeue() : string.Empty;
		_readMoreArrow.SetActive(_pages.Count > 0);
	}

	void Show ()
	{
		if (willAppearEvent != null)
			willAppearEvent(this, EventArgs.Empty);
		Tweener tweener = _rt.MoveToLocal(_onScreen, 0.5f, EasingEquations.EaseOutQuad);
		tweener.easingControl.completedEvent += delegate(object sender, System.EventArgs e) {
			_isOnScreen = true;
			EventSystem.current.SetSelectedGameObject(this.gameObject);
			if (didAppearEvent != null)
				didAppearEvent(this, EventArgs.Empty);
				};
	}

	void Hide ()
	{
		if (willDisappearEvent != null)
			willDisappearEvent(this, EventArgs.Empty);
		_isOnScreen = false;
		Tweener tweener = _rt.MoveToLocal(_offscreen, 0.5f, EasingEquations.EaseOutQuad);
		tweener.easingControl.completedEvent += delegate(object sender, System.EventArgs e) {
			if (didDisappearEvent != null)
				didAppearEvent(this, EventArgs.Empty);
				};
	}
	#endregion
}