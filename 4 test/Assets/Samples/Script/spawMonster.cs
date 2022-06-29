using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawMonster : MonoBehaviour
{
    public Transform tras = null;
    //������ ���� ������Ʈ
    public GameObject monsterSpawner = null;

    //����Ȱ ���͵� ��Ƴ���
    public List<GameObject> monsters = new List<GameObject>();

    //������ ���� �ִ��
    public int spawnMaxCnt = 50;

    //������ ���� ���� ��ǥ (x,z)��ġ
    float rndPos = 100f; 

    void Spawn()
    {
        //���� ���� ������ ���� �ִ�� ���� ũ�� ���ư�~
        if(monsters.Count > spawnMaxCnt)
        {
            return;
        }

        //������ ��ġ�� �����Ѵ�. �ʱ� ���̸� 1000 ������ .x,z�� ���� 
        Vector3 vecSpawn = new Vector3(tras.position.x,tras.position.y, tras.position.z);

        //������ �ӽ� ���̿��� �Ʒ��������� Raycast�� ���� �������� ���� ���ϱ�
        Ray ray = new Ray(vecSpawn, Vector3.down);

        //Raycast ���� ��������
        RaycastHit raycastHit = new RaycastHit();
        if(Physics.Raycast(ray, out raycastHit, Mathf.Infinity) == true)
        {
            //Raycast ���̸� y������ �缳��
            vecSpawn.y = raycastHit.point.y;
        }

        //������ ���ο� ���͸� Instantiate�� clone�� �����.
        GameObject newMonster = Instantiate(monsterSpawner, vecSpawn, Quaternion.identity);

        //���� ��Ͽ� ���ο� ���͸� �߰�
        monsters.Add(newMonster);

        GameManager.instance.everyMonsters.Add(newMonster);
        
        UIManager.Instance.SetText(GameManager.instance.everyMonsters.Count.ToString());
        
    }

    private void Start()
    {
        //�ݺ������� ���͸� ����� InvokeRepeating
        InvokeRepeating("Spawn", 3f, 10f);
    }
}