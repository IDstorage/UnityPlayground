using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTest : MonoBehaviour
{
    private enum IKDirection
    {
        Left,
        Right
    }

    [SerializeField]
    private Animator anim;

    [SerializeField, Range(0F, 1F)]
    private float footHeightOffset;

    [SerializeField]
    private Transform lookTarget;

    [SerializeField]
    private Transform leftHandGrepPoint, rightHandGrepPoint;


    private void OnAnimatorIK(int layerIndex)
    {
        if (!anim) return;

        SetFootStep(IKDirection.Left, 1F);
        SetFootStep(IKDirection.Right, 1F);

        SetHandGrep(IKDirection.Left, 1F);
        SetHandGrep(IKDirection.Right, 1F);

        SetSight(1F);

        void SetFootStep(IKDirection dir, float weight)
        {
            AvatarIKGoal target = dir == IKDirection.Left ? AvatarIKGoal.LeftFoot : AvatarIKGoal.RightFoot;

            anim.SetIKPositionWeight(target, weight);
            anim.SetIKRotationWeight(target, weight);

            Ray leftFootRay = new Ray(anim.GetIKPosition(target) + transform.up, -transform.up);
            if (Physics.Raycast(leftFootRay, out var hitInfo))
            {
                anim.SetIKPosition(target, hitInfo.point + transform.up * footHeightOffset);
            }
        }

        void SetHandGrep(IKDirection dir, float weight)
        {
            AvatarIKGoal target = dir == IKDirection.Left ? AvatarIKGoal.LeftHand : AvatarIKGoal.RightHand;
            Transform point = dir == IKDirection.Left ? leftHandGrepPoint : rightHandGrepPoint;

            if (!point) return;

            anim.SetIKPositionWeight(target, weight);
            anim.SetIKPosition(target, point.position);
        }

        void SetSight(float weight)
        {
            if (!lookTarget) return;

            anim.SetLookAtWeight(weight);
            anim.SetLookAtPosition(lookTarget.position);
        }
    }
}
