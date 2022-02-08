using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveMovement : MonoBehaviour
{
    private SwerveInputSystem _swerveInputSystem;

    [SerializeField] private float swerveSpeed = 0.5f;

    [SerializeField] private GameObject _getPoint;

    [SerializeField] private float _radius;

    Vector3 centerPosition;

    private void Awake()
    {
        _swerveInputSystem = GetComponent<SwerveInputSystem>();
    }

    private void Update()
    {
        if (GameController.instance.isContinue == true)
        {
            centerPosition = _getPoint.transform.position;


            float swerveAmount = Time.deltaTime * swerveSpeed * _swerveInputSystem.MoveFactorX;

            transform.Translate(swerveAmount, 0, 0);


            float distance = Vector3.Distance(transform.position, centerPosition);

            if (distance > _radius)
            {
                Vector3 fromOriginToObject = transform.position - centerPosition;
                fromOriginToObject *= _radius / distance;
                transform.position = centerPosition + fromOriginToObject;
            }
        }
       
    }
}
