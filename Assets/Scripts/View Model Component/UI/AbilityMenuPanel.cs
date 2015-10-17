using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class AbilityMenuPanel : SidePanel 
{
	#region Events
	public event EventHandler<InfoEventArgs<int>> selectEvent;
	public event EventHandler cancelEvent;
	#endregion

	#region Properties
	[SerializeField] GameObject entryPrefab;
	[SerializeField] Text titleLabel;
	List<AbilityMenuEntry> currentEntries = new List<AbilityMenuEntry>();
	Queue<AbilityMenuEntry> reusableEntries = new Queue<AbilityMenuEntry>();
	int selection;
	#endregion

	#region Event Handlers
	protected override void OnMove (object sender, InfoEventArgs<Point> e)
	{
		if (visible == false)
			return;

		if (e.info.x > 0 || e.info.y < 0)
			Next ();
		else
			Previous ();
	}
	
	protected override void OnFire (object sender, InfoEventArgs<int> e)
	{
		if (visible == false)
			return;

		switch (e.info)
		{
		case 0:
			if (selectEvent != null)
				selectEvent(this, new InfoEventArgs<int>(selection));
			break;
		case 1:
			if (cancelEvent != null)
				cancelEvent(this, EventArgs.Empty);
			break;
		}
	}
	#endregion

	#region Public
	public void Load (string title, params string[] options)
	{
		Clear();
		titleLabel.text = title;
		for (int i = 0; i < options.Length; ++i)
		{
			AbilityMenuEntry entry = DequeueEntry();
			entry.Title = options[i];
			currentEntries.Add(entry);
		}
		SetSelection(0);
	}

	public void SetLocked (int index, bool value)
	{
		if (index < 0 || index >= currentEntries.Count)
			return;
		currentEntries[index].Locked = value;
		if (value && selection == index)
			Next();
	}
	#endregion

	#region Private
	void Clear ()
	{
		for (int i = currentEntries.Count - 1; i >= 0; --i)
			EnqueueEntry(currentEntries[i]);
	}

	AbilityMenuEntry DequeueEntry ()
	{
		if (reusableEntries.Count > 0)
		{
			AbilityMenuEntry entry = reusableEntries.Dequeue();
			entry.gameObject.SetActive(true);
			entry.transform.SetParent(transform, false);
			return entry;
		}

		GameObject instance = Instantiate(entryPrefab) as GameObject;
		instance.transform.SetParent(transform, false);
		return instance.GetComponent<AbilityMenuEntry>();
	}

	void EnqueueEntry (AbilityMenuEntry entry)
	{
		currentEntries.Remove(entry);
		reusableEntries.Enqueue(entry);
		entry.Selected = false;
		entry.Locked = false;
		entry.transform.SetParent(null, false);
		entry.gameObject.SetActive(false);
	}

	void Next ()
	{
		for (int i = selection + 1; i < selection + currentEntries.Count; ++i)
		{
			int index = i % currentEntries.Count;
			if (SetSelection(index))
				break;
		}
	}

	void Previous ()
	{
		for (int i = selection - 1 + currentEntries.Count; i > selection; --i)
		{
			int index = i % currentEntries.Count;
			if (SetSelection(index))
				break;
		}
	}

	bool SetSelection (int value)
	{
		if (currentEntries[value].Locked)
			return false;

		// Deselect the previously selected entry
		if (selection >= 0 && selection < currentEntries.Count)
			currentEntries[selection].Selected = false;

		selection = value;
		
		// Select the new entry
		if (selection >= 0 && selection < currentEntries.Count)
			currentEntries[selection].Selected = true;

		return true;
	}
	#endregion
}
