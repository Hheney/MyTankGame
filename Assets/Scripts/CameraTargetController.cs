using UnityEngine;

public class CameraTargetController : MonoBehaviour
{
    [SerializeField] private Vector3 vOffset = new Vector3(0.0f, 10.0f, -15.0f);
    //[SerializeField] private float fSwitchAngleThreshold = 180.0f;

    private Transform tankTransform = null;
    private bool bRearView = false;

    public void f_SetTankTransform(Transform target)
    {
        tankTransform = target;
    }

    private void LateUpdate()
    {
        if (tankTransform == null) return;

        f_UpdateCamOffset();
        f_FollowTank();
    }

    private void f_UpdateCamOffset()
    {
        float yAngle = tankTransform.eulerAngles.y;

        //후방 판정: 앞을 기준으로 양쪽 90~270도 위치(원을 기준으로 중앙을 가로지르는 선, 회전 기준선 벗어나는 경우 전환 시킴)
        bool isBehind = (yAngle > 90f && yAngle < 270f);

        if (!bRearView && isBehind)
        {
            vOffset.z = Mathf.Abs(vOffset.z);
            bRearView = true;
        }
        else if (bRearView && !isBehind)
        {
            vOffset.z = -Mathf.Abs(vOffset.z);
            bRearView = false;
        }
    }

    private void f_FollowTank()
    {
        transform.position = tankTransform.position + vOffset;
    }
}