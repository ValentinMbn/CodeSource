using UnityEngine;
using UnityEngine.UI;
using System;

public class GameMenu : Menu
{
	#region Variables

	[SerializeField]
	PrefabsGameMenuList prefabsList;

	#endregion

	#region Properties

	public Text TimerText
	{
		get { return prefabsList.timerText; }
	}


	#endregion

	#region Init

	protected override void Start()
	{
		base.Start();
		DisplayBestTime();
	}

	#endregion

	#region Display

	public void DisplayBestTime()
	{
		if(GameMgr.Instance.InGameSave.IfBestTime())
			prefabsList.bestTimeText.text = String.Format("Best Time :  {0:00} : {1:00}", GameMgr.Instance.LevelScore.min, GameMgr.Instance.LevelScore.sec);
		else
			prefabsList.bestTimeText.text = "Best Time : --:--"; 
	}

	#endregion

	#region LoadMethods

	public string LoadImage()
	{
		string path = "Collected";

		if (LevelMgr.Instance.Score.bLetter)
			path += "B";
		if (LevelMgr.Instance.Score.yLetter)
			path += "Y";
		if (LevelMgr.Instance.Score.rLetter)
			path += "R";
		if(!LevelMgr.Instance.Score.bLetter && !LevelMgr.Instance.Score.yLetter && !LevelMgr.Instance.Score.rLetter)
			path += "None";

		return "Sprites\\" + path;
	}

	#endregion

	#region Methods

	public void ChangeImage(ColorLetter color)
	{
		if(color == ColorLetter.Blue)
			LevelMgr.Instance.Score.bLetter = true;
		else if(color == ColorLetter.Yellow)
			LevelMgr.Instance.Score.yLetter = true;
		else if(color == ColorLetter.Red)
			LevelMgr.Instance.Score.rLetter = true;

		prefabsList.letterImage.sprite = Resources.Load<Sprite>(LoadImage());
	}

	#endregion
}