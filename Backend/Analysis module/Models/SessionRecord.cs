﻿using Backend.Models;

namespace Backend.Analysis_module.Models
{
    public class SessionRecord
    {
        public int SessionRecordID { get; set; }
        public AnalysisResult AnalysisResult { get; set; }
        public Session Session { get; set; }
        public GameplayData GameplayData { get; set; }
    }
}