using UnityEngine;

public class EnvironmentDrift : MonoBehaviour
{
    [Header("Settings")]
    public float movementAmount = 0.05f; // How far it moves
    public float speed = 0.5f;           // How fast it moves

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calculate a smooth movement using Sine waves
        float x = Mathf.Sin(Time.time * speed) * movementAmount;
        float y = Mathf.Sin(Time.time * speed * 1.2f) * movementAmount;

        transform.position = initialPosition + new Vector3(x, y, 0);
    }
}