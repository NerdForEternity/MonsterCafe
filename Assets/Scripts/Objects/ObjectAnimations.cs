using UnityEngine;

public class ObjectAnimations : MonoBehaviour
{
    public Unit parentUnit;

    public void AnimationDone()
    {
        parentUnit.animationDone = true;
    }
}
