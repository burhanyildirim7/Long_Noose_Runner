using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public int collectibleDegeri;
    public bool xVarMi = true;
    public bool collectibleVarMi = true;
    Rigidbody rb;
    private bool left, right, isEnableForSwipe;
    [SerializeField] private float skateSpeed = 5.0f;
    [SerializeField] private float swipeControlLimit;  
    [SerializeField] private float playerSwipeSpeed;
    [SerializeField] private float horizontalRadius = 3;

    private float screenWidth, screenHeight;
    private Vector3 leftPos, rightPos, centerPos;
    private float lastMousePosX, firstMousePosX, lastMousePosY, firstMousePosY;



    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start()
    {
        DOTween.Init();
        StartingEvents();
        screenWidth = Screen.width / 2;
        screenWidth = Screen.height / 2;
        Debug.Log(screenWidth);
        //rb = GetComponent<Rigidbody>();
        //leftPos = new Vector3(-horizontalRadius,transform.position.y,transform.position.z);
        //rightPos = new Vector3(horizontalRadius,transform.position.y,transform.position.z);
        //centerPos = new Vector3(0, transform.position.y, transform.position.z);
    }

	private void Update()
	{
		#region For Mobile Control ... SBI
		if (Input.touchCount > 0 && isEnableForSwipe)
		{
			Touch myTouch = Input.GetTouch(0);
			if (myTouch.deltaPosition.x > swipeControlLimit)
			{
				isEnableForSwipe = false;
				right = true;
				left = false;
				MoveHorizontal();
				Debug.Log("sağa sürükledin beni...");
				return;

			}
			else if (myTouch.deltaPosition.x < -swipeControlLimit)
			{
				isEnableForSwipe = false;
				right = false;
				left = true;
				MoveHorizontal();
				Debug.Log("sola sürükledin beni...");
				return;
			}
			else if (myTouch.deltaPosition.y < -swipeControlLimit)
			{
				isEnableForSwipe = false;
				SkateEvents();
				Debug.Log("aşağı sürükledin beni...");
				return;
			}
		}
		#endregion



		#region For Stand Control ... SBI

		if (Input.GetMouseButtonDown(0))
		{
            firstMousePosX = Input.mousePosition.x - screenWidth;
            firstMousePosY = Input.mousePosition.y - screenHeight;
        }
		if (Input.GetMouseButton(0))
		{
            lastMousePosX = Input.mousePosition.x - screenWidth;
            lastMousePosY = Input.mousePosition.y - screenHeight;
            Debug.Log(lastMousePosX - firstMousePosX);
            if((lastMousePosX - firstMousePosX) > swipeControlLimit) // sağa
			{
                isEnableForSwipe = false;
                right = true;
                left = false;
                MoveHorizontal();
                Debug.Log("sağa sürükledin beni...");
                return;
            }
            else if((lastMousePosX - firstMousePosX) < -swipeControlLimit) // sola
			{
                isEnableForSwipe = false;
                right = false;
                left = true;
                MoveHorizontal();
                Debug.Log("sola sürükledin beni...");
                return;
            }
            else if((lastMousePosY - firstMousePosY) < -swipeControlLimit) // aşşa
			{
                isEnableForSwipe = false;
                SkateEvents();
                Debug.Log("aşağı sürükledin beni...");
                return;
            }
        }

		if (Input.GetMouseButtonUp(0))
		{
            lastMousePosX = firstMousePosX = lastMousePosY = firstMousePosY = 0;
		}

		#endregion
	}

	private void MoveHorizontal()
	{
        if (right)
        {
            if (transform.position.x > horizontalRadius - 1) return;
            else if (transform.position.x > -1)
                transform.DOMoveX(horizontalRadius, playerSwipeSpeed).OnComplete(()=> 
                {
                    isEnableForSwipe = true;
                    return;
                });
            // transform.position = Vector3.Lerp(transform.position, rightPos, skateSpeed * Time.deltaTime);
            else if (transform.position.x  < -1)
                transform.DOMoveX(0, playerSwipeSpeed).OnComplete(() =>
                {
                    isEnableForSwipe = true;
                    return;
                });
        }
        else if (left)
        {
            if (transform.position.x < -horizontalRadius + 1) return;
            else if (transform.position.x < 1)
                transform.DOMoveX(-horizontalRadius, playerSwipeSpeed).OnComplete(() =>
                {
                    isEnableForSwipe = true;
                    return;
                });
            else if (transform.position.x > 1)
                transform.DOMoveX(0, playerSwipeSpeed).OnComplete(() =>
                {
                    isEnableForSwipe = true;
                    return;
                });
        }
    }

    private void SkateEvents()
	{
        // collider küçülecek... kayma animasyonu yapılacak... 
	}

	/// <summary>
	/// Playerin collider olaylari.. collectible, engel veya finish noktasi icin. Burasi artirilabilir.
	/// elmas icin veya baska herhangi etkilesimler icin tag ekleyerek kontrol dongusune eklenir.
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("collectible"))
        {
            // COLLECTIBLE CARPINCA YAPILACAKLAR...
            GameController.instance.SetScore(collectibleDegeri); // ORNEK KULLANIM detaylar icin ctrl+click yapip fonksiyon aciklamasini oku

        }
        else if (other.CompareTag("engel"))
        {
            // ENGELELRE CARPINCA YAPILACAKLAR....
            GameController.instance.SetScore(-collectibleDegeri); // ORNEK KULLANIM detaylar icin ctrl+click yapip fonksiyon aciklamasini oku
            if (GameController.instance.score < 0) // SKOR SIFIRIN ALTINA DUSTUYSE
			{
                // FAİL EVENTLERİ BURAYA YAZILACAK..
                GameController.instance.isContinue = false; // çarptığı anda oyuncunun yerinde durması ilerlememesi için
                UIController.instance.ActivateLooseScreen(); // Bu fonksiyon direk çağrılada bilir veya herhangi bir effect veya animasyon bitiminde de çağrılabilir..
                // oyuncu fail durumunda bu fonksiyon çağrılacak.. 
			}


        }
        else if (other.CompareTag("finish")) 
        {
            // finishe collider eklenecek levellerde...
            // FINISH NOKTASINA GELINCE YAPILACAKLAR... Totalscore artırma, x işlemleri, efektler v.s. v.s.
            GameController.instance.isContinue = false;
            GameController.instance.ScoreCarp(3);  // Bu fonksiyon normalde x ler hesaplandıktan sonra çağrılacak. Parametre olarak x i alıyor. 
            // x değerine göre oyuncunun total scoreunu hesaplıyor.. x li olmayan oyunlarda parametre olarak 1 gönderilecek.
            UIController.instance.ActivateWinScreen(); // finish noktasına gelebildiyse her türlü win screen aktif edilecek.. ama burada değil..
            // normal de bu kodu x ler hesaplandıktan sonra çağıracağız. Ve bu kod çağrıldığında da kazanılan puanlar animasyonlu şekilde artacak..

            
        }

    }


    /// <summary>
    /// Bu fonksiyon her level baslarken cagrilir. 
    /// </summary>
    public void StartingEvents()
    {

        transform.parent.transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.parent.transform.position = Vector3.zero;
        GameController.instance.isContinue = false;
        GameController.instance.score = 0;
        transform.position = new Vector3(0, transform.position.y, 0);
        GetComponent<Collider>().enabled = true;

    }

}
