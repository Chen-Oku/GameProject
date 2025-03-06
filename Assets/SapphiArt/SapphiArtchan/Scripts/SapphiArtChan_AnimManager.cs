
using UnityEngine;
using System.Collections;

public class SapphiArtChan_AnimManager : MonoBehaviour {

    private Animator _SapphiArtChanAnimator;                //Character Animation
    internal string _SapphiArtChanAnimation = null;         //Character Animation Name
    private AnimationManagerUI _AnimationManagerUI;         //Character Animation UI Connection
    private string _SapphiArtChanLastAnimation = null;      //Character Last Animation

    private SkinnedMeshRenderer _SapphiArtChanRenderer_Face;        //Character Skin Mesh Renderer for Face
    private SkinnedMeshRenderer _SapphiArtChanRenderer_Brow;        //Character Skin Mesh Renderer for Eyebrows
    private SkinnedMeshRenderer _SapphiArtChanRenderer_BottomTeeth;        //Character Skin Mesh Renderer for Bottom Teeth
    private SkinnedMeshRenderer _SapphiArtChanRenderer_Tongue;        //Character Tongue Skinned Mesh Renderer
    private SkinnedMeshRenderer _SapphiArtChanRenderer_TopTeeth;      //Character Top Teeth Skinned Mesh Renderer

    //BlendShapeValues
    

    void Start()
    {
        _SapphiArtChanAnimator = this.gameObject.GetComponent<Animator>();
        Transform[] SapphiArtchanChildren = GetComponentsInChildren<Transform>();

        
    }


    //void GetAnimation()
    //{
    //    //Record Last Animation
    //    _SapphiArtChanLastAnimation = _SapphiArtChanAnimation;

    //    if (_SapphiArtChanAnimation == null)
    //        _SapphiArtChanAnimation = "idle";

    //    else
    //    {
    //        //Set Animation Parameter
    //        _SapphiArtChanAnimation = _AnimationManagerUI._Animation;
    //        //_SapphiArtChanAnimation = "hit01";
    //    }
    //}

    void SetAllAnimationFlagsToFalse()
    {
        _SapphiArtChanAnimator.SetBool("param_idletowalk", false);
        _SapphiArtChanAnimator.SetBool("param_idletorunning", false);
        _SapphiArtChanAnimator.SetBool("param_idletojump", false);
        _SapphiArtChanAnimator.SetBool("param_idletowinpose", false);
        _SapphiArtChanAnimator.SetBool("param_idletoko_big", false);
        _SapphiArtChanAnimator.SetBool("param_idletodamage", false);
        _SapphiArtChanAnimator.SetBool("param_idletohit01", false);
        _SapphiArtChanAnimator.SetBool("param_idletohit02", false);
        _SapphiArtChanAnimator.SetBool("param_idletohit03", false);
    }


    void SetAnimation()
    {
        SetAllAnimationFlagsToFalse();

        //IDLE
        if (_SapphiArtChanAnimation == "idle")
        {
            _SapphiArtChanAnimator.SetBool("param_toidle", true);
        }

        //WALK
        else if (_SapphiArtChanAnimation == "walk")
        {
            _SapphiArtChanAnimator.SetBool("param_idletowalk", true);
        }

        //RUN
        else if (_SapphiArtChanAnimation == "running")
        {
            _SapphiArtChanAnimator.SetBool("param_idletorunning", true);
        }

        //JUMP
        else if (_SapphiArtChanAnimation == "jump")
        {
            _SapphiArtChanAnimator.SetBool("param_idletojump", true);
        }

        //WIN POSE
        else if (_SapphiArtChanAnimation == "winpose")
        {
            _SapphiArtChanAnimator.SetBool("param_idletowinpose", true);
        }

        //KO
        else if (_SapphiArtChanAnimation == "ko_big")
        {
            _SapphiArtChanAnimator.SetBool("param_idletoko_big", true);
        }

        //DAMAGE
        else if (_SapphiArtChanAnimation == "damage")
        {
            _SapphiArtChanAnimator.SetBool("param_idletodamage", true);
        }

        //HIT 1
        else if (_SapphiArtChanAnimation == "hit01")
        {
            _SapphiArtChanAnimator.SetBool("param_idletohit01", true);
        }

        //HIT 2
        else if (_SapphiArtChanAnimation == "hit02")
        {
            _SapphiArtChanAnimator.SetBool("param_idletohit02", true);
        }

        //HIT 3
        else if (_SapphiArtChanAnimation == "hit03")
        {
            _SapphiArtChanAnimator.SetBool("param_idletohit03", true);
        }
    }

    void ReturnToIdle()
    {
        if (_SapphiArtChanAnimator.GetCurrentAnimatorStateInfo(0).IsName(_SapphiArtChanAnimation))
        {
            if (
                _SapphiArtChanAnimation != "walk" &&
                _SapphiArtChanAnimation != "running" &&
                _SapphiArtChanAnimation != "ko_big" &&
                _SapphiArtChanAnimation != "winpose"
                )
            {
                SetAllAnimationFlagsToFalse();
                _SapphiArtChanAnimator.SetBool("param_toidle", true);
            }
        }
    }


    void Update ()
    {
        //Get Animation from UI
        //GetAnimation();
        
        ////Set New Animation
        //if (_SapphiArtChanLastAnimation != _SapphiArtChanAnimation)
        //    SetAnimation();
        //else
        //{
        //    ReturnToIdle();
        //}

    }
}