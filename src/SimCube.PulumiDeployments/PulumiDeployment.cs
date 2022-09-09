namespace SimCube.PulumiDeployments;

public static class PulumiDeployment
{
    public static async Task Execute<TStack>()
        where TStack : Stack, new()
    {
        await Deployment.RunAsync<TStack>();

        DeploymentHelpers.DeleteDirectoryAfterCompletionOrError(BaseHelmChartResource.HelmValuesFolder);
    }
}
