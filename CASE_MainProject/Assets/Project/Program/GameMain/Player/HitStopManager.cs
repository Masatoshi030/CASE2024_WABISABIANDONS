using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class HitStopManager : MonoBehaviour
{
    //�V���O���g���C���X�^���X
    public static HitStopManager instance; // �C���X�^���X�̒�`

    private void Awake()
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

    public async void HitStopEffect(float _timeScale, float _stopTime)
    {
        // �Q�[�����~
        Time.timeScale = _timeScale;

        // �q�b�g�X�g�b�v�̒��������ҋ@
        await Task.Delay((int)(_stopTime * 1000));

        // �ҋ@���I�������Q�[�����ĊJ
        Time.timeScale = 1f;
    }
}
