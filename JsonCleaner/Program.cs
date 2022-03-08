// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using JsonCleaner.Helper;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json").Build();
var cleanerConfig = configuration.GetSection("CleanerConfig").Get<List<CleanConfig>>();
using (var cleaner = new CleanTool(cleanerConfig))
{
    await cleaner.ClearAsync();
}