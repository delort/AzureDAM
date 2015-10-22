$StorageAccountName = "azuredam"
$StorageAccountKey = Get-AzureStorageKey -StorageAccountName $StorageAccountName
$Ctx = New-AzureStorageContext –StorageAccountName $StorageAccountName -StorageAccountKey $StorageAccountKey.Primary
$QueueName = "newvideoadded"
$Queue = New-AzureStorageQueue –Name $QueueName -Context $Ctx