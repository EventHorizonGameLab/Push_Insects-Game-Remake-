using UnityEngine;

public class Block_Movable_X : MonoBehaviour, IMovable
{
    float distance;
    bool dragging;
    Vector3 offset;

    public void Move()
    {
        Vector3 worldTouchPosition;
        if (Input.touchCount != 1 )
        { dragging = false; return; }

        Touch touchInput = Input.touches[0];
        Vector3 touchPosition = touchInput.position;

        if (touchInput.phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            distance = transform.position.z - Camera.main.transform.position.z;
            worldTouchPosition = new Vector3(touchPosition.x, touchPosition.y, distance);
            worldTouchPosition = Camera.main.ScreenToWorldPoint(worldTouchPosition);
            offset = transform.position - worldTouchPosition;
            dragging = true;
        }

        if (dragging && touchInput.phase == TouchPhase.Moved)
        {
            worldTouchPosition = new(touchInput.position.x, touchInput.position.y, distance);
            worldTouchPosition = Camera.main.ScreenToWorldPoint(worldTouchPosition);
            transform.position = new Vector3(worldTouchPosition.x + offset.x, transform.position.y, transform.position.z);
        }

        if (dragging && (touchInput.phase == TouchPhase.Ended || touchInput.phase == TouchPhase.Canceled))
        {
            dragging = false;
        }
    }
}
