using Amazon.SQS;
using Amazon.SQS.Model;
using System.Text.Json;

//string accessKey = string.Empty;
//string secretKey = string.Empty;
//var region = Amazon.RegionEndpoint.EUWest3;

var sqsClient = new AmazonSQSClient();

var customer = new
{
    FirstName = "Emre",
    LastName = "Can",
    Age = 28
};

Console.WriteLine(" [*] Started sending queue message...");
var queueUrlResponse = await sqsClient.GetQueueUrlAsync("Customers"); //kuyruk ismi ile bağlanmak için

var sendMessageRequest = new SendMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customer), //Kuyruğa sadece string değer ekleyebiliyoruz.
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue
            {
                DataType = "String",
                StringValue = "Customer"
            }
        }
    }
};

SendMessageResponse response = await sqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine(" [*] Sending queue message is now complete...");
Console.Read();


