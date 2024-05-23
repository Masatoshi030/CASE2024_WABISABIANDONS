using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{

    public static CursorController instance;

    public enum ON_CURSOR_STATE
    {
        Idle,
        OffGround
    }

    [SerializeField, Header("�J�[�\�����"),
        Toolbar(typeof(ON_CURSOR_STATE), "OnCursorState")]
    public ON_CURSOR_STATE onCursorState = ON_CURSOR_STATE.Idle;

    Animator myCursorAnimator;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            // ���g���C���X�^���X�Ƃ���
            instance = this;
        }
        else
        {
            // �C���X�^���X�����ɑ��݂��Ă����玩�g����������
            Destroy(gameObject);
        }

        myCursorAnimator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Aim)
        {
            for (int i = 0; i < 3; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void ChangeCursorState(ON_CURSOR_STATE _state)
    {
        onCursorState = _state;

        switch (_state)
        {
            case ON_CURSOR_STATE.Idle:
                myCursorAnimator.SetBool("bOnGround", true);
                break;
            case ON_CURSOR_STATE.OffGround:
                myCursorAnimator.SetBool("bOnGround", false);
                break;
        }
    }
}
