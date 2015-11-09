using UnityEngine;
using System;
using System.Collections.Generic;

public class Equipment : MonoBehaviour
{
	#region Notifications
	public const string EquippedNotification = "Equipment.EquippedNotification";
	public const string UnEquippedNotification = "Equipment.UnEquippedNotification";
	#endregion

	#region Fields / Properties
	public IList<Equippable> items { get { return _items.AsReadOnly(); } }
	List<Equippable> _items = new List<Equippable>();
	#endregion

	#region Public
	public void Equip(Equippable item, EquipSlots slots)
	{
		UnEquip(slots);

		_items.Add(item);
		item.transform.SetParent(transform);
		item.slots = slots;
		item.OnEquip();

		this.PostNotification(EquippedNotification, item);
	}

	public void UnEquip(Equippable item)
	{
		item.OnUnEquip();
		item.slots = EquipSlots.None;
		item.transform.SetParent(transform);
		_items.Remove(item);

		this.PostNotification(UnEquippedNotification, item);
	}

	public void UnEquip(EquipSlots slots)
	{
		for (int i = _items.Count - 1; i >= 0; --i)
		{
			Equippable item = _items[i];
			if ((item.slots & slots) != EquipSlots.None)
				UnEquip(item);
		}
	}

	public Equippable GetItem(EquipSlots slots)
	{
		for (int i = _items.Count - 1; i >= 0; --i)
		{
			Equippable item = _items[i];
			if ((item.slots & slots) != EquipSlots.None)
				return item;
		}
		return null;
	}
	#endregion
}
