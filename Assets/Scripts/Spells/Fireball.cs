﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour, IPooledObject
{

    [SerializeField] float velocity;
    [SerializeField] float secondsDuration;
    [SerializeField] float damage;
    private float angle;
    private float currentDuration;
    private string[] listOfObstacleTags = { Tags.ENEMY, Tags.SOLID_OBSTACLE };

    // Update is called once per frame
    void Update()
    {
        transform.Translate(VectorFromAngle(angle) * velocity);
        currentDuration += Time.fixedDeltaTime;
        if (currentDuration > secondsDuration)
        {
            gameObject.SetActive(false);
        }
    }

    Vector2 VectorFromAngle(float theta)
    {
        return new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)); 
    }

    public void OnObjectSpawn()
    {
        Vector3 dir = transform.rotation.eulerAngles;
        angle = Utilities.getAngleDegBetween(dir.y, dir.x) + 90;
        currentDuration = 0;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject hitTarget = col.gameObject;
        if (hitTarget.tag == Tags.ENEMY)
        {
            hitTarget.GetComponent<EnemyController>().decrementHealth(damage);
        }

        deactivateObjectIfNeeded(hitTarget.tag); 
    }

    void deactivateObjectIfNeeded(string tag)
    {
        for(int i = 0; i < listOfObstacleTags.Length; i++)
        {
            if (tag == listOfObstacleTags[i])
            {
                gameObject.SetActive(false);
            }
        }
    }
}
