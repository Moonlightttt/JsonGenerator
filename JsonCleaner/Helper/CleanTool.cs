using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonCleaner.Helper
{
    public class CleanTool : IDisposable
    {
        private static readonly HttpClient _client = new HttpClient();
        private List<CleanConfig> _cleanerConfigs;

        public CleanTool(List<CleanConfig> cleanerConfigs)
        {
            _cleanerConfigs = cleanerConfigs;
        }

        /// <summary>
        /// 读取JSON文件
        /// </summary>
        /// <param name="path">JSON文件路径</param>
        /// <returns>JSON文件中的value值</returns>
        private JObject ReadFromJsonFile(string path)
        {
            if (!File.Exists(path)) throw new Exception("无效文件路径");

            using (System.IO.StreamReader file = System.IO.File.OpenText(path))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    return o;
                }
            }
        }

        /// <summary>
        /// 读取网络JSON文件
        /// </summary>
        /// <param name="url">JSON文件Url</param>
        /// <returns>JSON文件中的value值</returns>
        private async Task<JObject> ReadFromNetJsonFileAsync(string url)
        {
            var result = await _client.GetAsync(url);

            var content = await result.Content.ReadAsStringAsync();

            JObject o = JObject.Parse(content);

            return o;
        }

        public async Task ClearAsync()
        {
            foreach (var item in _cleanerConfigs)
            {
                JObject jsonObject;
                if (!string.IsNullOrWhiteSpace(item.JsonSourceUrl))
                {
                    jsonObject = await ReadFromNetJsonFileAsync(item.JsonSourceUrl);
                }
                else
                {
                    jsonObject = ReadFromJsonFile(item.JsonFilePath);
                }

                if (item.InvalidApiPaths != null && item.InvalidApiPaths.Count > 0)
                {
                    foreach (var invalidApiPath in item.InvalidApiPaths)
                    {
                        ((JObject)jsonObject["paths"]).Remove(invalidApiPath);
                    }
                }

                if (item.InvalidApiDefinitions != null && item.InvalidApiDefinitions.Count > 0)
                {
                    foreach (var invalidApiDefinition in item.InvalidApiDefinitions)
                    {
                        ((JObject)jsonObject["definitions"]).Remove(invalidApiDefinition);
                    }
                }

                string? convertString = Convert.ToString(jsonObject);

                File.WriteAllText(item.JsonFilePath, convertString);
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}