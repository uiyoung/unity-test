using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveHorizontalAxisName = "Horizontal";
    public string moveVerticalAxisName = "Vertical";
    public string fireButtonName = "Fire1";
    public string jumpButtonName = "Jump";
    public string reloadButtonName = "Reload";

    public Vector2 MoveInput { get; private set; }
    public bool Fire { get; private set; }
    public bool Reload { get; private set; }
    public bool Jump { get; private set; }

    private void Update()
    {
        // 게임오버상태에서는 입력을 감지하지 않는다
        if (GameManager.Instance != null && GameManager.Instance.IsGameover)
        {
            MoveInput = Vector2.zero;
            Fire = false;
            Reload = false;
            Jump = false;
            return;
        }

        MoveInput = new Vector2(Input.GetAxis(moveHorizontalAxisName), Input.GetAxis(moveVerticalAxisName));
        if (MoveInput.sqrMagnitude > 1)
            MoveInput = MoveInput.normalized;

        Jump = Input.GetButtonDown(jumpButtonName);
        Fire = Input.GetButton(fireButtonName);
        Reload = Input.GetButtonDown(reloadButtonName);
    }
}