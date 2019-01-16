using AZ_Paas_Demo.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount 
using Microsoft.WindowsAzure.Storage.Queue; // Namespace for Queue storage types
using AZ_Paas_Demo.Data.Models;
using Microsoft.Extensions.Options;

namespace AZ_Paas_Demo.Data.Services
{
    public class QueueService : IQueueService
    {
        private ConnectionStrings _connectionStrings;
        private CloudStorageAccount _storageAccount;
        public QueueService(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _storageAccount = CloudStorageAccount.Parse(connectionStrings.Value.AzureStorageConnection);
        }
        //https://docs.microsoft.com/en-us/azure/storage/queues/storage-dotnet-how-to-use-queues
        public async void QueueNewStoreCreation(string userAndStoreData)
        {
            // Create the queue client.
            CloudQueueClient queueClient = _storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Create the queue if it doesn't already exist
            await queue.CreateAsync();

            //and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage(userAndStoreData);
            await queue.AddMessageAsync(message);
        }
    }
}
