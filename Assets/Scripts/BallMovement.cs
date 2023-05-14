using DG.Tweening;
using GG.Infrastructure.Utils.Swipe;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private SwipeListener swipeListener;
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private float stepDuration = 0.1f;
    [SerializeField] private LayerMask wallsAndRoadsLayer;
    private const float MaxRayDistance = 10f;

    private Vector3 _moveDirection;
    private bool _canMove = true;

    private void Start()
    {
        transform.position = levelManager.defaultBallRoadTile.position;

        swipeListener.OnSwipe.AddListener(swipe =>
        {
            _moveDirection = swipe switch
            {
                "Right" => Vector3.right,
                "Left" => Vector3.left,
                "Up" => Vector3.forward,
                "Down" => Vector3.back,
                _ => _moveDirection
            };
            Debug.Log(swipe);

            MoveBall();
        });
    }

    private void MoveBall()
    {
        if (_canMove)
        {
            _canMove = false;

            RaycastHit[] hits = Physics.RaycastAll(transform.position, _moveDirection, MaxRayDistance,
                wallsAndRoadsLayer.value);

            Vector3 targetPosition = transform.position;

            int steps = 0;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.isTrigger)
                {
                }
                else
                {
                    // Wall tile
                    if (i == 0)
                    {
                        // means wall is near the ball
                        _canMove = true;
                        return;
                    }

                    //else:
                    steps = i;
                    targetPosition = hits[i - 1].transform.position;
                    break;
                }
            }

            float moveDuration = stepDuration * steps;

            transform.DOMove(targetPosition, moveDuration).SetEase(Ease.OutExpo).OnComplete(() => _canMove = true);
        }
    }
}