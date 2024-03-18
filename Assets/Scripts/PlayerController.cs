using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameManagement;

public class PlayerController : MonoBehaviour
{
    public static int DeadCounter;

    [SerializeField, Range(-360, 360)] private float _angel = -90f;
    [SerializeField, Range(1f, 20f)] private float _jumpForce = 17f;
    [SerializeField, Range(0f, 1f)] private float _rotationDuration = 0.4f;
    [SerializeField, Range(0f, 1f)] private float _boxCastDistance = 0.62f;
    [SerializeField, Range(-0.5f, 2f)] private float _targetXPosition; // Целевая координата X для приземления
    [SerializeField, Range(0f, 10f)] private float _jumpHeight = 5f; // Высота прыжка

    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Vector2 _boxCastSize = new Vector2(1.33f, 0.1f);

    private Rigidbody2D _rb;

    private bool _isRotating = false;

    private float _defaultAngle = -90f;
    private float _targetPositionX = 6f;

    public void DestroyPlayer()
    {
        // Здесь код для обработки уничтожения игрока
        // Например, можно отключить игрока, показать анимацию взрыва, перезапустить уровень и т.д

        // Загрузить сцену с тем же индексом
        gameObject.SetActive(false);
        // _isGameStarted = false;
        Debug.Log("Player has been destroyed");

        DeadCounter++;
        HandleGameEnded();

        // Пример отключения объекта игрока
        // Можно добавить вызов методов для анимации взрыва или перезапуска уровня
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStarted -= HandleGameStarted;
        GameManager.Instance.OnGameEnded -= HandleGameEnded;
    }

    public void Restart()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        LaunchPlayerToTargetX();
        GameManager.Instance.OnGameStarted += HandleGameStarted;
        GameManager.Instance.OnGameEnded += HandleGameEnded;
    }

    private void FixedUpdate()
    {
        // Вся логика теперь внутри HandleGameStarted
        HandleGameStarted();

    }

    private void HandleGameStarted()
    {
        HandleGroundCheck();
        HandleJumpInput();
        CheckIfReachedTargetX();

    }

    private void HandleGameEnded()
    {
        GameManager.Instance.EndGame();
    }

    private bool CheckIfGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        Vector2 size = _boxCastSize;

        RaycastHit2D hit = Physics2D.BoxCast(position, size, 0f, direction, _boxCastDistance, _groundLayer);

        // Визуализация BoxCast
        Vector2 bottomCenter = position + direction * _boxCastDistance;
        Vector2 topLeft = new Vector2(bottomCenter.x - size.x / 2, bottomCenter.y + size.y / 2);
        Vector2 topRight = new Vector2(bottomCenter.x + size.x / 2, bottomCenter.y + size.y / 2);
        Vector2 bottomLeft = new Vector2(bottomCenter.x - size.x / 2, bottomCenter.y - size.y / 2);
        Vector2 bottomRight = new Vector2(bottomCenter.x + size.x / 2, bottomCenter.y - size.y / 2);

        Debug.DrawLine(topLeft, topRight, hit.collider != null ? Color.green : Color.red);
        Debug.DrawLine(topRight, bottomRight, hit.collider != null ? Color.green : Color.red);
        Debug.DrawLine(bottomRight, bottomLeft, hit.collider != null ? Color.green : Color.red);
        Debug.DrawLine(bottomLeft, topLeft, hit.collider != null ? Color.green : Color.red);

        return hit.collider != null;
    }

    private void Jump()
    {
        _rb.velocity = Vector2.up * _jumpForce;
        GameManager.Instance.PlayerJumped(); // Замените на это
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

    private void HandleGroundCheck()
    {
        if (CheckIfGrounded())
        {
            GameManager.Instance.PlayerLanded();
        }
    }

    private void HandleJumpInput()
    {
        bool jumpInput = Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space) || Input.GetButton("Joystick button A");

        if (jumpInput && CheckIfGrounded() && !_isRotating)
        {
            Jump();
            StartCoroutine(RotateOverTime(_defaultAngle));
        }
    }

    private void CheckIfReachedTargetX()
    {
        if (transform.position.x >= _targetPositionX)
        {
            _rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            _rb.freezeRotation = true;
        }
    }

    private void LaunchPlayerToTargetX()
    {
        float gravity = Physics2D.gravity.magnitude * _rb.gravityScale;
        float distanceToTarget = _targetXPosition - transform.position.x;
        float velocityX = Mathf.Sqrt(gravity * distanceToTarget / Mathf.Sin(2 * 45 * Mathf.Deg2Rad));

        float velocityY = Mathf.Sqrt(2 * gravity * _jumpHeight);

        Vector2 launchVelocity = new Vector2(velocityX, velocityY);

        _rb.velocity = launchVelocity;

        StartCoroutine(RotateOverTime(_angel));
    }

    IEnumerator RotateOverTime(float angle)
    {
        _isRotating = true;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, angle);
        float time = 0.0f;

        while (time < _rotationDuration)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, time / _rotationDuration);
            yield return null;
        }

        Vector3 rotationEuler = transform.eulerAngles;
        rotationEuler.z = Mathf.Round(rotationEuler.z / 90) * 90;
        transform.eulerAngles = rotationEuler;

        _isRotating = false;
    }
}
