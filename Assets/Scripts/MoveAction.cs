using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private int maxMoveDistance = 4;

    private Vector3 _targetPosition;
    private Unit unit;

    private float _moveSpeed = 4f;
    private float _rotateSpeed = 10f;
    private float _stopLimit = .1f;

    private void Awake() 
    {
        unit = GetComponent<Unit>();
        _targetPosition = transform.position;
    }

    private void Update() {
        if(Vector3.Distance(_targetPosition, transform.position) > _stopLimit)
        {
            Vector3 movePosition = (_targetPosition - transform.position).normalized;
            transform.position += movePosition * Time.deltaTime * _moveSpeed;
            transform.forward = Vector3.Lerp(transform.forward, movePosition, Time.deltaTime * _rotateSpeed);
            _animator.SetBool("IsWalking", true);
        }
        else
            _animator.SetBool("IsWalking", false);
    }

    public void Move(GridPosition gridPosition)
    {
        this._targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for(int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;

                if(unitGridPosition == testGridPosition)
                    continue;

                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    continue;

                validGridPositions.Add(testGridPosition);
            }
        }

        return validGridPositions;
    }
}
