using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    private bool _isMoving;

    private Board _board;
    
    [SerializeField]
    private InterpType _interpolation = InterpType.SmootherStep;

    public MatchValue _matchValue;

    public enum InterpType
    {
        Linear,
        EaseOut,
        EaseIn,
        Exponential,
        SmoothStep,
        SmootherStep
    };

    public enum MatchValue
    {
        Yellow,
        Blue,
        Magenta,
        Indigo,
        Green,
        Teal,
        Red,
        Cyan,
        Wild
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void Init(Board board)
    {
        _board = board;
    }

    public void SetCoord(int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }

    public void Move(int destX, int destY, float timeToMove)
    {
        if (!_isMoving)
        {
            _isMoving = true;

            StartCoroutine(MoveRoutine(new Vector3(destX, destY, 0), timeToMove));
        }
    }

    IEnumerator MoveRoutine(Vector3 destination, float timeToMove)
    {
        Vector3 startPosition = transform.position;

        bool reachedDestination = false;

        float elapsedTime = 0f;

        while (!reachedDestination && (elapsedTime < timeToMove))
        {
            if (Vector3.Distance(transform.position, destination) < 0.01f)
            {
                reachedDestination = true;

                if (_board != null)
                {
                    _board.PlaceGamePiece(this, (int) destination.x, (int) destination.y);
                }

                break;
            }
            
            elapsedTime += Time.deltaTime;

            float timeFactor = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);

            timeFactor = Interpolate(timeFactor);

            transform.position = Vector3.Lerp(startPosition, destination, timeFactor);
            
            yield return null;
        }

        _isMoving = false;
    }

    float Interpolate(float timeFactor)
    {
        switch (_interpolation)
        {
            case InterpType.Linear:
                timeFactor = Linear(timeFactor);
                break;

            case InterpType.EaseOut:
                timeFactor = EaseOut(timeFactor);
                break;

            case InterpType.EaseIn:
                timeFactor = EaseIn(timeFactor);
                break;

            case InterpType.Exponential:
                timeFactor = Exponential(timeFactor);
                break;

            case InterpType.SmoothStep:
                timeFactor = SmoothStep(timeFactor);
                break;

            case InterpType.SmootherStep:
                timeFactor = SmootherStep(timeFactor);
                break;

            default:
                break;
        }

        return timeFactor;
    }

    float Linear(float timeFactor)
    {
        return timeFactor;
    }

    float EaseOut(float timeFactor)
    {
        return Mathf.Sin(timeFactor * Mathf.PI * 0.5f);
    }

    float EaseIn(float timeFactor)
    {
        return 1 - Mathf.Cos(timeFactor * Mathf.PI * 0.5f);
    }

    float Exponential(float timeFactor)
    {
        return timeFactor * timeFactor;
    }

    float SmoothStep(float timeFactor)
    {
        return timeFactor * timeFactor * (3 - 2 * timeFactor);
    }

    float SmootherStep(float timeFactor)
    {
        return timeFactor * timeFactor * timeFactor * ((timeFactor * (timeFactor * 6 - 15)) + 10);
    }
}