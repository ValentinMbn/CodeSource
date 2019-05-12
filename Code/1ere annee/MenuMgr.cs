using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuMgr : MonoBehaviour
{
	#region Variables

	List<Menu> menus = new List<Menu>();
	static public MenuMgr instance = null;

	#endregion

	#region Properties

	static public MenuMgr Instance
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<MenuMgr>();
			return instance;
		}
	}

	public List<Menu> Menus
	{
		get { return menus; }
	}

	#endregion

	#region Methods

	public Menu GetMenu(EMenu ID)
	{
		foreach (var menu in menus)
		{
			if (menu.GetID().Equals(ID))
				return menu;
		}

		return null;
	}

	public void InteractableButton(bool interactable)
	{
		foreach (Menu menu in menus)
		{
			foreach(Button button in menu.ListButtons)
				button.interactable = interactable;
		}
	}

	public void ShowUnshow(EMenu ID, bool display)
	{
		GetMenu(ID).ShowUnshow(display);
	}

	public void AddMenu(Menu menu)
	{
		menus.Add(menu);
	}

	public void RemoveMenu(Menu menu)
	{
		menus.Remove(menu);
	}

	#endregion
}