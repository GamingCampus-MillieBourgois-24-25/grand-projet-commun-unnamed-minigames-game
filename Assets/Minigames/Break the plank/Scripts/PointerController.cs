using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class PointerController : MonoBehaviour, IMinigameController
{
    #region Inspector Properties

    [SerializeField] ContinueText continueText;


    [Header("Game Configuration")]
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private float speedIncrease = 10f;
    [SerializeField] private float shakeDuration = 0.15f;
    [SerializeField] private float shakeMagnitude = 0.08f;
    [SerializeField] private int maxFailures = 2;

    [Header("Animation Configuration")]
    [SerializeField] private float hammerRecoilDuration = 0.2f;
    [SerializeField] private float hammerStrikeDuration = 0.2f;
    [SerializeField] private float hammerReturnDuration = 0.2f;
    [SerializeField] private float safeZoneFadeDuration = 0.7f;
    [SerializeField] private Ease hammerRecoilEase = Ease.OutQuad;
    [SerializeField] private Ease hammerStrikeEase = Ease.InQuad;
    [SerializeField] private Ease hammerReturnEase = Ease.InOutQuad;

    


    [Header("References")]
    public MinigameObject breakThePlank;
    public Transform pointA;
    public Transform pointB;
    public RectTransform safeZone;
    public LevelManager levelManager;
    public SpriteAnimator spriteAnimator;

    [Header("UI Elements")]
    public Text startText;
    public Text victoryText;
    public Text loseText;

    [Header("Character States")]
    public GameObject axoVictory;
    public GameObject axoLose;
    public GameObject axoWaiting;
    public GameObject axoPointerMove;
    public GameObject axoSuccess;

    [Header("Plank UI Elements")]
    public RectTransform plankFull;
    public RectTransform plankBreakMiddle;
    public RectTransform plankBreakRight;
    public RectTransform plankBreakLeft;

    #endregion

    #region Private Fields

    private RectTransform _pointerTransform;
    private Vector3 _targetPosition;
    private bool _canMove = false;
    private bool _isAnimating = false;
    private int _successCount = 0;
    private int _failCount = 0;
    private int _successNeeded = 2;

    private Camera _mainCamera;
    private Transform _mainCameraTransform;
    private Vector3 _originalCameraPosition;
    private Volume _postProcessingVolume;
    private DepthOfField _depthOfField;

    // DOTween Sequences
    private Sequence _pointerMoveSequence;
    private Sequence _hammerSequence;
    private Sequence _cameraShakeSequence;
    private Tween _safeZoneFadeTween;

    #endregion

    #region Lifecycle Methods

    private void Awake()
    {
        // Initialize DOTween (only needed once)
        DOTween.SetTweensCapacity(500, 50);
    }

    private void Start()
    {
        if (!InitializeComponents()) return;

        ResetGameState();
        InitializeUI();

        if (spriteAnimator != null)
        {
            spriteAnimator.StartRotation();
        }

        StartGameSequence();
    }

    private void Update()
    {
        if (!_canMove || _isAnimating) return;

        HandlePointerMovement();
        CheckForInput();
    }

    private void OnDestroy()
    {
        // Kill all tweens when object is destroyed
        DOTween.Kill(transform);
        DOTween.Kill(_mainCameraTransform);
        DOTween.Kill(safeZone);

        _pointerMoveSequence?.Kill();
        _hammerSequence?.Kill();
        _cameraShakeSequence?.Kill();
        _safeZoneFadeTween?.Kill();
    }

    #endregion

    #region Initialization

    private bool InitializeComponents()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("Points A and B are not assigned!");
            return false;
        }

        _pointerTransform = GetComponent<RectTransform>();
        if (_pointerTransform == null)
        {
            Debug.LogError("RectTransform missing on the PointerController object!");
            return false;
        }

        _targetPosition = pointB.position;

        InitializeCamera();
        return true;
    }

    private void InitializeCamera()
    {
        _mainCamera = Camera.main;
        if (_mainCamera != null)
        {
            _mainCameraTransform = _mainCamera.transform;
            _originalCameraPosition = _mainCameraTransform.position;

            _postProcessingVolume = _mainCamera.GetComponent<Volume>();
            if (_postProcessingVolume != null)
            {
                _postProcessingVolume.profile.TryGet(out _depthOfField);
            }
        }
        else
        {
            Debug.LogError("Main camera not found!");
        }
    }

    private void InitializeUI()
    {
        if (loseText != null)
            loseText.gameObject.SetActive(false);

        UpdateCharacterState(CharacterState.Waiting);
        ResetPlankState();
    }

    private void ResetGameState()
    {
        _successCount = 0;
        _failCount = 0;
        _canMove = false;
        _isAnimating = false;
    }

    #endregion

    #region IMinigameController Implementation

    public void GenerateMinigame(int seed, MinigameDifficultyLevel difficultyLevel)
    {
        Debug.Log($"Generating minigame with seed {seed} and difficulty {difficultyLevel}");
        UnityEngine.Random.InitState(seed);

        // Configure game parameters based on difficulty
        ConfigureDifficulty(difficultyLevel);

        // Reset game state
        ResetPlankState();
        ResetGameState();
    }

    public void InitializeMinigame()
    {
        Debug.Log("Initializing minigame...");
        UpdateCharacterState(CharacterState.Waiting);
        StartGameSequence();
    }

    public void StartMinigame()
    {
        Debug.Log("Starting minigame...");
        _canMove = true;
    }

    private void ConfigureDifficulty(MinigameDifficultyLevel difficultyLevel)
    {
        switch (difficultyLevel)
        {
            case MinigameDifficultyLevel.VeryEasy:
                moveSpeed = 50f;
                _successNeeded = 1;
                break;
            case MinigameDifficultyLevel.Easy:
                moveSpeed = 75f;
                _successNeeded = 2;
                break;
            case MinigameDifficultyLevel.Medium:
                moveSpeed = 100f;
                _successNeeded = 3;
                break;
            case MinigameDifficultyLevel.Hard:
                moveSpeed = 125f;
                _successNeeded = 4;
                break;
            case MinigameDifficultyLevel.VeryHard:
                moveSpeed = 150f;
                _successNeeded = 5;
                break;
            case MinigameDifficultyLevel.Impossible:
                moveSpeed = 200f;
                _successNeeded = 6;
                break;
            default:
                moveSpeed = 100f;
                _successNeeded = 2;
                break;
        }
    }

    #endregion

    #region Game Logic

    //private void HandlePointerMovement()
    //{
    //    UpdateCharacterState(CharacterState.PointerMove);

    //    // Create a new DOTween sequence for pointer movement if one doesn't exist
    //    if (_pointerMoveSequence == null || !_pointerMoveSequence.IsActive())
    //    {
    //        // Calculate duration based on speed and distance
    //        float distance = Vector3.Distance(_pointerTransform.position, _targetPosition);
    //        float duration = distance / moveSpeed;

    //        _pointerMoveSequence = DOTween.Sequence();
    //        _pointerMoveSequence.Append(_pointerTransform.DOMove(_targetPosition, duration).SetEase(Ease.Linear));
    //        _pointerMoveSequence.OnComplete(() => {
    //            // Switch target when reaching a point
    //            _targetPosition = (_targetPosition == pointA.position) ? pointB.position : pointA.position;
    //            HandlePointerMovement(); // Restart the sequence for continuous motion
    //        });
    //    }
    //}
    private void HandlePointerMovement()
    {
        UpdateCharacterState(CharacterState.PointerMove);

        if (_pointerMoveSequence == null || !_pointerMoveSequence.IsActive())
        {
            _pointerTransform.position = Vector3.MoveTowards(
                _pointerTransform.position,
                _targetPosition,
                moveSpeed * Time.deltaTime
            );
        }

        if (Vector3.Distance(_pointerTransform.position, pointA.position) < 0.1f)
            _targetPosition = pointB.position;
        else if (Vector3.Distance(_pointerTransform.position, pointB.position) < 0.1f)
            _targetPosition = pointA.position;
    }

    private void CheckForInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) || IsTouching())
            ExecuteHammerAction();
    }

    private bool IsTouching()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    private void ExecuteHammerAction()
    {
        // Kill the current pointer movement sequence
        _pointerMoveSequence?.Kill();

        // Perform hammer animation with DOTween
        HammerEffectWithDOTween();
    }

    private void EvaluateHammerHit()
    {
        if (safeZone == null)
        {
            Debug.LogError("Safe Zone not assigned!");
            return;
        }

        if (IsPointerInSafeZone())
        {
            HandleSuccessfulHit();
        }
        else
        {
            HandleFailedHit();
        }
    }

    private bool IsPointerInSafeZone()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(safeZone, _pointerTransform.position, null);
    }

    private void HandleSuccessfulHit()
    {
        _successCount++;
        moveSpeed += speedIncrease;

        ShakeCameraWithDOTween();
        Debug.Log($"Success {_successCount}/{_successNeeded}");

        UpdateCharacterState(CharacterState.Success);

        if (_successCount == _successNeeded)
        {
            UpdatePlankState();
            FadeOutSafeZoneWithDOTween();
        }

        if (levelManager != null && _successCount < _successNeeded)
        {
            levelManager.UpdateSafeZone();
        }

        if (_successCount >= _successNeeded)
        {
            ShowVictorySequence();
        }
    }

    private void HandleFailedHit()
    {
        _failCount++;
        Debug.Log($"Failure! Number of failures: {_failCount}");

        if (_failCount >= maxFailures)
        {
            ShowDefeatSequence();
        }
    }

    private void ShowDefeatSequence()
    {
        _canMove = false;
        UpdateCharacterState(CharacterState.Lose);
        ResetPlankState();

        if (spriteAnimator != null)
        {
            spriteAnimator.MoveOutOfView();
        }

        if (loseText != null)
        {
            loseText.gameObject.SetActive(true);
        }

        Debug.Log("You lost!");
        continueText.Enable(false);
    }


    #endregion

    #region UI Management

    private enum CharacterState
    {
        Victory,
        Lose,
        Waiting,
        PointerMove,
        Success
    }

    private void UpdateCharacterState(CharacterState state)
    {
        // Deactivate all images
        axoVictory?.SetActive(false);
        axoLose?.SetActive(false);
        axoWaiting?.SetActive(false);
        axoPointerMove?.SetActive(false);
        axoSuccess?.SetActive(false);

        // Activate only the corresponding image
        switch (state)
        {
            case CharacterState.Victory:
                axoVictory?.SetActive(true);
                break;
            case CharacterState.Lose:
                axoLose?.SetActive(true);
                break;
            case CharacterState.Waiting:
                axoWaiting?.SetActive(true);
                break;
            case CharacterState.PointerMove:
                axoPointerMove?.SetActive(true);
                break;
            case CharacterState.Success:
                axoSuccess?.SetActive(true);
                break;
        }
    }

    private void UpdatePlankState()
    {
        ResetPlankState();

        float safeZoneX = safeZone.anchoredPosition.x;

        if (safeZoneX < -100)
        {
            plankBreakLeft.gameObject.SetActive(true);
            plankFull.gameObject.SetActive(false);
        }
        else if (safeZoneX >= -100 && safeZoneX <= 100)
        {
            plankBreakMiddle.gameObject.SetActive(true);
            plankFull.gameObject.SetActive(false);
        }
        else
        {
            plankBreakRight.gameObject.SetActive(true);
            plankFull.gameObject.SetActive(false);
        }
    }

    private void ResetPlankState()
    {
        plankBreakMiddle.gameObject.SetActive(false);
        plankBreakRight.gameObject.SetActive(false);
        plankBreakLeft.gameObject.SetActive(false);
        plankFull.gameObject.SetActive(true);
    }

    #endregion

    #region DOTween Animations

    private void StartGameSequence()
    {
        if (startText != null)
        {
            startText.gameObject.SetActive(true);

            // Fade in and out the start text
            Sequence startSequence = DOTween.Sequence();
            startSequence.AppendInterval(1f);
            startSequence.AppendCallback(() => {
                startText.gameObject.SetActive(false);

                // Update safe zone position
                if (levelManager != null)
                {
                    levelManager.UpdateSafeZone();
                }
            });
            startSequence.AppendInterval(0.5f);
            startSequence.AppendCallback(() => {
                _canMove = true;
            });
        }
        else
        {
            // If no start text, just activate movement after a short delay
            DOVirtual.DelayedCall(0.5f, () => {
                if (levelManager != null)
                {
                    levelManager.UpdateSafeZone();
                }
                _canMove = true;
            });
        }
    }

    private void HammerEffectWithDOTween()
    {
        _isAnimating = true;
        Vector3 originalPosition = _pointerTransform.position;
        Vector3 recoilPosition = originalPosition + new Vector3(0, 50f, 0);
        Vector3 hammerPosition = originalPosition + new Vector3(0, -50f, 0);

        // Create a new sequence for the hammer effect
        _hammerSequence = DOTween.Sequence();

        // Step 1: Move up for recoil
        _hammerSequence.Append(_pointerTransform.DOMove(recoilPosition, hammerRecoilDuration).SetEase(hammerRecoilEase));

        // Step 2: Move down for hammer strike
        _hammerSequence.Append(_pointerTransform.DOMove(hammerPosition, hammerStrikeDuration).SetEase(hammerStrikeEase));

        // Evaluate hit
        _hammerSequence.AppendCallback(EvaluateHammerHit);

        // Step 3: Return to original position
        _hammerSequence.Append(_pointerTransform.DOMove(originalPosition, hammerReturnDuration).SetEase(hammerReturnEase));

        // Final callback
        _hammerSequence.AppendCallback(() => {
            _isAnimating = false;
            // Restart pointer movement if game is still active
            if (_canMove)
            {
                HandlePointerMovement();
            }
        });
    }

    private void ShakeCameraWithDOTween()
    {
        if (_mainCameraTransform == null) return;

        // Kill previous shake if it's still running
        _cameraShakeSequence?.Kill();

        // Create a new sequence for camera shake
        _cameraShakeSequence = DOTween.Sequence();

        // Initial position
        Vector3 originalPosition = _originalCameraPosition;

        // Add shake movements
        int shakeCount = 10; // Number of shake movements
        for (int i = 0; i < shakeCount; i++)
        {
            Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * shakeMagnitude;
            _cameraShakeSequence.Append(_mainCameraTransform.DOMove(originalPosition + randomOffset, shakeDuration / shakeCount));
        }

        // Return to original position
        _cameraShakeSequence.Append(_mainCameraTransform.DOMove(originalPosition, 0.05f));
    }

    private void FadeOutSafeZoneWithDOTween()
    {
        if (safeZone == null) return;

        CanvasGroup canvasGroup = safeZone.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = safeZone.gameObject.AddComponent<CanvasGroup>();
        }

        // Kill previous fade if it's still running
        _safeZoneFadeTween?.Kill();

        // Create a new tween for fading out
        _safeZoneFadeTween = canvasGroup.DOFade(0f, safeZoneFadeDuration)
            .OnComplete(() => {
                safeZone.gameObject.SetActive(false);
            });
    }

    private void ShowVictorySequence()
    {
        _canMove = false;

        // Kill all active sequences
        _pointerMoveSequence?.Kill();

        // Create a sequence for victory animation
        Sequence victorySequence = DOTween.Sequence();

        // Step 1: Show success state
        victorySequence.AppendCallback(() => {
            UpdateCharacterState(CharacterState.Success);
            if (spriteAnimator != null)
            {
                spriteAnimator.StartBounce();
            }
        });

        // Wait for 2 seconds
        victorySequence.AppendInterval(2f);

        // Step 2: Show victory state and text
        victorySequence.AppendCallback(() => {
            UpdateCharacterState(CharacterState.Victory);
            if (victoryText != null)
            {
                victoryText.gameObject.SetActive(true);
            }
        });

        // Wait for 2 seconds
        victorySequence.AppendInterval(2f);

        // Step 3: Load next minigame
        continueText.Enable(true);
    }

    #endregion
}