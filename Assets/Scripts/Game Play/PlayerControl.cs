using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Animator playerAnim;
    [SerializeField] private RotateDirDisplay rotateDirDisplay;
    [SerializeField] private BeatManager beatManager;
    [SerializeField] private ComboText comboText;
    [SerializeField]private Lifes lifesScript;
    private float animSpeed = 1f;
    private const float MAX_ANIM_SPEED = 5f;
    private const float MIN_ANIM_SPEED = 0f;
    public float scoreStep = 15f;
    public float animSpeedStep = 1f;
    private float score = 0f;

    public int failCount = 0;

    public bool inputWindowOpen = false;
    private float inputWindowTimer = 0f;
    private float inputWindowDur = 2f;


    private void OnEnable()
    {
        BeatManager.OnBeat += OpenWindow;
    }

    private void OnDisable()
    {
        BeatManager.OnBeat -= OpenWindow;
    }

    private void Start()
    {
        playerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleRotationInput();
        ApplyAnimations();
        HandleWindow();
    }

    private void OpenWindow()
    {
        inputWindowOpen = true;
        inputWindowTimer = inputWindowDur;
    }

    private void HandleWindow()
    {
        if (!inputWindowOpen) return;

        inputWindowTimer -= Time.deltaTime;

        if(inputWindowTimer <= 0f)
        {
            inputWindowOpen = false;
        }
    }

    private void HandleRotationInput()
    {
        if (!inputWindowOpen) return;

        bool isPlayerRotatedCW = Input.GetKeyDown(KeyCode.D);
        bool isPlayerRotatedCCW = Input.GetKeyDown(KeyCode.A);

        if (!isPlayerRotatedCW && !isPlayerRotatedCCW) return;

        bool isCorrectDir = (isPlayerRotatedCW && rotateDirDisplay.currentDir == RotateDirDisplay.RotationDir.ClockWise) ||
            (isPlayerRotatedCCW && rotateDirDisplay.currentDir == RotateDirDisplay.RotationDir.CounterClockWise);

        if (isCorrectDir)
        {
            animSpeed += animSpeedStep;
            comboText.increaseCount();
            score += scoreStep;
            beatManager.IncreaseBPM();
            beatManager.UpdatePitch();
            rotateDirDisplay.ClearDirText();
        }
        else
        {
            score -= scoreStep;
            if(score < 0) score = 0;
            lifesScript.lifes();
            comboText.resetCount();
            animSpeed -= animSpeedStep;
            failCount += 1;
            beatManager.DecreaseBPM();
            beatManager.UpdatePitch();
            rotateDirDisplay.ClearDirText();
        }


        animSpeed = Mathf.Clamp(animSpeed, MIN_ANIM_SPEED, MAX_ANIM_SPEED);
        UIManager.Instance.UpdateScore(score);
        if (failCount >= 3) UIManager.Instance.ShowGameOver();

        inputWindowOpen = false;

    }

    private void ApplyAnimations()
    {
        Debug.Log($"Animation Speed: {animSpeed}");
        playerAnim.speed = animSpeed;
    }

}
