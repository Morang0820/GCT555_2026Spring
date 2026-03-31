using UnityEngine;

// C:\Users\Morang\Documents\GitHub\GCT555_2026Spring\GCT555_Server

public class AnimationManager : MonoBehaviour
{
    public GameObject Avatar;
    public float animateThreshold = 1.0f;

    private Animator animator;
    private bool isClose;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Avatar == null || animator == null)
            return;

        Animator avatarAnim = Avatar.GetComponent<Animator>();
        if (avatarAnim == null) return;

        float distance = Vector3.Distance(transform.position, Avatar.transform.position);
        isClose = distance <= animateThreshold;
        animator.SetBool("IsClose", isClose);

        CheckAvatarGesture(avatarAnim);
    }

    void CheckAvatarGesture(Animator avatarAnim)
    {
        Transform headT = avatarAnim.GetBoneTransform(HumanBodyBones.Head);
        Transform leftHandT = avatarAnim.GetBoneTransform(HumanBodyBones.LeftHand);
        Transform rightHandT = avatarAnim.GetBoneTransform(HumanBodyBones.RightHand);

        if (headT == null) headT = avatarAnim.GetBoneTransform(HumanBodyBones.Neck);
        if (headT == null) headT = avatarAnim.GetBoneTransform(HumanBodyBones.Hips);

        if (headT == null || leftHandT == null || rightHandT == null) return;

        float headY = headT.position.y;
        float leftHandY = leftHandT.position.y;
        float rightHandY = rightHandT.position.y;
        Debug.Log($"[Gesture Check] Head: {headY:F2}, L-Hand: {leftHandY:F2}, R-Hand: {rightHandY:F2} | isClose: {isClose}");

        float shoulderLine = headY * 0.85f;

        bool isCheering = (leftHandY > headY && rightHandY > headY);
        bool isClapping = !isCheering && (leftHandY > shoulderLine && rightHandY > shoulderLine);
        bool isWaving = !isCheering && !isClapping && (leftHandY > shoulderLine || rightHandY > shoulderLine);

        animator.SetBool("IsCheering", isCheering);
        animator.SetBool("IsClapping", isClapping);
        animator.SetBool("IsWaving", isWaving);

        bool finalIsClose = isClose && !isCheering && !isClapping && !isWaving;
        animator.SetBool("IsClose", finalIsClose);
    }
}
