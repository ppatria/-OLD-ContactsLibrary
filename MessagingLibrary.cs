using System;

namespace Contacts
{

	public class MessagingLibrary
	{

		public readonly int UserID;

        public MessagingLibrary()
        {
            try
            {
                if (!MessageQueue.Exists(queuePath))
                {
                    MessageQueue.Create(queuePath);
                }
                else
                {
                    Console.WriteLine(queuePath + " already exists.");
                }
            }
            catch (MessageQueueException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public MessagingLibrary(int userID)
		{
			UserID = userID;
		}

        public void CreateQueue()
        {
            try
            {
                if (!MessageQueue.Exists(queuePath))
                {
                    MessageQueue.Create(queuePath);
                }
                else
                {
                    Console.WriteLine(queuePath + " already exists.");
                }
            }
            catch (MessageQueueException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void SendMessage(MessagePriority priority, string messageBody)
        {

            // Connect to a queue on the local computer.
            MessageQueue myQueue = new MessageQueue(".\\myQueue");

            // Create a new message.
            Message myMessage = new Message();

            if (priority > MessagePriority.Normal)
            {
                myMessage.Body = "High Priority: " + messageBody;
            }
            else
            {0p
                myMessage.Body = messageBody;
            }

            // Set the priority of the message.
            myMessage.Priority = priority;

            // Send the Order to the queue.
            myQueue.Send(myMessage);

            return;
        }

        public void ReceiveMessage()
        {
            // Connect to the a queue on the local computer.
            MessageQueue myQueue = new MessageQueue(".\\myQueue");

            // Set the queue to read the priority. By default, it
            // is not read.
            myQueue.MessageReadPropertyFilter.Priority = true;

            // Set the formatter to indicate body contains a string.
            myQueue.Formatter = new XmlMessageFormatter(new Type[]
                {typeof(string)});

            try
            {
                // Receive and format the message.
                Message myMessage = myQueue.Receive();

                // Display message information.
                Console.WriteLine("Priority: " + myMessage.Priority.ToString());
                Console.WriteLine("Body: " + myMessage.Body.ToString());
            }

            catch (MessageQueueException)
            {
                // Handle Message Queuing exceptions.
            }

            // Handle invalid serialization format.
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }

            // Catch other exceptions as necessary.

            return;
        }

    }
}
