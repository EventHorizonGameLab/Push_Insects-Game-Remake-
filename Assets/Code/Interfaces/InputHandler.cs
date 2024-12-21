using UnityEngine;

public class InputHandler : MonoBehaviour
{
    IMovable block;

    private void Update()
    {
        DetectTouchInput();

        if (block != null)
        {
            block.Move();
        }
    }

    private void DetectTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    IMovable touchedBlock = hit.collider.GetComponent<IMovable>();
                    block = touchedBlock != null ? touchedBlock : null;
                }
                else
                {
                    block = null;
                }
            }
        }
    }
}

