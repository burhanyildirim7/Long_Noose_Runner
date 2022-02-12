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
    private bool left, right, isEnableForSwipe;
    [SerializeField] private float skateSpeed = 5.0f;
    [SerializeField] private float swipeControlLimit;  
    [SerializeField] private float playerSwipeSpeed;
    [SerializeField] private float horizontalRadius = 3;
    private float screenWidth, screenHeight;
    private float lastMousePosX, firstMousePosX, lastMousePosY, firstMousePosY;
    private float colliderHeight, colliderPosY;
    public List<GameObject> circleRopes = new List<GameObject>();
    public List<GameObject> xIslands = new List<GameObject>();
    public GameObject parentRopeR, parentRopeL, bagRopeR, bagRopeL,atlamaRopu,sallananRopeL,sallananRopeR,artiBir,eksiBir,model;
    private int ropeCount = 0;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start()
    {
        colliderHeight = GetComponent<CapsuleCollider>().height;
        colliderPosY = GetComponent<CapsuleCollider>().center.y;
        DOTween.Init();
        StartingEvents();
        screenWidth = Screen.width / 2;
        screenWidth = Screen.height / 2;
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
				return;

			}
			else if (myTouch.deltaPosition.x < -swipeControlLimit)
			{
				isEnableForSwipe = false;
				right = false;
				left = true;
				MoveHorizontal();
				return;
			}
			else if (myTouch.deltaPosition.y < -swipeControlLimit)
			{
                StartCoroutine(DelayAndNormalizedCollider());
                //isEnableForSwipe = false;
				SlideEvents();
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
		if (Input.GetMouseButton(0) && isEnableForSwipe)
		{
            lastMousePosX = Input.mousePosition.x - screenWidth;
            lastMousePosY = Input.mousePosition.y - screenHeight;
            if((lastMousePosX - firstMousePosX) > swipeControlLimit) // sağa
			{
                isEnableForSwipe = false;
                right = true;
                left = false;
                MoveHorizontal();
                return;
            }
            else if((lastMousePosX - firstMousePosX) < -swipeControlLimit) // sola
			{
                isEnableForSwipe = false;
                right = false;
                left = true;
                MoveHorizontal();
                return;
            }
            else if((lastMousePosY - firstMousePosY) < -swipeControlLimit) // aşşa
			{
                StartCoroutine(DelayAndNormalizedCollider());
                //isEnableForSwipe = false;
                SlideEvents();            
                return;
            }
        }

		if (Input.GetMouseButtonUp(0))
		{
            lastMousePosX = firstMousePosX = lastMousePosY = firstMousePosY = 0;
		}

		#endregion
	}

    IEnumerator DelayAndNormalizedCollider()
	{
        GetComponent<CapsuleCollider>().height = 1.5f;
        GetComponent<CapsuleCollider>().center = new Vector3(0, .8f, 0);
        yield return new WaitForSeconds(.8f);
        GetComponent<CapsuleCollider>().height = colliderHeight;
        GetComponent<CapsuleCollider>().center = new Vector3(0,colliderPosY,0);
	}

	private void MoveHorizontal()
	{
        if (right)
        {
            if (transform.position.x > horizontalRadius - 1) return;
            else if (transform.position.x > -1) 
            {
                JumpAnim();
                transform.DOMoveX(horizontalRadius, playerSwipeSpeed).OnComplete(() =>
                {
                    isEnableForSwipe = true;
                    return;
                });
            }                        
            else if (transform.position.x  < -1)
			{
                JumpAnim();
                transform.DOMoveX(0, playerSwipeSpeed).OnComplete(() =>
                {
                    isEnableForSwipe = true;
                    return;
                });
            }          
        }
        else if (left)
        {
            if (transform.position.x < -horizontalRadius + 1) return;
            else if (transform.position.x < 1)
			{
                JumpAnim();
                transform.DOMoveX(-horizontalRadius, playerSwipeSpeed).OnComplete(() =>
                {
                    isEnableForSwipe = true;
                    return;
                });
            }
                
            else if (transform.position.x > 1)
			{
                JumpAnim();
                transform.DOMoveX(0, playerSwipeSpeed).OnComplete(() =>
                {
                    isEnableForSwipe = true;
                    return;
                });
            }
                
        }
    }

    private void SlideEvents()
	{
        // collider küçülecek... kayma animasyonu yapılacak... 
        SlideAnim();
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
            Destroy(other.gameObject);
            ropeCount++;
            if(ropeCount > 0)
			{
                sallananRopeL.SetActive(true);
                sallananRopeR.SetActive(true);
                bagRopeL.SetActive(true);
                bagRopeR.SetActive(true);
            }
            AddRope();
            GameController.instance.SetScore(collectibleDegeri); // ORNEK KULLANIM detaylar icin ctrl+click yapip fonksiyon aciklamasini oku
            GameObject arti = Instantiate(artiBir, new Vector3(model.transform.position.x,model.transform.position.y + 3,model.transform.position.z), Quaternion.identity, model.transform);
            TextAnim(arti);
        }
        else if (other.CompareTag("engel"))
        {
            // ENGELELRE CARPINCA YAPILACAKLAR....

            GameObject eksi = Instantiate(eksiBir, new Vector3(model.transform.position.x, model.transform.position.y + 3, model.transform.position.z), Quaternion.identity, model.transform);
            TextAnim(eksi);
            GameController.instance.SetScore(-collectibleDegeri); // ORNEK KULLANIM detaylar icin ctrl+click yapip fonksiyon aciklamasini oku
            ropeCount--;
            if (ropeCount == 0)
            {
                sallananRopeL.SetActive(false);
                sallananRopeR.SetActive(false);
                bagRopeL.SetActive(false);
                bagRopeR.SetActive(false);
            }
            AddRope();
            if (GameController.instance.score < 0) // SKOR SIFIRIN ALTINA DUSTUYSE
			{
                GameController.instance.isContinue = false; // çarptığı anda oyuncunun yerinde durması ilerlememesi için
                UIController.instance.ActivateLooseScreen();
                IdleAnim();
			}
			else
			{
                CrashAnim();
			}   

        }
        else if (other.CompareTag("finish")) 
        {
            IdleAnim();
            GameController.instance.isContinue = false;
           // GetComponent<Collider>().enabled = false;
           if(ropeCount == 0)
			{
                UIController.instance.ActivateLooseScreen();              
                return;
            }
			else
			{
                FinalFly();
            }
                    
        }else if (other.CompareTag("finalx"))
		{
            FinalEffectEvents(other.gameObject);
        }

    }


    void AddRope()
	{
        if(ropeCount < 0)
		{
            GameController.instance.isContinue = false; // çarptığı anda oyuncunun yerinde durması ilerlememesi için
            UIController.instance.ActivateLooseScreen();
            IdleAnim();
        }
        else if(ropeCount < 8)
		{
            if(ropeCount !=0)circleRopes[ropeCount-1].SetActive(true);
            if(ropeCount< 7)circleRopes[ropeCount].SetActive(false);
            if (ropeCount !=0) circleRopes[ropeCount + 6].SetActive(true);
            if (ropeCount < 7) circleRopes[ropeCount + 7].SetActive(false);
            if(ropeCount == 1)
			{
                parentRopeL.transform.localScale = Vector3.one*.016f;
                parentRopeR.transform.localScale = Vector3.one*.016f;
                bagRopeL.transform.localScale = Vector3.one*1.1f;
                bagRopeR.transform.localScale = Vector3.one*1.1f;
                
			}else if(ropeCount == 3)
			{
                parentRopeL.transform.localScale = Vector3.one * .017f;
                parentRopeR.transform.localScale = Vector3.one * .017f;
                bagRopeL.transform.localScale = Vector3.one * 1.2f;
                bagRopeR.transform.localScale = Vector3.one * 1.2f;
            }
            else if (ropeCount == 4)
            {
                parentRopeL.transform.localScale = Vector3.one * .018f;
                parentRopeR.transform.localScale = Vector3.one * .018f;
                bagRopeL.transform.localScale = Vector3.one * 1.3f;
                bagRopeR.transform.localScale = Vector3.one * 1.3f;
            }
            else if (ropeCount == 5)
            {
                parentRopeL.transform.localScale = Vector3.one * .0185f;
                parentRopeR.transform.localScale = Vector3.one * .0185f;
                bagRopeL.transform.localScale = new Vector3(1.4f,1.4f,1.6f);
                bagRopeR.transform.localScale = new Vector3(1.4f, 1.4f, 1.6f);
            }
            else if (ropeCount == 6)
            {
                parentRopeL.transform.localScale = Vector3.one * .019f;
                parentRopeR.transform.localScale = Vector3.one * .019f;
                bagRopeL.transform.localScale = new Vector3(1.4f, 1.4f, 2f);
                bagRopeR.transform.localScale = new Vector3(1.4f, 1.4f, 2f);
            }

        }
        
        
	}

    /// <summary>
    /// Bu fonksiyon her level baslarken cagrilir. 
    /// </summary>
    public void StartingEvents()
    {
        foreach(var rope in circleRopes)
		{
            rope.SetActive(false);
           
		}
        ropeCount = 0;
        parentRopeL.transform.localScale = Vector3.one*.015f;
        parentRopeR.transform.localScale = Vector3.one*.015f;
        bagRopeL.transform.localScale = Vector3.one;
        bagRopeR.transform.localScale = Vector3.one;
        sallananRopeL.SetActive(false);
        sallananRopeR.SetActive(false);
        bagRopeL.SetActive(false);
        bagRopeR.SetActive(false);
        transform.parent.transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.parent.transform.position = Vector3.zero;
        GameController.instance.isContinue = false;
        GameController.instance.score = 0;
        transform.position = new Vector3(0, transform.position.y, 0);
        GetComponent<Collider>().enabled = true;
        isEnableForSwipe = true;
    }

    public void PreStartingEvents()
	{
        RunAnim();
        GameController.instance.isContinue = true;
    }

    private void FinalFly()
	{
        int index = 0;
        if(ropeCount >= 2 && ropeCount <= 4)
		{
            // 2x olabilir
		}
        else if(ropeCount > 4 && ropeCount <= 6)
        {
            // 2x olabilir
            index += 1;

        }
        else if (ropeCount > 6 && ropeCount <= 8)
        {
            // 2x olabilir
            index += 2;

        }
        else if (ropeCount > 6 && ropeCount <= 8)
        {
            // 2x olabilir
            index += 3;
        }
        else if (ropeCount > 8 && ropeCount <= 10)
        {
            // 2x olabilir
            index += 4;
        }
        else if (ropeCount > 10 && ropeCount <= 12)
        {
            // 2x olabilir
            index += 5;
		}
		else
		{
            index += 6;
		}
        GetComponent<Collider>().enabled = false;
        atlamaRopu.gameObject.SetActive(true);
        atlamaRopu.transform.DOScale(new Vector3(1,1,1),.2f);
        transform.DOMove(new Vector3(0, transform.position.y + index*2f, transform.position.z + index * 2f), .5f).SetEase(Ease.InCirc).
                OnComplete(() => { StartCoroutine(FinalFlyMovement(xIslands[index],index*1.5f)); });

        GameController.instance.ScoreCarp(index+1);
    }

    private IEnumerator FinalFlyMovement(GameObject xObject, float height)
	{
        float aci; ;
        float y;
        float z;
        float tempZ = transform.position.z;
        float tempY = transform.position.y;
        float dist = 0;
        float addDistance = 0.009f / ((height / 1.5f) + 1);
        while (dist < 1)
        {
            dist+=addDistance;
            aci = Mathf.Lerp(0,180,dist);
            z = Mathf.Lerp(tempZ,xObject.transform.position.z,dist);
            y = -height * Mathf.Sin(Mathf.Deg2Rad * aci) + tempY;

            //transform.position = Vector3.Lerp(transform.position,xObject.transform.position+ new Vector3(0,y,0),dist);
            transform.position = new Vector3(0, y, z);
            yield return new WaitForSeconds(.005f);
        }


        //      while (transform.position.z < xObject.transform.position.z)
        //{
        //          if(aci < 180)
        //	{
        //              aci += .9f;
        //              y = -height* Mathf.Sin(Mathf.Deg2Rad * aci) + tempY;
        //          }

        //          transform.position = new Vector3(0,y,transform.position.z +.1f);
        //          yield return new WaitForSeconds(.004f);
        //}
        atlamaRopu.SetActive(false);
        transform.DOMove(xObject.transform.position + new Vector3(0,2.8f,0),.6f).SetEase(Ease.OutFlash)
            .OnComplete(()=> { GetComponent<Collider>().enabled = true; });
	}

   void FinalEffectEvents(GameObject island)
	{
        island.transform.GetChild(0).gameObject.SetActive(true);
        UIController.instance.ActivateWinScreen();
	}

	#region ANIMATIONS .....
    private void TextAnim(GameObject obj)
	{
        obj.transform.DOScale(Vector3.one * .2f, .5f);
        obj.transform.DOMoveY(5, .5f).OnComplete(()=> { Destroy(obj); });
    }
	private void RunAnim()
	{
        GetComponentInChildren<Animator>().SetTrigger("run");
        StartCoroutine(DelayAndResetAnims());
    }

    private void IdleAnim()
	{
        GetComponentInChildren<Animator>().SetTrigger("idle");
        StartCoroutine(DelayAndResetAnims());
    }

    private void JumpAnim()
	{
        GetComponentInChildren<Animator>().SetTrigger("jump");
        StartCoroutine(DelayAndResetAnims());
    }

    private void SlideAnim()
	{
        GetComponentInChildren<Animator>().SetTrigger("slide");
        StartCoroutine(DelayAndResetAnims());
    }

    private void CrashAnim()
	{
        GetComponentInChildren<Animator>().SetTrigger("crash");
        StartCoroutine(DelayAndResetAnims());
    }

    private IEnumerator DelayAndResetAnims()
	{
        yield return new WaitForSeconds(.1f);
        GetComponentInChildren<Animator>().ResetTrigger("idle");
        GetComponentInChildren<Animator>().ResetTrigger("jump");
        GetComponentInChildren<Animator>().ResetTrigger("run");
        GetComponentInChildren<Animator>().ResetTrigger("slide");
        GetComponentInChildren<Animator>().ResetTrigger("crash");
    }

	#endregion

}
