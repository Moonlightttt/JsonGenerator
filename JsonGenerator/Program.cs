// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using JsonGenerator.Helper;
using Microsoft.Extensions.Configuration;

string currentDirectory = Path.GetDirectoryName(typeof(GeneratorHelper).Assembly.Location);
var configuration = new ConfigurationBuilder().SetBasePath(currentDirectory)
    .AddJsonFile("appsettings.json").Build();
var cleanerConfig = configuration.GetSection("CleanerConfig").Get<List<GeneratorConfig>>();
using (var cleaner = new GeneratorHelper(cleanerConfig))
{
    await cleaner.ClearAsync();
}