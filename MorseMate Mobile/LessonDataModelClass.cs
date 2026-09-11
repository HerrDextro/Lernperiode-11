using System;
using System.Collections.Generic;
using System.Text;

namespace MorseMate_Mobile
{
    public class LessonDataModelClass
    {
        public int LevelNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> Characters { get; set; }
    }
}
