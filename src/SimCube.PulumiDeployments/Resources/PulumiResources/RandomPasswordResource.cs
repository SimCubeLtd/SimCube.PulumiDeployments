using Pulumi.Random;
using SimCube.PulumiDeployments.Arguments.PulumiResources;

namespace SimCube.PulumiDeployments.Resources.PulumiResources;

public class RandomPasswordResource : RandomPassword
{
    private RandomPasswordResource(string name, RandomPasswordArgs args, CustomResourceOptions? options = null)
        : base(name, args, options)
    {
    }

    public static RandomPasswordResource Create(RandomPasswordResourceArgs args) =>
        new RandomPasswordResource(
            args.Name,
            new()
            {
                Length = args.Length,
                MinLower = args.MinLower,
                MinUpper = args.MinUpper,
                MinSpecial = args.MinSpecial,
                MinNumeric = args.MinNumeric,
            }
        );
}