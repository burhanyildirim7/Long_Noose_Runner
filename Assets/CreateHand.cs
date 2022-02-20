using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class CreateHand : MonoBehaviour
{
    public Image hand,handPrefab;

    public void Create()
	{
		Image img = Instantiate(handPrefab,transform);
		img.transform.localPosition = hand.transform.localPosition;		
		img.transform.DOScale(Vector3.one * .1f, .4f).OnComplete(() => { Destroy(img.gameObject); });
	}
}
