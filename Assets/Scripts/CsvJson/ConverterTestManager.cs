using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ConverterTestManager : MonoBehaviour
{
    [Header("===Ŭ���� �̸��� �ۼ����ּ���===")]
    public string[] className;
    public Dictionary<string, string> jsonResults = new Dictionary<string, string>();

    public void CsvConverByName() 
    {
        Debug.Log(" �׽�Ʈ �޼��� �Դϴ�");

        for (int i = 0; i < className.Length; i++)
        {
            // ���⼭ type�� ? Ŭ������� �����ϸ� ���� 
            // string�� �´� Ÿ�� ���� 
            Type type = Type.GetType(className[i]); 

            if (type == null)
            {
                Debug.LogError($"�ش� Ŭ����({className[i]})�� ã�� �� �����ϴ�.");
                continue;
            }

            if (!typeof(ICsvParsable).IsAssignableFrom(type))
            {
                Debug.LogError($"Ŭ����({className[i]})�� ICsvParsable�� �����ؾ� �մϴ�.");
                continue;
            }

            // CsvConverter<> Ŭ������ Ÿ����
            // MakeGeneritType : type���� ���׸� ����
            // convertype : �� CsvConverter<Ŭ������>�� �ȴ�
            Type converterType = typeof(CsvConverter<>).MakeGenericType(type);

            // CsvConverter�� �ν��Ͻ�ȭ 
            // �Ű������� className[i]
            object converterInstance = Activator.CreateInstance(converterType, className[i]);

            // CsvConverter<>�� GetDataArray() �޼��� �������� 
            MethodInfo method = converterType.GetMethod("GetDataArray");

            if (method != null)
            {
                // GetDataArray �޼��� Invoke
                // return ���� List<T>������ objectŸ������ �ڽ�(boxing) �Ͼ 
                // ���� ������ �迭 �������� (List<T> Ÿ��)
                // ������ Ÿ�ӿ��� object, ��Ÿ�Ӷ��� List<T>
                object dataArray = method.Invoke(converterInstance, null);

                // ���� Ÿ���� ������ ä JSON���� ��ȯ
                string json = JsonConverter.ConvertOriginalListToJson(dataArray, type);

                // ��� ����
                jsonResults[className[i]] = json;

                // ���Ϸ� ����
                JsonConverter.SaveJsonToFile(json, className[i]);

                Debug.Log($"{className[i]} �����͸� ���������� ��ȯ�߽��ϴ�.");
            }

            // Json���� Convert
            //string json = JsonConverter.ConvertObjectTypeToJson(list, type);

            // ����
            //JsonConverter.SaveJsonToFile(json, className[i]);




        }
    }
}
