using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public interface ICsvParsable
{
    void Parse(string[] values);
}

[System.Serializable]
public class Stage : ICsvParsable
{
    [SerializeField] private int hp;
    [SerializeField] private string name;
    [SerializeField] private List<string> animal;

    public void Parse(string[] values)
    {
        hp = int.Parse(values[0]);                                      
        name = values[1];                                      
        
        animal = new List<string>();
        string[] temp = values[3].Split('-');
        for (int i = 0; i < temp.Length; i++) 
        {
            animal.Add(temp[i]);
        }
    }
}

public class CsvConverter<T>
    where T : class , ICsvParsable, new()
{
    // csv �Ľ� ���Խ� 
    private string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    private string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    // ���� �̸� 
    private string fileName;

    // ���������� ��Ƴ���
    List<T> dataArray;

    public CsvConverter(string fName) 
    {
        this.fileName = fName;

        Debug.Log(fileName);

        dataArray = new List<T>();

        InitCsv();
    }


    private void InitCsv() 
    {
        // Resouce���Ͽ� ����� �ؽ�Ʈ ���� �������� 
        TextAsset _textAsset = Resources.Load(fileName) as TextAsset;

        if (_textAsset == null)
        {
            Debug.LogError($"������ ã�� �� �����ϴ�: {fileName}");
            return;
        }
        else if (string.IsNullOrWhiteSpace(_textAsset.text))
        {
            Debug.LogError($"CSV ������ �������� �ʰų� ��� �ֽ��ϴ�: {fileName}");
            return;
        }

        try 
        {
            // �ະ�� �ڸ���
            string[] lines = Regex.Split(_textAsset.text, LINE_SPLIT_RE);

            // ù��° �� �ڸ��� 
            string[] header = Regex.Split(lines[0], SPLIT_RE);

            for (int i = 1; i < lines.Length; i++)
            {
                // �ܾ�� �ڸ��� 
                string[] values = Regex.Split(lines[i], SPLIT_RE);

                ProcessData(values);
            }

        }
        catch (Exception e) 
        {
            Debug.LogError($"CSV �Ľ� �� ���� �߻�: {e.Message}");
        }

        
    }

    private void ProcessData(string[] lines) 
    {
        T data = new T();
        data.Parse(lines);

        dataArray.Add(data);
    }

    public List<T> GetDataArray()
    {
        //Debug.Log("GetDataArray�޼��� ����");

        return dataArray;
    }


}
