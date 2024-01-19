using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArrow : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject child;
    [SerializeField] float distance;

    private void Update()
    {
        target = GameObject.Find("[Creature] Sunfish");
        
        if (target == null)
            child.SetActive(false);
        
        RotateTowardsTarget();
        ArrowToggle();
    }

    public void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x -transform.position.x ) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 50 * Time.deltaTime);
    }

    public void ArrowToggle()
    {
        if (Vector2.Distance(gameObject.transform.position, target.transform.position) < distance)
        {
            child.SetActive(false);
        }
        else if (Vector2.Distance(gameObject.transform.position, target.transform.position) > distance)
        {
            child.SetActive(true);
        }
    }
}