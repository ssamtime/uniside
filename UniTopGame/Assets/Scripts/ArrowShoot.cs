using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShoot : MonoBehaviour
{
    public float shootSpeed = 12.0f;    //ȭ�� �ӵ�
    public float shootDelay = 0.25f;    //�߻� ����
    public GameObject bowPrefab;        //Ȱ
    public GameObject arrowPrefab;      //ȭ��

    bool inAttack = false;  //���� ���� �Ǵ�
    GameObject bowObj;      //Ȱ

    // Start is called before the first frame update
    void Start()
    {
        // Ȱ�� �÷��̾� ��ġ�� ��ġ
        Vector3 pos = transform.position;
        bowObj = Instantiate(bowPrefab, pos, Quaternion.identity);
        bowObj.transform.SetParent(transform);  //�÷��̾� ��ü�� Ȱ ��ü�� �θ�� ����
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetButtonDown("Fire1")))
        {
            // ���� Ű �Է�
            Attack();
        }

        // Ȱ ȸ���� �켱���� ����
        float bowZ = -1;    //Ȱ ��ü�� Z�� �� (ĳ���ͺ��� �տ� ����)
        PlayerController plmv = GetComponent<PlayerController>();
        if(plmv.angleZ > 30 && plmv.angleZ <150)
        {
            // ������
            bowZ = 1;
        }
        // Ȱ ȸ��
        bowObj.transform.rotation = Quaternion.Euler(0, 0, plmv.angleZ);
        // Ȱ �켱����
        bowObj.transform.position = new Vector3(transform.position.x,transform.position.y, bowZ);
    }

    // ����
    public void Attack()
    {
        // ȭ���� ������ �ְ� ���� ���� ���°� �ƴ� ���
        if(ItemKeeper.hasArrows >0 &&inAttack ==false)
        {
            ItemKeeper.hasArrows -= 1;  //ȭ�� 1�� �Ҹ�
            inAttack = true;            //���� ���� ����

            // ȭ�� �߻�
            PlayerController playerCnt = GetComponent<PlayerController>();
            // ȸ���� ����� ����
            float angleZ = playerCnt.angleZ;
            // ȭ�� ������Ʈ ���� (ĳ���� ���� �������� ȸ��)
            Quaternion r = Quaternion.Euler(0, 0, angleZ);
            GameObject arrowObj = Instantiate(arrowPrefab, transform.position, r);

            // ȭ���� �߻��ϱ� ���� ���� ����
            float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
            float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;

            // ������ ������ �������� ȭ���� �߻�
            Rigidbody2D body = arrowObj.GetComponent<Rigidbody2D>();
            body.AddForce(v, ForceMode2D.Impulse);

            // ������ ����
            Invoke("StopAttack", shootDelay);

            // SE ���
            SoundManager.soundManager.SEPlay(SEType.Shoot);
        }
    }

    // ���� ����
    public void StopAttack()
    {
        inAttack = false;
    }
}
