using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.VFX;
using Unity.VisualScripting;

namespace MoreMountains.Feedbacks
{
    [AddComponentMenu("")]
    [FeedbackHelp("Feedback for Visual Effect")]
    [FeedbackPath("#Custom/VFX")]
    public class MMF_VFX : MMF_Feedback
    {
        /// a static bool used to disable all feedbacks of this type at once
        public static bool FeedbackTypeAuthorized = true;
        /// use this override to specify the duration of your feedback (don't hesitate to look at other feedbacks for reference)
        public override float FeedbackDuration { get { return 0f; } }
        /// pick a color here for your feedback's inspector
#if UNITY_EDITOR
        public override Color FeedbackColor { get { return MMFeedbacksInspectorColors.DebugColor; } }
#endif

        [MMFInspectorGroup("VFX", true, 12, true)]

        [Tooltip("the VisualEffect you want to play")]
        public VisualEffect vfx;

        [Tooltip("the Bell object for setting the vfx to hit spot")]
        public ObjBell bell;
        
        [Tooltip("the scale of the VisualEffect")]
        public Vector2 scale = new Vector2(1, 1);

        protected override void CustomInitialization(MMF_Player owner)
        {
            base.CustomInitialization(owner);
            // your init code goes here
        }

        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
        {
            if (!Active || !FeedbackTypeAuthorized)
            {
                return;
            }
            // your play code goes here
            if (bell != null) {
                vfx.transform.position = bell.HitPos;
                //Vector2.Angle compares directional Vector2; HitDirection *-1 for reversed direction
                vfx.transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.right, bell.HitDirection * -1), Vector3.forward);
            };

            vfx.transform.localScale = scale;
            vfx.Play();

        }

        protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1)
        {
            if (!FeedbackTypeAuthorized)
            {
                return;
            }
            // your stop code goes here

            vfx.Stop();

        }
    }
}