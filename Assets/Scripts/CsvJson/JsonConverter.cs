using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

[SerializeField]
public class ListWrapper<T>
{
    public List<T> values;
}

public static class JsonConverter 
{
    // C:\Users\[user name]\AppData\LocalLow\[company name]\[product name]
    static string savePath = Application.persistentDataPath;

    // 원본 타입의 리스트를 변환하는 메서드 (리플렉션 사용)
    public static string ConvertOriginalListToJson(object dataArray, Type elementType)
    {
        // dataArray는 현재 List<T>
        // 1. 적절한 ListWrapper<tyoe> 타입 생성
        Type wrapperType = typeof(ListWrapper<>).MakeGenericType(elementType);

        // 2. 래퍼 인스턴스 생성
        object wrapper = Activator.CreateInstance(wrapperType);

        // 3. values 필드 가져오기
        FieldInfo valuesField = wrapperType.GetField("values");

        // 4. dataArray를 values 필드에 할당
        // dataArray는 object타입이지만 실제로는 List<T> (매개변수로 List<T>를 넘겼기 때문)
        valuesField.SetValue(wrapper, dataArray);

        // 5. JsonUtility로 직렬화
        return JsonUtility.ToJson(wrapper);
    }

    public static void SaveJsonToFile(string json, string saveFileName)
    {
        try
        {
            string path = Path.Combine(savePath, saveFileName);
            File.WriteAllText(path, json);
            Debug.Log($"{saveFileName}이 저장되었습니다. 경로: {path}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"파일 저장 중 오류 발생: {ex.Message}");
        }
    }

    /*
    // 오브젝트 타입 - Json
    public static string ConvertObjectTypeToJson( List<object> dataArray , Type arrayType) 
    {
        // dataArray을 List<T>로 바꿔야함, 타입은 arrayType
        //IList list = dataArray.Cast<arrayType>().ToList()

        // 인덱스로 액세스 할 수 있는 개체 컬렉션인지? 
        if (dataArray is IList) 
        {
            // array Type으로 ListWrapper<> 인스턴스화
            object wrapper = Activator.CreateInstance(typeof(ListWrapper<>).MakeGenericType(arrayType));

            // 객제의 List<>의 변수(values)를 가져옴
            FieldInfo listProperty = wrapper.GetType().GetField("values");

            return JsonUtility.ToJson(wrapper);

        }
        return "";
    }

    */

}
