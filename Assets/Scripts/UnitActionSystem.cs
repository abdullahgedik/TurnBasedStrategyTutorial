using System;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set;}

    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private LayerMask _unitLayer;

    public event EventHandler OnSelectedUnitChanged;

    private bool isBusy;

    private void Awake() 
    {
        if(Instance != null)
        {
            Debug.LogError("There is more than one UnitActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Update() 
    {
        if(isBusy)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            if(TryHandleUnitSelection()) return;

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if(_selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            {
                SetBusy();
                _selectedUnit.GetMoveAction().Move(mouseGridPosition);
            }  
        }

        if(Input.GetMouseButtonDown(1))
        {
            SetBusy();
            _selectedUnit.GetSpinAction().Spin(ClearBusy);
        }
    }

    private void SetBusy()
    {
        isBusy = true;
    }

    private void ClearBusy()
    {
        isBusy = false;
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _unitLayer))
        {
            if(raycastHit.transform.TryGetComponent(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return _selectedUnit;
    }
}
