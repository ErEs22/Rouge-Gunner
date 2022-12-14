using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    [Header("Player Manager")]
    [SerializeField] PlayerManager playerManager;
    [Header("Aniamtor Controllers")]
    [SerializeField] Animator modelAnim;
    [SerializeField] RuntimeAnimatorController pistolAtr;
    [SerializeField] RuntimeAnimatorController rifleAtr;

    int vertical;
    int horizontal;
    Animator anim;



    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Initialize()
    {
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }



    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
    {
        #region Vertical
        float v;
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            v = 0.5f;
        }
        else if (verticalMovement >= 0.55f)
        {
            v = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            v = -1;
        }
        else
        {
            v = 0;
        }
        #endregion

        #region Horizontal
        float h = 0;
        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            h = 0.5f;
        }
        else if (horizontalMovement >= 0.55f)
        {
            h = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            h = -1;
        }
        else
        {
            h = 0;
        }
        #endregion
        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        modelAnim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        modelAnim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);

    }

    /// <summary>
    /// ?????????????????? ?????????????????????????????????????????????????????????isInteracting???true
    /// </summary>
    /// <param name="targetAnim"></param>
    /// <param name="isInteracting"></param>
    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }

}
