using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDirection : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public Transform Direction; //child on the Vector2.right side

    Quaternion rotation;
    float angle;

    private void Update()
    {
        //Vector2.Angle(SignedAngle) compares Vector2 !directions! not positions
        angle = Vector2.SignedAngle(Vector2.right, pointB.position - pointA.position);
        rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;

        //Draw line to visualize direction of this.gameObject
        Debug.DrawLine(transform.position, Direction.position);
    }
}
