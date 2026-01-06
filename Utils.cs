using System.IO;
using System.Xml.Serialization;

namespace VTSMemoryCompression
{
	public static class Utils
	{
		public static void Store<T>(string location, T obj)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			TextWriter sw = new StreamWriter(location);
			serializer.Serialize(sw, obj);
			sw.Close();
		}
	}
}
