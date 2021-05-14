using Newtonsoft.Json;

namespace ESR.Files
{
    public class ESRFileHeader
    {
        public const int HearderSize = 64 * 1024; // 64KB

        public const string ESRFileKeyword = "ESR";

        public int MajorVersion = 0;

        public int MinorVersion = 1;

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
            });
        }

        public static ESRFileHeader FromJsonString(string json)
        {
            ESRFileHeader fileHeader = null;

            fileHeader = JsonConvert.DeserializeObject<ESRFileHeader>(json, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
            });

            return fileHeader;
        }
    }
}
