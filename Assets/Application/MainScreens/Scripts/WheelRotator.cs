using UnityEngine;

public class WheelRotatorMobile : MonoBehaviour
{
    public float rotationSpeed = 0.5f;



    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // On ne s'intéresse qu'au déplacement (pas au tap ou au début/fin du toucher)
            if (touch.phase == TouchPhase.Moved)
            {
                float deltaX = touch.deltaPosition.x;
                transform.Rotate(0, 0, -deltaX * rotationSpeed);
            }
        }
    }
}

