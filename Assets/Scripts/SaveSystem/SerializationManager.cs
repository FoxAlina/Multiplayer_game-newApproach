using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SerializationManager 
{
    public static bool Save<T>(string saveName, T saveData)
    {
        //xml 
        XmlSerializer serializer = new XmlSerializer(typeof(T));

        string path = createPath(saveName);

        FileStream file = File.Create(path);

        // save data to file
        serializer.Serialize(file, saveData);

        file.Close();
        return true;
    }

    public static T Load<T>(string saveName) where T : Data, new()
    {
        string path = createPath(saveName);
        if (!File.Exists(path))
        {
            return new T();
        }

        //xml
        XmlSerializer serializer = new XmlSerializer(typeof(T));

        FileStream file = File.Open(path, FileMode.Open);

        try
        {
            T save = serializer.Deserialize(file) as T;
            file.Close();
            return save;
        }
        catch
        {
            Debug.LogErrorFormat("Failed to load file at {0}", path);
            file.Close();
            return new T();
        }
    }

    private static string createPath(string saveName)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }

        string path = Application.persistentDataPath + "/saves/" + saveName + ".xml";
        return path;
    }
}
