using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElephantSDK;

public class LevelController : MonoBehaviour
{
	public static LevelController instance;
	public int levelNo, tempLevelNo, totalLevelNo; // totallevelno tum leveller bitip random level gelmeye baslayinca kullaniliyor
	public List<GameObject> levels = new List<GameObject>();
	private GameObject currentLevelObj;

	private void Awake()
	{
		if (instance == null) instance = this;
		//else Destroy(this);
	}

	private void Start()
	{
		PlayerPrefs.DeleteAll();
		totalLevelNo = PlayerPrefs.GetInt("level");
		if (totalLevelNo == 0)
		{
			totalLevelNo = 1;
			levelNo = 1;
		}
		UIController.instance.SetLevelText(totalLevelNo);
		LevelStartingEvents();
	}


	/// <summary>
	/// Bu fonksiyon level nuarasini bir artirir.
	/// </summary>
	public void IncreaseLevelNo()
	{
		tempLevelNo = levelNo;
		totalLevelNo++;
		PlayerPrefs.SetInt("level", totalLevelNo);
		UIController.instance.SetLevelText(totalLevelNo);
	}

	/// <summary>
	/// Bu fonksiyon oyun ilk acildiginda veya nextlevelde tetiklenir.
	/// </summary>
	public void LevelStartingEvents()
	{
		if (totalLevelNo > levels.Count)
		{
			levelNo = Random.Range(1, levels.Count + 1);
			if (levelNo == tempLevelNo) levelNo = Random.Range(1, levels.Count + 1);
		}
		else
		{
			levelNo = totalLevelNo;
		}
		UIController.instance.SetLevelText(totalLevelNo);
		currentLevelObj = Instantiate(levels[levelNo - 1], Vector3.zero, Quaternion.identity);
		Elephant.LevelStarted(totalLevelNo);

	}

	/// <summary>
	/// Bu fonksiyon nextlevel butonuna basildiginda tetiklenir. UIControlden tetikleniyor.
	/// </summary>
	public void NextLevelEvents()
	{
		Elephant.LevelCompleted(totalLevelNo);
		Destroy(currentLevelObj);
		IncreaseLevelNo();
		LevelStartingEvents();
		PlayerController.instance.StartingEvents();		
	}

	/// <summary>
	/// Bu fonksiyon RestartLevel butonuna basildiginda tetiklenir. UIControlden tetikleniyor.
	/// </summary>
	public void RestartLevelEvents()
	{
		Elephant.LevelFailed(totalLevelNo);
		Destroy(currentLevelObj);
		LevelStartingEvents();
		PlayerController.instance.StartingEvents();

	}
}
