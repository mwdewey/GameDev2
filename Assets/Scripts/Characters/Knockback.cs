using UnityEngine;
using System.Collections;

public class Knockback {

    public float timeRemaining;
    public Vector2 force;
    public bool isActing;

    public Knockback(float time, Vector2 forceToApply)
    {
        timeRemaining = time;
        force = forceToApply;
        isActing = true;
    }

    private void FixedUpdate()
    {
        if (isActing)
        {
            timeRemaining -= Time.fixedDeltaTime;
            if (timeRemaining < 0)
            {
                timeRemaining = 0;
                isActing = false;

            }
        }

    }

}
