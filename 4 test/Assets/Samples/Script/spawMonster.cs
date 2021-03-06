using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawMonster : MonoBehaviour
{
    public Transform tras = null;
    //생성할 몬스터 오브젝트
    public GameObject monsterSpawner = null;

    //생성활 몬스터들 담아놓기
    public List<GameObject> monsters = new List<GameObject>();

    //생성할 몬스터 최대수
    public int spawnMaxCnt = 50;

    //생성할 몬스터 랜덤 좌표 (x,z)위치
    float rndPos = 100f; 

    void Spawn()
    {
        //몬스터 수가 생성할 몬스터 최대수 보다 크면 돌아가~
        if(monsters.Count > spawnMaxCnt)
        {
            return;
        }

        //생성할 위치를 지정한다. 초기 높이만 1000 나머지 .x,z는 랜덤 
        Vector3 vecSpawn = new Vector3(tras.position.x,tras.position.y, tras.position.z);

        //생성할 임시 높이에서 아래방향으로 Raycast를 통해 지형까지 높이 구하기
        Ray ray = new Ray(vecSpawn, Vector3.down);

        //Raycast 정보 가져오기
        RaycastHit raycastHit = new RaycastHit();
        if(Physics.Raycast(ray, out raycastHit, Mathf.Infinity) == true)
        {
            //Raycast 높이를 y값으로 재설정
            vecSpawn.y = raycastHit.point.y;
        }

        //생성할 새로운 몬스터를 Instantiate로 clone을 만든다.
        GameObject newMonster = Instantiate(monsterSpawner, vecSpawn, Quaternion.identity);

        //몬스터 목록에 새로운 몬스터를 추가
        monsters.Add(newMonster);

        GameManager.instance.everyMonsters.Add(newMonster);
        
        UIManager.Instance.SetText(GameManager.instance.everyMonsters.Count.ToString());
        
    }

    private void Start()
    {
        //반복적으로 몬스터를 만든다 InvokeRepeating
        InvokeRepeating("Spawn", 3f, 10f);
    }
}
