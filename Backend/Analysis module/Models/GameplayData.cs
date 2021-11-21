using System.Text.Json.Serialization;

namespace Backend.Analysis_module.Models
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
    }
}