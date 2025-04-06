using UnityEngine;

namespace Axoloop.Global.UI
{
    public class DotSwingLoader : MonoBehaviour
    {
        public Transform dotLeft;
        public Transform dotRight;
        public float maxAngle = 70f;
        public float speed = 1.4f;

        private float time;

        void Update()
        {
            time += Time.deltaTime;
            float normalizedTime = (time % speed) / speed;

            // Left Dot Animation: Keyframe mimic
            float leftAngle = 0f;
            if (normalizedTime < 0.25f)
            {
                float t = EaseOut(normalizedTime / 0.25f);
                leftAngle = Mathf.Lerp(0f, maxAngle, t);
            }
            else if (normalizedTime < 0.5f)
            {
                float t = EaseIn((normalizedTime - 0.25f) / 0.25f);
                leftAngle = Mathf.Lerp(maxAngle, 0f, t);
            }

            // Right Dot Animation: offset phase
            float rightAngle = 0f;
            if (normalizedTime > 0.5f && normalizedTime < 0.75f)
            {
                float t = EaseOut((normalizedTime - 0.5f) / 0.25f);
                rightAngle = Mathf.Lerp(0f, -maxAngle, t);
            }
            else if (normalizedTime >= 0.75f)
            {
                float t = EaseIn((normalizedTime - 0.75f) / 0.25f);
                rightAngle = Mathf.Lerp(-maxAngle, 0f, t);
            }

            // Apply rotations
            if (dotLeft != null)
                dotLeft.localRotation = Quaternion.Euler(0, 0, leftAngle);

            if (dotRight != null)
                dotRight.localRotation = Quaternion.Euler(0, 0, rightAngle);
        }

        float EaseIn(float t) => t * t;
        float EaseOut(float t) => 1 - Mathf.Pow(1 - t, 2);
    }
}

