using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private Vector3 _targetPosition;
    private GridPosition gridPosition;

    private float _moveSpeed = 4f;
    private float _rotateSpeed = 10f;
    private float _stopLimit = .1f;

    private void Awake() 
    {
        _targetPosition = transform.position;
    }

    private void Start() 
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetUnitAtGridPosition(gridPosition, this);
    }

    private void Update() 
    {
        if(Vector3.Distance(_targetPosition, transform.position) > _stopLimit)
        {
            Vector3 movePosition = (_targetPosition - transform.position).normalized;
            transform.position += movePosition * Time.deltaTime * _moveSpeed;
            transform.forward = Vector3.Lerp(transform.forward, movePosition, Time.deltaTime * _rotateSpeed);
            _animator.SetBool("IsWalking", true);
        }
        else
            _animator.SetBool("IsWalking", false);

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {
            // Unit changed Grid Position | 30. Level Grid => 12:34 Course
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this._targetPosition = targetPosition;
    }
}
