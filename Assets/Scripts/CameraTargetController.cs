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

        //�Ĺ� ����: ���� �������� ���� 90~270�� ��ġ(���� �������� �߾��� ���������� ��, ȸ�� ���ؼ� ����� ��� ��ȯ ��Ŵ)
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