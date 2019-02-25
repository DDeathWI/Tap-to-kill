using UnityEngine;

public class ObjectController : MonoBehaviour
{
    // GameObject State
    private bool state;

    public void SetMaterial(Material mat, bool _state)
    {
        GetComponent<MeshRenderer>().material = mat;
        state = _state;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.Pause)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        // Change Score
                        Events.ChangeScore(state ? 1 : -1);

                        GameController.CanCreate = state;

                        Destroy(gameObject);
                    }
                }
            }

            transform.localScale *= 0.99f;

            if (transform.localScale.x < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

}
