using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarakterPaketiMovement : MonoBehaviour
{
    public static KarakterPaketiMovement instance;
    public float _speed;

	private void Awake()
	{
        if (instance == null) instance = this;
        else Destroy(this);
	}

	void Update()
    {
        if (GameController.instance.isContinue == true)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        }      
    }

}
