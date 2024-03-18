using UnityEngine;

public class ContinuousRotation : MonoBehaviour
{
    public float rotationSpeed = 360.0f; // Скорость вращения в градусах в секунду

    void Update()
    {
        // Вращаем объект вокруг оси Z с заданной скоростью
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
