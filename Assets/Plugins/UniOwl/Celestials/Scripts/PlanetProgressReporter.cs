using System;

namespace UniOwl.Celestials
{
    public static class PlanetProgressReporter
    {
        public static event Action<string, string> progressUpdated;

        private static string stage;
        private static string description;
        
        public static void ReportStage(string stage)
        {
            PlanetProgressReporter.stage = stage;
            progressUpdated?.Invoke(stage, description);
        }

        public static void ReportDescription(string description)
        {
            PlanetProgressReporter.description = description;
            progressUpdated?.Invoke(stage, description);
        }
    }
}
