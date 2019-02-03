﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStorm : ISpell
{
    [SerializeField] float slowCoolDown = 2.5f;
    [SerializeField] float idleTimeToLive = 0.5f;
    [SerializeField] float slowDownPercentage = 0.25f;

    private void Awake()
    {
        slowDownPercentage = Mathf.Max(0.0001f, slowDownPercentage);
    }

    protected override void Update()
    {
        base.Update();
        if (!isMoving)
        {
            currentTimeToLive += Time.fixedDeltaTime;
            if (currentTimeToLive > idleTimeToLive)
            {
                gameObject.SetActive(false);
            }
        }
    }

    protected override void onMovementTimeToLiveStopped()
    {
        currentTimeToLive = 0;
        isMoving = false;
    }


    public void OnObjectSpawn()
    {
        Vector3 dir = transform.rotation.eulerAngles;
        angle = Utilities.getAngleDegBetween(dir.y, dir.x) + 90;
        currentTimeToLive = 0;
        isMoving = true;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
        damageEnemyIfNeeded(col.gameObject);
        slowEnemyIfNeeded(col.gameObject);
    }

    void slowEnemyIfNeeded(GameObject gObject)
    {
        if (gObject.tag == Tags.ENEMY)
        {
            gObject.GetComponent<EnemyController>().modifySpeed(0.00001f, slowCoolDown);
        }
    }

    void damageEnemyIfNeeded(GameObject colGameObj)
    {
        if (colGameObj.tag == Tags.ENEMY)
        {
            colGameObj.GetComponent<ICharacter>().decrementHealth(damage);
        }
    }

    protected override void onSpellHitObject()
    {
        onMovementTimeToLiveStopped();
    }
}