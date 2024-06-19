using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GoldValveController : MonoBehaviour
{

    [SerializeField, Header("�v���C���[�Ɍ������X�s�[�h")]
    float speed = 2.0f;

    float lerpValue = 0.0f;

    [SerializeField, Header("�擾�t���O")]
    bool bGetFlag = false;

    public Vector3 startPosition;
    private void Start()
    {
        startPosition = transform.position;
    }

    public void GetGoldValve()
    {
        bGetFlag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (bGetFlag)
        {
            //���`��Ԃ̌��݂̈ʒu���J�E���g
            lerpValue += Time.deltaTime * speed;

            //���݂̈ʒu�J�E���g������W���v�Z
            transform.position = Vector3.Lerp(startPosition, PlayerController.instance.transform.position, lerpValue);

            //�����v���C���[�ɓ���������폜
            if(lerpValue > 1.0f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
