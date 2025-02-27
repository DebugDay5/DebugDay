using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryObject : MonoBehaviour
{
    // �̱��� �ν��Ͻ��� ������ ���� ����
    private static DontDestoryObject instance;

    private void Awake()
    {
        // �̹� �ν��Ͻ��� �����ϴ��� Ȯ��
        if (instance != null && instance != this)
        {
            // �̹� �����ϸ� �� �ν��Ͻ��� ����
            Destroy(gameObject);
            return;
        }

        // ������ �� �ν��Ͻ��� ����
        instance = this;

        // ���� ������ �ִ� ������Ʈ���
        if (transform.parent != null && transform.root != null)
        {
            // �ֻ��� �θ� DontDestroy�� ����
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        // ���� �ֻ��� ������Ʈ�� 
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

}
