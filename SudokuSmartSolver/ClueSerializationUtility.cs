using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace SudokuPuzzleSolver
{
    public static class ClueSerializationUtility
    {
        public static List<CellData> DeserailizeClues(string filename, out bool existent)
        {
            string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string path = Path.Combine(dir, "clues");
            string file = filename + ".json";
            string fullname = Path.Combine(path,file);
            //Console.WriteLine(File.Exists(fullname));
            List<CellData> DataClueList = new List<CellData>();
            if (File.Exists(fullname))
            {
                string cluesText = File.ReadAllText(fullname);
                //might want to implement if the json file actually contains any objects to deserialize (i.e if someone somehow submits a valid json file that's for something else completely
                DataClueList = JsonConvert.DeserializeObject<List<CellData>>(cluesText);
                existent = true;
                return DataClueList;
            }
            existent = false;
            return null;
        }
    }
}
