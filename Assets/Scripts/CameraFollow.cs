using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Objeto que a câmera segue (personagem)
    public Vector3 offset = new Vector3(0, 2, -10);  // Ajuste o Z para uma posição negativa
    public float smoothSpeed = 1.0f;  // Suavização da câmera

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;  // Calcula a posição desejada
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
