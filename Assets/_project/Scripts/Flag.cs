using UnityEngine;

public class Flag : MonoBehaviour
{
    public void TurnOnFlag()
    {
        Debug.Log("TurnedOn");
        gameObject.SetActive(true);

    }

    public void TurnOffFlag()
    {
        Debug.Log("TurnedOff");
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);

            transform.position = Vector3.zero;
        }
    }
}
