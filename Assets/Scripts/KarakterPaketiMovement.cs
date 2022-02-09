using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarakterPaketiMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    void update()
    {
        if (GameController.instance.isContinue == true)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        }      
    }

}
