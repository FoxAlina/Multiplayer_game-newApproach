using System.Xml.Serialization;

[XmlType("Data")]
//[XmlInclude(typeof(LocationMapData))]
[System.Serializable]
public abstract class Data
{
    public Data() { }
}
