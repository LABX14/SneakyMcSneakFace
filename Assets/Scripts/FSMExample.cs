﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMExample : MonoBehaviour
{
    public string AIState = "Idle";
    public float aiSenseRadius;
    public Vector3 targetPosition;
    public float health;
    public float healthCutoff;
    public float moveSpeed;
    public float healingRate;
    public float maxHealth;
    public float hearingDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AIState == "Idle")
        {
            // Do the state behavior
            Idle();
            //Check for transitions
            if (Vector3.Distance(transform.position, targetPosition) < aiSenseRadius)
            {
                ChangeState("Seek");
            }
        }
        else if (AIState == "Seek")
        {
            //Do the state behavior
            Seek();
            //Check for transitions
            if (health < healthCutoff)
            {
                ChangeState("Rest");
            }
            else if (Vector3.Distance(transform.position,targetPosition) >= aiSenseRadius)
            {
                ChangeState("Idle");
            }
        }
        else if (AIState == "Rest")
        {
            // DO the state behavior
            Rest();
            // Check for transitions
            if (health >= healthCutoff)
            {
                ChangeState("Idle");
            }
        }
        else
        {
            Debug.LogWarning("AIState not found: " + AIState);
        }
    }

    void Idle()
    {
        //Do nothing
    }

    void Seek()
    {
        Vector3 vectorToTarget = targetPosition - transform.position;
        transform.position += vectorToTarget.normalized * moveSpeed * Time.deltaTime;
    }
    void Rest()
    {
        // TODOL Write the rest state
        health += healingRate * Time.deltaTime;
        health = Mathf.Min(health, maxHealth);
    }

    public void ChangeState(string newState)
    {
        AIState = newState;
    }

    private bool CanHear(GameObject target)
    {
        // Get target noise maker
        NoiseMaker targetNoiseMaker = target.GetComponent<NoiseMaker>();
        // if they don't have a noise maker, we can't hear them.
        if(targetNoiseMaker == null) { return false; }
        // if the distance between us and the target is less than the sum of the noise distance, we can hear it
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if ((targetNoiseMaker.volumeDistance + hearingDistance) > distanceToTarget) { return true; }

        return false;
    }
}
