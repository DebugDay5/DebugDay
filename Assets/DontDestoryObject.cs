using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryObject : MonoBehaviour
{
    // 싱글톤 인스턴스를 저장할 정적 변수
    private static DontDestoryObject instance;

    private void Awake()
    {
        // 이미 인스턴스가 존재하는지 확인
        if (instance != null && instance != this)
        {
            // 이미 존재하면 새 인스턴스를 제거
            Destroy(gameObject);
            return;
        }

        // 없으면 이 인스턴스를 설정
        instance = this;

        // 내가 하위에 있는 오브젝트라면
        if (transform.parent != null && transform.root != null)
        {
            // 최상위 부모를 DontDestroy로 설정
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        // 내가 최상위 오브젝트면 
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

}
