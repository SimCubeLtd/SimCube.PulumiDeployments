using Pulumi.AzureNative.Storage;

namespace SimCube.PulumiDeployments.Resources.Azure.Storage;

public sealed class QueueStorageResource : BaseAzureResource<QueueStorageResource, QueueStorageResourceArgs>
{
    public QueueStorageResource(
        string name,
        QueueStorageResourceArgs args,
        ComponentResourceOptions? options = null)
        : base(
            name,
            args,
            options)
    {
        var defaultStorageQueueName = $"{ResourceNames.StorageQueue}-{args.Location}-{args.Environment}";

        var storageQueue = new Queue(
            args.StorageQueueName ?? defaultStorageQueueName,
            new()
            {
                AccountName = args.BlobStorage.StorageAccountName,
                QueueName = args.StorageQueueName ?? defaultStorageQueueName,
                ResourceGroupName = args.ResourceGroup.Name,
            });

        StorageQueueId = storageQueue.Id;

        RegisterOutputs();
    }

    public Output<string> StorageQueueId { get; }
}