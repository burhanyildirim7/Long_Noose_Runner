using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public int collectibleDegeri;
    public bool xVarMi = true;
    public bool collectibleVarMi = true;
    Rigidbody rb;
    private bool left, right, isEnableForSwipe;
    public float skateSpeed = 5.0f;
    public float swipeControlLimit = 50f;  
    public float horizontalRadius = 3;


    private Vector3 leftPos, rightPos, centerPos;



    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start()
    {
        StartingEvents();
        rb = GetComponent<Rigidbody>();
        leftPos = new Vector3(-horizontalRadius,transform.position.y,transform.position.z);
        rightPos = new Vector3(horizontalRadius,transform.position.y,transform.position.z);
        centerPos = new Vector3(0, transform.position.y, transform.position.z);
    }

	private void Update()
	{
		if(Input.touchCount > 0 && isEnableForSwipe)
		{
            isEnableForSwipe = false;
            Touch myTouch = Input.GetTouch(0);
            if(myTouch.deltaPosition.x > swipeControlLimit) 
			{
                right = true;
                left = false;
			}
            else if (myTouch.deltaPosition.x < -swipeControlLimit) 
            {
                right = false;
                left = true;
            }

			
        }
	}

	private void Move()
	{
        if (right)
        {
            if (transform.position.x > horizontalRadius - 1) return;
            else if (transform.position.x > 1)
                transform.position = Vector3.Lerp(transform.position, rightPos, skateSpeed * Time.deltaTime);
            // else if()
        }
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
