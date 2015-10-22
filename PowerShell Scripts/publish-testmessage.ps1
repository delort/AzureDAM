#Define the storage account and context.
$StorageAccountName = "azuredam"
$StorageAccountKey = Get-AzureStorageKey -StorageAccountName $StorageAccountName
$Ctx = New-AzureStorageContext –StorageAccountName $StorageAccountName -StorageAccountKey $StorageAccountKey.Primary

#Retrieve the queue.
$QueueName = "newimageadded"
$Queue = Get-AzureStorageQueue -Name $QueueName -Context $ctx

#If the queue exists, add a new message.
if ($Queue -ne $null) {
   # Create a new message using a constructor of the CloudQueueMessage class.
   $QueueMessage = New-Object -TypeName Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage -ArgumentList '{ImageName:"01bb8c3d-9320-452f-b810-04e7e72c4a9e/0860.JPG"}'

   #Add a new message to the queue.
   $Queue.CloudQueue.AddMessage($QueueMessage)
}

