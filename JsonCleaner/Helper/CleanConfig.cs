using System.Collections.Generic;

namespace JsonCleaner.Helper
{
    public class CleanConfig
    {
        public string JsonSourceUrl { get; set; }
        public string JsonFilePath { get; set; }
        
        public List<string> InvalidApiPaths { get; set; }
        
        public List<string> InvalidApiDefinitions { get; set; }
    }
}