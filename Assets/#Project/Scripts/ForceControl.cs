using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceControl : MonoBehaviour
{
    Rigidbody2D body;
    [SerializeField] bool onLeftSide = false;
    public float addTorque = 150f;
    public float angularVel;
    public float insufficientVelocityThreshold = 10f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        angularVel = body.angularVelocity;

        //if angularVelocity lower than threshold (+/-), add torque (clockwise if tilted to the right, vice versa)
        if (body.angularVelocity <= insufficientVelocityThreshold && body.angularVelocity >= insufficientVelocityThreshold * -1)
        {
            if (Vector2.Angle(Vector2.left, transform.up) <= 90f)  //On left side
            {
                onLeftSide = true;
            }
            else if (Vector2.Angle(Vector2.right, transform.up) <= 90f)
            {
                onLeftSide = false;
            }

            if (onLeftSide)
            {
                body.AddTorque(addTorque);
                Debug.Log("torque " + addTorque);
            }
            else
            {
                body.AddTorque(addTorque * -1);
                Debug.Log("torque -" + addTorque);
            }
        }
    }
}
