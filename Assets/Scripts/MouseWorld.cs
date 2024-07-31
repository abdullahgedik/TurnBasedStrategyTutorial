using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    public static MouseWorld instance;
    [SerializeField] private LayerMask hitLayer;

    private void Awake() 
    {
        instance = this;
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.hitLayer);
        return raycastHit.point;
    }
}
