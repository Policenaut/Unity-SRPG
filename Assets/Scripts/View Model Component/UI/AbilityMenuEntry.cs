using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityMenuEntry : MonoBehaviour 
{
	#region Properties
	public bool Selected
	{
		get
		{
			return _selected;
		}
		set
		{
			_selected = value;
			StateChanged();
		}
	}
	bool _selected;

	public bool Locked
	{
		get
		{
			return _locked;
		}
		set
		{
			_locked = value;
			StateChanged();
		}
	}
	bool _locked;

	public string Title
	{
		get
		{
			return label.text;
		}
		set
		{
			label.text = value;
		}
	}

	[SerializeField] Image bullet;
	[SerializeField] Sprite normalSprite;
	[SerializeField] Sprite selectedSprite;
	[SerializeField] Sprite disabledSprite;
	[SerializeField] Text label;
	Outline outline;
	#endregion

	#region MonoBehaviour
	void Awake ()
	{
		outline = label.GetComponent<Outline>();
	}
	#endregion

	#region Private
	void StateChanged ()
	{
		if (Locked)
		{
			bullet.sprite = disabledSprite;
			label.color = Color.gray;
			outline.effectColor = new Color32(20, 36, 44, 255);
		}
		else if (Selected)
		{
			bullet.sprite = selectedSprite;
			label.color = new Color32(249, 210, 118, 255);
			outline.effectColor = new Color32(255, 160, 72, 255);
		}
		else
		{
			bullet.sprite = normalSprite;
			label.color = Color.white;
			outline.effectColor = new Color32(20, 36, 44, 255);
		}
	}
	#endregion
}