using MediaCenterCore.Models;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MediaCenterCore.Shared
{
    public class StaffC
    {

        public IEnumerable<string> Actors { get; }

        public IEnumerable<string> Directors { get; }

        public StaffC(string json)
        {
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, IEnumerable<string>>>(json);
            this.Actors = jsonObject["Actors"];
            this.Directors = jsonObject["Directors"];
        }
        public StaffC(IEnumerable<string> Actors, IEnumerable<string> Directors)
        {
            this.Actors = Actors;
            this.Directors = Directors;
        }

        public string GetJsonString()
        {
            var jsonObject = new Dictionary<string, IEnumerable<string>>
            {
              { "Actors", this.Actors ?? new List<string>()  },
              { "Directors", this.Directors ?? new List<string>()  }
             };
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize(jsonObject, options);
            return jsonString;
        }
        public string GetActorsAsString()
        {
            return string.Join(", ", this.Actors);
        }
        public string GetDirectorsAsString()
        {
            return string.Join(", ", this.Actors);
        }
    }
}
