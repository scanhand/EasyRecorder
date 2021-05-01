using Newtonsoft.Json;

namespace AMK.Files
{
    public class AMKFileHeader
    {
        public const int HearderSize = 64 * 1024; // 64KB

        public const string AMKFileKeyword = "AMK";

        public int MajorVersion = 0;

        public int MinorVersion = 1;

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
            });
        }

        public static AMKFileHeader FromJsonString(string json)
        {
            AMKFileHeader fileHeader = null;

            fileHeader = JsonConvert.DeserializeObject<AMKFileHeader>(json, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
            });

            return fileHeader;
        }
    }
}
