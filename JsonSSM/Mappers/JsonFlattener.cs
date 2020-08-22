using System;
using System.Collections.Generic;
using System.Linq;

using Amazon.SimpleSystemsManagement;

using JsonSSM.Models.Data;

using Newtonsoft.Json.Linq;

namespace JsonSSM.Mappers
{
    public class JsonFlattener
    {
        private readonly string META = "META", DATA = "DATA";
        private List<DataContainer> List;
        private MetaData Meta;

        public DataList Flatten(JToken json)
        {
            List = new List<DataContainer>();
            Meta = ResolveMetadata(json);
            if (Meta != default)
            {
                json = json.SelectToken(DATA);
            }
            ResolveObjectsFromToken(json, "");
            var data = new DataList(Meta, List);

            return data;
        }

        private MetaData ResolveMetadata(JToken json)
        {
            var paths = json.Children().Select(j => j.Path);
            var hasMeta = json.Children().Any(j => j.Path == META);
            var hasData = json.Children().Any(j => j.Path == DATA);

            if (hasMeta ^ hasData)
            {
                throw new Exception("The JSON document must have both META and DATA sections or neither");
            }

            if (!hasMeta)
            {
                return null;
            }

            var Meta = json.SelectToken(META).ToObject<MetaData>();

            return Meta;
        }

        /// <summary>
        /// https://stackoverflow.com/questions/32782937/generically-flatten-json-using-c-sharp
        /// </summary>
        /// <param name="token"></param>
        /// <param name="prefix"></param>
        private void ResolveObjectsFromToken(JToken token, string prefix)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    foreach (JProperty prop in token.Children<JProperty>())
                    {
                        ResolveObjectsFromToken(prop.Value, Join(prefix, prop.Name));
                    }
                    break;

                case JTokenType.Array:

                    if (token.Children().All(c => c.Type == JTokenType.String))
                    {
                        var value = string.Join(", ", token.Children());
                        AddToList(value, prefix, ParameterType.StringList);
                    }
                    else
                    {
                        int index = 0;
                        foreach (JToken value in token.Children())
                        {
                            ResolveObjectsFromToken(value, Join(prefix, index.ToString()));
                            index++;
                        }
                    }
                    break;

                default:
                    var type = Meta?.ShouldEncrypt(prefix) ?? false ? ParameterType.SecureString : ParameterType.String;
                    AddToList(((JValue)token).Value, prefix, type);
                    break;
            }
        }

        private void AddToList(object value, string prefix, ParameterType type)
        {
            var stringItem = new DataContainer(prefix, value);
            stringItem.SetParameterType(type);
            List.Add(stringItem);
        }

        private string Join(string prefix, string name)
        {
            return string.IsNullOrEmpty(prefix) ? name : prefix + "/" + name;
        }
    }
}