using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float velocidadRotacion = 300f;
    [SerializeField] private Joystick joystickMovimiento;
    [SerializeField] private Joystick joystickRotacion;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (PlayerPrefs.GetInt("VolverDesdeMinijuego", 0) == 1)
        {
            controller.enabled = false;

            float x = PlayerPrefs.GetFloat("returnPosX");
            float y = PlayerPrefs.GetFloat("returnPosY");
            float z = PlayerPrefs.GetFloat("returnPosZ");
            float rotY = PlayerPrefs.GetFloat("returnRotY");

            transform.position = new Vector3(x, y, z);
            transform.eulerAngles = new Vector3(0, rotY, 0);

            controller.enabled = true;
            PlayerPrefs.SetInt("VolverDesdeMinijuego", 0);
        }
    }

    void Update()
    {
#if UNITY_ANDROID
        Vector3 direccion = new Vector3(joystickMovimiento.Horizontal, 0f, joystickMovimiento.Vertical);
        if (direccion.magnitude > 0.1f)
        {
            Vector3 mover = transform.TransformDirection(direccion.normalized);
            controller.Move(mover * velocidad * Time.deltaTime);
        }

        float rotacion = joystickRotacion.Horizontal;
        if (Mathf.Abs(rotacion) > 0.05f)
        {
            float rotacionY = rotacion * velocidadRotacion * Time.deltaTime;
            transform.Rotate(0f, rotacionY, 0f);
        }
#else
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direccion = transform.forward * vertical + transform.right * horizontal;
        controller.Move(direccion * velocidad * Time.deltaTime);

        float rotacionY = Input.GetAxis("Mouse X") * velocidadRotacion * Time.deltaTime;
        transform.Rotate(0f, rotacionY, 0f);
#endif
    }
}
