using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public static UIController instance; // Singleton yapisi icin gerekli ornek

	public GameObject TapToStartPanel, LoosePanel, GamePanel, WinPanel,winScreenEffectObject, winScreenCoinImage,startScreenCoinImage,scoreEffect;
	public Text gamePlayScoreText, winScreenScoreText, levelNoText, tapToStartScoreText, totalElmasText;
	public Animator ScoreTextAnim;



	// singleton yapisi burada kuruluyor.
	private void Awake()
	{
		if (instance == null) instance = this;
		//else Destroy(this);
	}

	private void Start()
	{
		StartUI();
	}

	// Oyun ilk acildiginda calisacak olan ui fonksiyonu. 
	public void StartUI()
	{
		ActivateTapToStartScreen();
	}

	/// <summary>
	/// Level numarasini ui kisminda degistirmek icin fonksiyon. Parametre olarak level numarasi aliyor.
	/// </summary>
	/// <param name="levelNo">UI ekranina yazilmak istenen Level numarasý</param>
	public void SetLevelText(int levelNo)
	{
		levelNoText.text = "Level " + levelNo.ToString();
	}

	// TAPTOSTART TUSUNA BASILDISINDA  --- GIRIS EKRANINDA VE LEVEL BASLARINDA
	public void TapToStartButtonClick()
	{

		GameController.instance.isContinue = true;
		PlayerController.instance.StartingEvents();
		PlayerController.instance.PreStartingEvents();
		TapToStartPanel.SetActive(false);
		GamePanel.SetActive(true);
		SetLevelText(LevelController.instance.totalLevelNo);
		SetGamePlayScoreText();

	}

	// RESTART TUSUNA BASILDISINDA  --- LOOSE EKRANINDA
	public void RestartButtonClick()
	{
		GamePanel.SetActive(false);
		LoosePanel.SetActive(false);
		TapToStartPanel.SetActive(true);
		LevelController.instance.RestartLevelEvents();
		SetTapToStartScoreText();
	}


	// NEXT LEVEL TUSUNA BASILDIGINDA... WIN EKRANINDAKI BUTON
	public void NextLevelButtonClick()
	{
		SetTapToStartScoreText();
		TapToStartPanel.SetActive(true);
		WinPanel.SetActive(false);
		GamePanel.SetActive(false);
		LevelController.instance.NextLevelEvents();
		StartCoroutine(StartScreenCoinEffect());
	}


	/// <summary>
	/// Bu fonksiyon gameplay ekranindaki score textini gunceller.
	/// </summary>
	public void SetGamePlayScoreText()
	{
		gamePlayScoreText.text = PlayerPrefs.GetInt("totalScore").ToString();
	}


	/// <summary>
	/// Bu fonksiyon totalScore un yazilmasi gereken textleri gunceller.
	/// </summary>
	public void SetTapToStartScoreText()
	{
		tapToStartScoreText.text = PlayerPrefs.GetInt("totalScore").ToString();
	}

	/// <summary>
	/// Bu fonksiyon winscreen de geçerli level scoreunun yazildigi texti gunceller.
	/// </summary>
	public void WinScreenScore()
	{
		winScreenScoreText.text = GameController.instance.score.ToString();	
	}

	/// <summary>
	/// Bu fonksiyon totalElmas sayilarinin yazildigi textleri gunceller.
	/// </summary>
	public void SetTotalElmasText()
	{
		totalElmasText.text = PlayerPrefs.GetInt("totalElmas").ToString() ;
	}

	/// <summary>
	/// Bu fonksiyon winscreen ekranini acar.
	/// </summary>
	public void ActivateWinScreen()
	{
		GamePanel.SetActive(false);
		StartCoroutine(WinScreenDelay());
	}

	IEnumerator WinScreenDelay()
	{
		yield return new WaitForSeconds(1.5f);
		WinPanel.SetActive(true);
		winScreenScoreText.text = "0";
		int sayac = 0;
		while (sayac < GameController.instance.score)
		{
			sayac+=10;
			if(sayac %20 == 0)
			{
				GameObject effectObj = Instantiate(winScreenEffectObject, new Vector3(144, 400, 0), Quaternion.identity,winScreenCoinImage.transform);
				effectObj.transform.localPosition = new Vector3(144,300,0);
				effectObj.transform.localRotation = Quaternion.Euler(0,0, winScreenCoinImage.transform.localRotation.z);
				effectObj.GetComponent<Image>().sprite = winScreenCoinImage.GetComponent<Image>().sprite;
				effectObj.transform.localScale = Vector3.one * .2f;
				StartCoroutine(WinScreenEffect(effectObj));
			}
			winScreenScoreText.text = sayac.ToString();
			yield return new WaitForSeconds(.05f);
		}
	}

	IEnumerator WinScreenEffect(GameObject effectObj)
	{
		float sayac = 0;
		float scale = 0;
		while (Vector2.Distance(effectObj.transform.position, winScreenCoinImage.transform.position) > 0.05f)
		{
			effectObj.transform.position = Vector2.Lerp(effectObj.transform.position, winScreenCoinImage.transform.position, sayac);
			scale = Mathf.Lerp(effectObj.transform.localScale.x,winScreenCoinImage.transform.localScale.x,sayac);
			effectObj.transform.localScale = Vector3.one * scale;
			sayac += .02f;
			yield return new WaitForSeconds(.015f);
		}
		Destroy(effectObj);
	}

	IEnumerator StartScreenCoinEffect()
	{
		startScreenCoinImage.GetComponent<Image>().sprite = winScreenCoinImage.GetComponent<Image>().sprite;
		startScreenCoinImage.SetActive(true);
		float sayac = 0;
		int adet = 0;
		while (Vector2.Distance(startScreenCoinImage.transform.position,tapToStartScoreText.transform.position) >= 5f)
		{
			adet++;
			sayac += .01f;
			startScreenCoinImage.transform.position = Vector2.Lerp(startScreenCoinImage.transform.position, tapToStartScoreText.transform.position, sayac);
			yield return new WaitForSeconds(.025f);
			if(adet %3 == 0)
			{
				Debug.Log("çalýþtýýýý");
				GameObject coin = Instantiate(winScreenEffectObject, startScreenCoinImage.transform.position, Quaternion.identity,TapToStartPanel.transform);
				coin.GetComponent<Image>().sprite = winScreenCoinImage.GetComponent<Image>().sprite;
				coin.transform.rotation = startScreenCoinImage.transform.rotation;
				StartCoroutine(StartScreenCoinsDissolve(coin));
			}			
		}
		Instantiate(scoreEffect, new Vector3(1.34f,5.43F,-1.15F), Quaternion.identity);
		ScoreTextAnim.SetTrigger("score");
		startScreenCoinImage.SetActive(false);
		startScreenCoinImage.transform.localPosition = new Vector3(0,-446,0);
	}

	IEnumerator StartScreenCoinsDissolve(GameObject obj)
	{
		Color tempColor = obj.GetComponent<Image>().color;
		while (obj.transform.localScale.x > .2f)
		{
			obj.transform.localScale = new Vector3(obj.transform.localScale.x - .05f, obj.transform.localScale.y - .05f, obj.transform.localScale.z - .05f);
			tempColor.a = tempColor.a - .05f;
			obj.GetComponent<Image>().color = tempColor;
			yield return new WaitForSeconds(.03f);
		}
		Destroy(obj);
	}

	/// <summary>
	/// Bu fonksiyon loose secreeni acar. 
	/// </summary>
	public void ActivateLooseScreen()
	{
		GamePanel.SetActive(false);
		LoosePanel.SetActive(true);
	}


	/// <summary>
	/// Bu fonksiyon gamescreeni acar.
	/// </summary>
	public void ActivateGameScreen()
	{
		GamePanel.SetActive(true);
		TapToStartPanel.SetActive(false);
		SetGamePlayScoreText();
	}

	/// <summary>
	/// Bu fonksiyon taptostartscreen i acar.
	/// </summary>
	public void ActivateTapToStartScreen()
	{
		TapToStartPanel.SetActive(true);
		WinPanel.SetActive(false);
		LoosePanel.SetActive(false);
		GamePanel.SetActive(false);
		tapToStartScoreText.text = PlayerPrefs.GetInt("total").ToString();
	}

	

}
