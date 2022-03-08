using System.Collections.Generic;

namespace JsonGenerator.Helper
{
    public class GeneratorConfig
    {
        public string JsonSourceUrl { get; set; }
        public string JsonFilePath { get; set; }
        
        public List<string> InvalidApiPaths { get; set; }
        
        public List<string> InvalidApiDefinitions { get; set; }
    }
}