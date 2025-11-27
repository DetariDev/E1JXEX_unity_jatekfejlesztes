using UnityEngine;
using VInspector;

public class InputManager : MonoBehaviour
{
    public static InputManager instance { get; private set; }

    [Foldout("Input példány")]
    public InputSystemActions input;

    public bool isGamepadMode = false;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        input = new InputSystemActions();

    }
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }
}
