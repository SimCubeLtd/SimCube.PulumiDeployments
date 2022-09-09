namespace SimCube.PulumiDeployments.Helpers;

public static class DeploymentHelpers
{
    public static void DeleteDirectoryAfterCompletionOrError(string directoryName, bool recursive = true)
    {
        var directoryPath = Path.Combine(AppContext.BaseDirectory, directoryName);

        if (Directory.Exists(directoryPath))
        {
            Directory.Delete(directoryPath, recursive);
        }
    }
}
