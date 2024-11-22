using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Objeto que a c�mera segue (personagem)
    public Vector3 offset = new Vector3(0, 2, -10);  // Ajuste o Z para uma posi��o negativa
    public float smoothSpeed = 1.0f;  // Suaviza��o da c�mera

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;  // Calcula a posi��o desejada
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
