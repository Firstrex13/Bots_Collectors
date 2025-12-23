using UnityEngine;

public class Flag : MonoBehaviour
{
    public void TurnOn()
    {
        gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);

            transform.position = Vector3.zero;
        }
    }
}
