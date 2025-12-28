using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRoterion : MonoBehaviour
{
    Vector3 before  = Vector3.zero;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (before != Vector3.zero)
        {
            Vector3 dir = transform.position - before;

            if (dir.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        } 

        before = transform.position;
    }

    private void OnDisable()
    {
        before = Vector3.zero;
    }

    private void OnEnable()
    {
        before = Vector3.zero;
    }
}
