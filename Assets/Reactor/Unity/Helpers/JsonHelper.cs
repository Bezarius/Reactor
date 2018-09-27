using Newtonsoft.Json;

public static class JsonHelper  {

    public static T DeepClone<T>(this T obj)
    {
        var jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto
        };
        var json = JsonConvert.SerializeObject(obj, Formatting.Indented, jsonSettings);
        var result = JsonConvert.DeserializeObject<T>(json, jsonSettings);
        return result;
    }
}
