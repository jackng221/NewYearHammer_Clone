using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjBell : MonoBehaviour
{
    Rigidbody2D rb;

    Vector2 hitPos;
    public Vector2 HitPos { get { return hitPos; } }
    Vector2 hitDirection;
    public Vector2 HitDirection { get {  return hitDirection; } }

    public float cooldown = 0.4f;
    [SerializeField] float timer = 0;
    float subtractTimeSinceStartup;

    [SerializeField] MMF_Player mmfPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        //Time.deltaTime affected by Time.scale, using alternative method
        if (timer < cooldown)
        {
            timer += (Time.realtimeSinceStartup - subtractTimeSinceStartup);
        }
        //record time passed on each update, which is used to calcualate [time since last update] on next update
        subtractTimeSinceStartup = Time.realtimeSinceStartup;
    }
    private void LateUpdate()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3)hitDirection);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (timer < cooldown) return; //skip if cooldown not reached
        Debug.Log("trigger");
        timer = 0;

        hitPos = collision.transform.position;
        //from hit point to bell position
        hitDirection = (transform.position - (Vector3)hitPos).normalized * collision.GetComponentInParent<Rigidbody2D>().velocity.magnitude;

        //feedback before force is applied
        mmfPlayer.GetFeedbackOfType<MMF_CinemachineImpulse>().Velocity = hitDirection;
        //magnitude scaled in 0~16 and rescaled
        Debug.Log(hitDirection.magnitude);
        mmfPlayer.GetFeedbackOfType<MMF_Sound>().MaxVolume = Mathf.Lerp(0.33f, 1, Mathf.InverseLerp(5, 15, hitDirection.magnitude));
        mmfPlayer.GetFeedbackOfType<MMF_Sound>().MinVolume = Mathf.Lerp(0.33f, 1, Mathf.InverseLerp(5, 15, hitDirection.magnitude));
        mmfPlayer.GetFeedbackOfType<MMF_VFX>().scale = new Vector2(1, 1) * Mathf.Lerp(0.5f, 2f, Mathf.InverseLerp(5, 15, hitDirection.magnitude));

        mmfPlayer.PlayFeedbacks();

        rb.AddForce(hitDirection, ForceMode2D.Impulse);
    }
}
