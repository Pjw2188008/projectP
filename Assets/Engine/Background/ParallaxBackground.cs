using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Camera camera;

    public float speed = 1;

    private Vector3 origin;
    private Vector3 startPosition;

    private void Start()
    {
        origin = transform.position;
        startPosition = camera.transform.position;
    }

    private void Update()
    {
        Vector2 newPos = origin + camera.transform.position + (startPosition - camera.transform.position) * speed; 
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}
