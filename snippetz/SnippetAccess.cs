using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace snippetz
{
    internal class SnippetAccess
    {
        string path;
        public SnippetAccess() {
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "snips.json");
        }

        public ActionContainer RegisterCommand(string command)
        {
            ActionContainer cmd = new ActionContainer(command);
            string json = JsonSerializer.Serialize(cmd);
            WriteToFile(json);
            return cmd;
        }

        public void RunStoredSnippet(string snippet)
        {
            List<ActionContainer> actions = new List<ActionContainer>();
            string json = ReadFromFile();
            actions = JsonSerializer.Deserialize<List<ActionContainer>>(json);
            actions = actions.Where(a => a.command.Contains(snippet)).ToList();

            if (actions.Count == 1 && actions.ElementAt(0).command == snippet)
            {
                actions.ElementAt(0).Execute();
            }
            else if (actions.Count < 1)
            {
                Console.WriteLine("No snippetz matching '" + snippet + "'");
            }
            else
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    Console.WriteLine((i + 1) + ": " + actions[i].command);
                }
                Console.WriteLine("Which command to run?");
                Console.WriteLine("Press X to cancel");
                string pick = Console.ReadLine();
                if(pick.ToLower() != "x")
                {
                    int num = Convert.ToInt32(pick);
                    actions[num - 1].Execute();
                }
            }
        }

        void WriteToFile(string jsonStringToAdd)
        {

            // Parse input JSON string and add it to the list
            JsonDocument doc = JsonDocument.Parse(jsonStringToAdd);

            string json = ReadFromFile();
            // Deserialize JSON array into a list
            List<JsonElement> jsonArray = JsonSerializer.Deserialize<List<JsonElement>>(json);
            jsonArray.Add(doc.RootElement.Clone());

            // Serialize updated list back to JSON
            string updatedJson = JsonSerializer.Serialize(jsonArray);

            // Write updated JSON array back to the file
            File.WriteAllText(path, updatedJson);
        }

        string ReadFromFile()
        {
            // Create the file if it doesn't exist
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "[]");
            }

            // Read existing JSON array from file
            string json = File.ReadAllText(path);

            // Deserialize JSON array into a list
            List<JsonElement> jsonArray = JsonSerializer.Deserialize<List<JsonElement>>(json);

            return json;
        }
    }
}
