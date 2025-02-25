using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ConverterTestManager : MonoBehaviour
{
    [Header("===클래스 이름을 작성해주세요===")]
    public string[] className;
    public Dictionary<string, string> jsonResults = new Dictionary<string, string>();

    public void CsvConverByName() 
    {
        Debug.Log(" 테스트 메서드 입니다");

        for (int i = 0; i < className.Length; i++)
        {
            // 여기서 type은 ? 클래스라고 생각하면 편함 
            // string에 맞는 타입 생성 
            Type type = Type.GetType(className[i]); 

            if (type == null)
            {
                Debug.LogError($"해당 클래스({className[i]})를 찾을 수 없습니다.");
                continue;
            }

            if (!typeof(ICsvParsable).IsAssignableFrom(type))
            {
                Debug.LogError($"클래스({className[i]})는 ICsvParsable을 구현해야 합니다.");
                continue;
            }

            // CsvConverter<> 클래스의 타입을
            // MakeGeneritType : type으로 제네릭 지정
            // convertype : 즉 CsvConverter<클래스명>이 된다
            Type converterType = typeof(CsvConverter<>).MakeGenericType(type);

            // CsvConverter를 인스턴스화 
            // 매개변수는 className[i]
            object converterInstance = Activator.CreateInstance(converterType, className[i]);

            // CsvConverter<>의 GetDataArray() 메서드 가져오기 
            MethodInfo method = converterType.GetMethod("GetDataArray");

            if (method != null)
            {
                // GetDataArray 메서드 Invoke
                // return 값은 List<T>이지만 object타입으로 박싱(boxing) 일어남 
                // 원본 데이터 배열 가져오기 (List<T> 타입)
                // 컴파일 타임에는 object, 런타임때는 List<T>
                object dataArray = method.Invoke(converterInstance, null);

                // 원본 타입을 유지한 채 JSON으로 변환
                string json = JsonConverter.ConvertOriginalListToJson(dataArray, type);

                // 결과 저장
                jsonResults[className[i]] = json;

                // 파일로 저장
                JsonConverter.SaveJsonToFile(json, className[i]);

                Debug.Log($"{className[i]} 데이터를 성공적으로 변환했습니다.");
            }

            // Json으로 Convert
            //string json = JsonConverter.ConvertObjectTypeToJson(list, type);

            // 저장
            //JsonConverter.SaveJsonToFile(json, className[i]);




        }
    }
}
