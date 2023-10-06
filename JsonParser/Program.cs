using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;


string swaggerJson;
JsonDocument document;
var pathFrom = Path.GetFullPath("D:\\Work\\swagger\\swagger.json");
var pathTo = Path.GetFullPath("D:\\Work\\swagger\\swaggerGet.json");
try
{
	swaggerJson = File.ReadAllText(pathFrom);
	document = JsonDocument.Parse(swaggerJson);
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
	throw;
}

JsonElement paths = document.RootElement.GetProperty("paths");

var myDeserializedPaths = JsonConvert.DeserializeObject<Dictionary<string,PathItemObject>>(paths.GetRawText());
Dictionary<string, PathItemObject> necessaryPaths = new();
foreach (var item in myDeserializedPaths)
{
    if (item.Value.get is not null)
    {
        necessaryPaths.Add(item.Key,new PathItemObject {
            get = item.Value.get,
            description = item.Value.description,
            head = item.Value.head,
            options = item.Value.options,
            parameters = item.Value.parameters,
            servers = item.Value.servers,
            trace = item.Value.trace,
            summary = item.Value.summary
        });
    }
}
var jsonStringWithnecessaryPaths = JsonConvert.SerializeObject(necessaryPaths,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

var arr = JsonConvert.DeserializeObject(swaggerJson);

if ((arr as JObject).ContainsKey("paths"))
{
    ((JObject)arr)["paths"] = JObject.Parse(jsonStringWithnecessaryPaths);
};

var end = JsonConvert.SerializeObject(arr);

if (File.Exists(pathTo))
{
    File.WriteAllText(pathTo, end);
}
else
{
    File.Create(pathTo);
    File.WriteAllText(pathTo, end);
}

