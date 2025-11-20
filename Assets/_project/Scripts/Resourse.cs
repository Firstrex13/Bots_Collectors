using UnityEngine;

public class Resourse : MonoBehaviour
{
    private Transform _transform;

    private void Start()
    {
        _transform = transform;

        _transform.position = new Vector3(Random.Range(-10, 10), 0.5f, Random.Range(-10, 10));
    }
}
