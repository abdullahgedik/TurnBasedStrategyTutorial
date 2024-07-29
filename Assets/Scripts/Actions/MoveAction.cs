using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private Animator _animator;
    [SerializeField] private int maxMoveDistance = 4;

    private Vector3 _targetPosition;

    private float _moveSpeed = 4f;
    private float _rotateSpeed = 10f;
    private float _stopLimit = .1f;

    protected override void Awake() 
    {
        base.Awake();
        _targetPosition = transform.position;
    }

    private void Update() 
    {
        if(!isActive)
            return;

        Vector3 movePosition = (_targetPosition - transform.position).normalized;

        if(Vector3.Distance(_targetPosition, transform.position) > _stopLimit)
        {
            transform.position += movePosition * Time.deltaTime * _moveSpeed;
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
            isActive = false;
        }

        transform.forward = Vector3.Lerp(transform.forward, movePosition, Time.deltaTime * _rotateSpeed);
    }

    public void Move(GridPosition gridPosition)
    {
        this._targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive =  true;
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
