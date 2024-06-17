using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class CameraManager : MonoBehaviour
{

    public static CameraManager instance;

    [SerializeField, Header("HitCamera")]
    GameObject hitCamera;

    private void Start()
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
    }

    public async void SetHitCamera(float _time)
    {
        //�J�����̗L��
        hitCamera.SetActive(true);

        // �q�b�g�X�g�b�v�̒��������ҋ@
        await Task.Delay((int)(_time * 1000));

        //�J�����̖���
        hitCamera.SetActive(false);
    }
}
