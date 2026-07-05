namespace Railway.Platform.UnitTests.Helpers
{
    /// <summary>
    /// Loads JSON test data from files in the TestData folder hierarchy
    /// Automatically searches in TestData subdirectories for scalability
    /// </summary>
    public static class JsonGenerator
    {
        private static readonly string TestDataPath = Path.Combine(
            AppContext.BaseDirectory,
            "TestData"
        );

        /// <summary>
        /// Loads a JSON file from any TestData subdirectory
        /// Searches recursively to find the file
        /// Example: LoadData("email-null.json") finds TestData/Messaging/email-null.json
        /// </summary>
        public static JsonElement LoadData(string fileName)
        {
            var json = LoadJson(fileName);
            var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("data");
        }

        /// <summary>
        /// Loads a JSON file and returns it as a string
        /// Searches recursively in TestData directories
        /// </summary>
        public static string LoadJson(string fileName)
        {
            var filePath = FindJsonFile(fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException(
                    $"Test data file '{fileName}' not found in any TestData subdirectory"
                );

            return File.ReadAllText(filePath);
        }

        /// <summary>
        /// Finds a JSON file in TestData subdirectories
        /// Returns the first match found or throws if not found
        /// </summary>
        private static string FindJsonFile(string fileName)
        {
            if (!Directory.Exists(TestDataPath))
                throw new DirectoryNotFoundException(
                    $"TestData directory not found at: {TestDataPath}"
                );

            // Search in all subdirectories recursively
            var files = Directory.GetFiles(TestDataPath, fileName, SearchOption.AllDirectories);

            if (files.Length == 0)
                return Path.Combine(TestDataPath, fileName); // Return path for error message

            // Return first match (assuming unique filenames per category)
            return files[0];
        }
    }
}
