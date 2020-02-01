using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EnumWriter : MonoBehaviour
{
    private const string m_extension = ".cs";
    private const string m_sceneFileName = "Scenes";

    [MenuItem("SimpleSceneSwitch/CreateSceneEnum")]
    public static void CreateScenesFile()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        string[] scenes = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)
            scenes[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));

        WriteToEnum(m_sceneFileName, scenes);
    }

    public static string WriteToEnum<T>(string name, ICollection<T> data, string path = "Assets/SimpleSceneSwitch/GeneratedEnums/")
    {

        if (data != null && data.Count > 0)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (StreamWriter writer = File.CreateText(path + name + m_extension))
            {
                writer.WriteLine("public enum " + name + " \n{");

                int i = 0;
                foreach (var line in data)
                {
                    string lineRep = line.ToString().Replace(" ", string.Empty);
                    if (!string.IsNullOrEmpty(lineRep))
                    {
                        writer.WriteLine(string.Format("\t{0} = {1},", lineRep, i));
                        i++;
                    }
                }

                writer.WriteLine("\n}");
                writer.WriteLine("\n// This is an automated file, don't edit this file.");
            }


            AssetDatabase.ImportAsset(path + name + m_extension);
            Object obj = AssetDatabase.LoadAssetAtPath(path + name + m_extension, typeof(Object));
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
        }
        else
        {
            Debug.LogWarning(name + " Has no names");
        }

        return path;
    }
}

