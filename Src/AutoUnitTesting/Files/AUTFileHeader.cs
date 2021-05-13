using Newtonsoft.Json;

namespace AUT.Files
{
    public class AUTFileHeader
    {
        public const int HearderSize = 64 * 1024; // 64KB

        public const string AUTFileKeyword = "AUT";

        public int MajorVersion = 0;

        public int MinorVersion = 1;

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
            });
        }

        public static AUTFileHeader FromJsonString(string json)
        {
            AUTFileHeader fileHeader = null;

            fileHeader = JsonConvert.DeserializeObject<AUTFileHeader>(json, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
            });

            return fileHeader;
        }
    }
}
