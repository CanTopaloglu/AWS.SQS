using Amazon.SQS;
using Amazon.SQS.Model;

var sqsClient = new AmazonSQSClient();

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("Customers");

var receiveMessageRequest = new ReceiveMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    AttributeNames = new List<string>() { "All" },
    MessageAttributeNames = new List<string>() { "All" }
};

var cts = new CancellationTokenSource(); //sonsuz döngüde kalmasın diye best-practice'i cancellationToken

while (!cts.IsCancellationRequested)
{
    var response = await sqsClient.ReceiveMessageAsync(receiveMessageRequest);
    foreach (var message in response.Messages)
    {
        //Mail gönder
        //SMS gönder

        try
        {
            Console.WriteLine($"Message Id: {message.MessageId}");
            Console.WriteLine($"Message Body: {message.Body}");

            await sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle); //kuyruktan kaldırıyorum, işlem başarılıysa.
        }
        catch (Exception)
        {

        }
    }

    await Task.Delay(100);
}