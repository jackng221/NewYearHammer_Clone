using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 pushVector;

    [SerializeField] MMF_Player mmfPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger");
        pushVector = (collision.transform.position - transform.position).normalized * collision.GetComponentInParent<Rigidbody2D>().velocity;
        rb.AddForce(pushVector, ForceMode2D.Impulse);
        mmfPlayer.GetFeedbackOfType<MMF_CinemachineImpulse>().Velocity = pushVector;
        mmfPlayer.PlayFeedbacks();
    }
    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3)pushVector);
    }
}
