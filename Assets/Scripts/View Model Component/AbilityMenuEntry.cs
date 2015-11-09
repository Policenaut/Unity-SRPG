using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityMenuEntry : MonoBehaviour
{
	#region Enums
	[System.Flags]
	enum States
	{
		None = 0,
		Selected = 1 << 0,
		Locked = 1 << 1
	}
	#endregion

	#region Properties
	public string Title
	{
		get { return label.text; }
		set { label.text = value; }
	}
	
	public bool IsLocked
	{
		get { return (State & States.Locked) != States.None; }
		set
		{
			if (value)
			{ State |= States.Locked; }
			else
			{ State &= ~States.Locked; }
		}
	}

	public bool IsSelected
	{
		get { return (State & States.Selected) != States.None; }
		set
		{
			if (value)
			{ State |= States.Selected;  }
			else
			{ State &= ~States.Selected; }
		}
	}

	States State
	{
		get { return state; }
		set
		{
			if (state == value)
			{ return; }
			state = value;

			if (IsLocked)
			{
				bullet.sprite = disabledSprite;
				label.color = Color.gray;
				outline.effectColor = new Color32(20, 36, 44, 255);
			}
			else if (IsSelected)
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
	}
	States state;

	[SerializeField] Image bullet;
	[SerializeField] Sprite normalSprite;
	[SerializeField] Sprite selectedSprite;
	[SerializeField] Sprite disabledSprite;
	[SerializeField] Text label;
	Outline outline;
	#endregion

	#region MonoBehaviour
	void Awake()
	{
		outline = label.GetComponent<Outline>();
	}
	#endregion

	#region Public
	public void Reset()
	{
		State = States.None;
	}
	#endregion
}