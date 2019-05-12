using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public enum EMenu
{
	Pause,
	GameOverAndWin,
	InGame,
	Settings,
	LevelInfo,
	Victory,
	Confirm,
	Main,
	Career,
	Load,
	Credits
}

public abstract class Menu : MonoBehaviour
{
	#region Variables

	[SerializeField]
	private EMenu ID;
	[SerializeField]
	bool interactive;
	[SerializeField]
	protected GameObject firstSelected = null;
	[SerializeField]
	List<Button> listButtons;

	protected GameObject currentButton;
	protected Menu prevMenu = null;
	protected EventSystem eventSystem;
	bool set = true;

	#endregion

	#region Properties

	public List<Button> ListButtons
	{
		get { return listButtons; }
	}

	public EMenu GetID()
	{
		return ID;
	}

	#endregion

	#region Init

	protected virtual void Start()
	{
		eventSystem = EventSystem.current;

		if(MenuMgr.Instance.Menus.Count > 0)
			MenuMgr.Instance.InteractableButton(false);

		MenuMgr.Instance.AddMenu(this);

		if (interactive)
		{
			GameMgr.Instance.CurrentState = GameState.Menu;
			GameMgr.Instance.ControllerMgr.setKeyEvent(ControllerMgr.ControllerKey.A, true, Submit);
			GameMgr.Instance.ControllerMgr.setKeyEvent(ControllerMgr.ControllerKey.B, true, Cancel);
		}
	}

	#endregion

	#region Update

	protected virtual void Update()
	{
		if (currentButton != null)
		{
			if (set)
			{
				set = false;
				eventSystem.SetSelectedGameObject(null);
			}

			if (eventSystem.currentSelectedGameObject != currentButton && currentButton != null)
			{
				if (eventSystem.currentSelectedGameObject == null)
					eventSystem.SetSelectedGameObject(currentButton);
				else
				{
					AudioMgr.Instance.PlaySound("Menu/MovingInMenu");
					currentButton = eventSystem.currentSelectedGameObject;
				}
			}
		}
	}

	#endregion

	#region Methods

	public int IndexCurrentButton()
	{
		int i = 0;

		foreach(Button button in listButtons)
		{
			if (button.gameObject == currentButton)
				return i;
			i++;
		}
		return -1;
	}

	public void ShowUnshow(bool display)
	{
		gameObject.SetActive(display);
	}

	protected void OnEnable()
	{
		if (interactive && firstSelected != null)
		{
			set = true;
			currentButton = firstSelected;
		}
	}

	public void SetPrevMenu(Menu prev)
	{
		prevMenu = prev;
	}

	public void Display(bool display, GameState state)
	{
		ShowUnshow(display);
		GameMgr.Instance.CurrentState = state;

		if (display)
			Time.timeScale = 0f;
		else
			Time.timeScale = 1f;
	}

	protected virtual void Cancel()
	{
		AudioMgr.Instance.PlaySound("Menu/Back");
	}

	protected virtual void Submit()
	{
		AudioMgr.Instance.PlaySound("Menu/Validation");
	}

	public virtual void Subscribe()
	{
		GameMgr.Instance.ControllerMgr.setKeyEvent(ControllerMgr.ControllerKey.A, true, Submit);
		GameMgr.Instance.ControllerMgr.setKeyEvent(ControllerMgr.ControllerKey.B, true, Cancel);
	}

	public virtual void Unsubscribe()
	{
		GameMgr.Instance.ControllerMgr.RemoveKeyEvent(ControllerMgr.ControllerKey.A, true, Submit);
		GameMgr.Instance.ControllerMgr.RemoveKeyEvent(ControllerMgr.ControllerKey.B, true, Cancel);
	}

	protected virtual void OnDestroy()
	{
		if(GameMgr.Instance != null)
		{
			GameMgr.Instance.ControllerMgr.RemoveKeyEvent(ControllerMgr.ControllerKey.A, true, Submit);
			GameMgr.Instance.ControllerMgr.RemoveKeyEvent(ControllerMgr.ControllerKey.B, true, Cancel);
		}

		if (MenuMgr.Instance != null)
		{
			MenuMgr.Instance.RemoveMenu(this);

			if (MenuMgr.Instance.Menus.Count > 0)
				MenuMgr.Instance.InteractableButton(true);
		}

		if (prevMenu != null)
			prevMenu.Subscribe();
	}

	#endregion
}