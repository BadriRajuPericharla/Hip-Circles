using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Animator playerAnim;
    [SerializeField] private RotateDirDisplay rotateDirDisplay;
    private float animSpeed = 1f;
    private const float MAX_ANIM_SPEED = 5f;
    private const float MIN_ANIM_SPEED = 0f;
    public float scoreStep = 15f;
    public float animSpeedStep = 1f;
    private float score = 0f;

    public int failCount = 0;


    private void Start()
    {
        playerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleRotationInput();
        ApplyAnimations();
    }

    private void HandleRotationInput()
    {
        bool isPlayerRotatedCW = Input.GetKeyDown(KeyCode.D);
        bool isPlayerRotatedCCW = Input.GetKeyDown(KeyCode.A);

        if (!isPlayerRotatedCW && !isPlayerRotatedCCW) return;

        bool isCorrectDir = (isPlayerRotatedCW && rotateDirDisplay.currentDir == RotateDirDisplay.RotationDir.ClockWise) ||
            (isPlayerRotatedCCW && rotateDirDisplay.currentDir == RotateDirDisplay.RotationDir.CounterClockWise);

        if (isCorrectDir)
        {
            animSpeed += animSpeedStep;
            score += scoreStep;
        }
        else
        {
            animSpeed -= animSpeedStep;
            score -= scoreStep;
            failCount += 1;
        }


        animSpeed = Mathf.Clamp(animSpeed, MIN_ANIM_SPEED, MAX_ANIM_SPEED);
        UIManager.Instance.UpdateScore(score);
        if (failCount >= 3) UIManager.Instance.ShowGameOver();

        rotateDirDisplay.AssignNewRandomDir();
    }

    private void ApplyAnimations()
    {
        Debug.Log($"Animation Speed: {animSpeed}");
        playerAnim.speed = animSpeed;
    }

}
