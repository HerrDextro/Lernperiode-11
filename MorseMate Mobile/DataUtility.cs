using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MorseMate_Mobile
{
    public class DataUtility //this guys job is to handle storage, ill need it for keeping track of lvl progress and user settings
    {
        public LessonDataModelClass LoadLessonData(int levelNumber)
        {
            string path = $"C:\\Users\\Neo\\source\\repos\\MorseMate Mobile\\MorseMate Mobile\\Resources\\Raw\\OpposedGroups.json";
            var jsonString = File.ReadAllText(path);
            List<LessonDataModelClass> lessons = JsonSerializer.Deserialize<List<LessonDataModelClass>>(jsonString);
            var lessonData = lessons.FirstOrDefault(l => l.LevelNumber == levelNumber);

            return lessonData;
        }

    }
}
