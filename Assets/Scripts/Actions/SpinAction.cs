using UnityEngine;

public class SpinAction : BaseAction
{
    public delegate void SpinCompleteDelegate();

    SpinCompleteDelegate onSpinComplete;
    private float totalSpinAmount;

    private void Update() 
    {
        if(!isActive)
            return;

        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);  

        totalSpinAmount += spinAddAmount;
        if(totalSpinAmount >= 360f)
        {
            isActive = false;
            onSpinComplete();
        }
            
    }

    public void Spin(SpinCompleteDelegate onSpinComplete)
    {
        this.onSpinComplete = onSpinComplete;
        isActive = true;
        totalSpinAmount = 0f;
    }
}
