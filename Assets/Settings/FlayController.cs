using GameManagement;
using UnityEngine;

public class FlayController : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float _maxRiseSpeed = 5f; 
    [SerializeField, Range(0, 10)] private float _fallSpeed = 3f; 
    [SerializeField, Range(0, 40)] private float _riseAcceleration = 20f; 
    [SerializeField, Range(0, 10)] private float _riseRotationSpeed = 4.5f; 
    [SerializeField, Range(0, 10)] private float _fallRotationSpeed = 1.5f;
    [SerializeField, Range(-90, 90)] private float _targetRotation = 30f;

    private Rigidbody2D _rb;

    private bool _isButtonHeld = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        GameManager.Instance.OnGameStarted += HandleGameStarted;
        GameManager.Instance.OnGameEnded += HandleGameEnded;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStarted -= HandleGameStarted;
        GameManager.Instance.OnGameEnded -= HandleGameEnded;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space) || Input.GetButton("Joystick button A"))
        {
            _isButtonHeld = true;
        }
        else
        {
            _isButtonHeld = false;
        }
    }

    private void FixedUpdate()
    {
        HandleGameStarted();
    }

    private void HandleGameStarted()
    {
        if (_isButtonHeld)
        {
            Rise();
            RotateTowards(30f, _riseRotationSpeed);
        }
        else
        {
            Fall();
            RotateTowards(-30f, _fallRotationSpeed);
        }
    }

    private void Rise()
    {
        float newVelocityY = Mathf.Min(_rb.velocity.y + _riseAcceleration * Time.fixedDeltaTime, _maxRiseSpeed);
        _rb.velocity = new Vector2(_rb.velocity.x, newVelocityY);
    }

    private void Fall()
    {
        float newVelocityY = Mathf.Max(_rb.velocity.y - _fallSpeed * Time.fixedDeltaTime, -_maxRiseSpeed);
        _rb.velocity = new Vector2(_rb.velocity.x, newVelocityY);
    }

    private void RotateTowards(float targetRotationZ, float currentRotationSpeed)
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetRotationZ);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, currentRotationSpeed * Time.fixedDeltaTime);
    }

    private void HandleGameEnded()
    {
        GameManager.Instance.EndGame();
    }

    private void DestroyPlayer()
    {
        // Здесь код для обработки уничтожения игрока
        // Например, можно отключить игрока, показать анимацию взрыва, перезапустить уровень и т.д

        // Загрузить сцену с тем же индексом
        gameObject.SetActive(false);
        // _isGameStarted = false;
        Debug.Log("Player has been destroyed");
        HandleGameEnded();

        // Пример отключения объекта игрока
        // Можно добавить вызов методов для анимации взрыва или перезапуска уровня
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            DestroyPlayer();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            DestroyPlayer();
        }
    }
}
