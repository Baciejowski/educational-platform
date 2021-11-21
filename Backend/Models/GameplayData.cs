using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class GameplayData
    {
        public int GameplayDataID { get; set; }
        public int Experience { get; set; }
        public int Money { get; set; }

        public int GameplayTime { get; set; }
        public float Light { get; set; }
        public float Vision { get; set; }
        public float Speed { get; set; }

        [JsonIgnore]
        public SessionRecord SessionRecord { get; set; }

        [ForeignKey("SessionRecord")]
        public int SessionRecordID { get; set; }
    }
}