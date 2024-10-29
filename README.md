Multithreading in .NET

Download and unzip 01.Multithreading.v.1.2.zip archive. Restore packages via “dotnet restore” if needed. Implement the tasks 1-6 using templates from the Multithreading.sln.
Note: If “dotnet restore” does not work, please make sure the environment variable contains a link to dotnet
Graphical user interface, text, application
Description automatically generated

Task 1 (MultiThreading.Task1.100Tasks.csproj):
Open 100Tasks project and write a program that creates an array of 100 Tasks, runs them and waits till all of them are completed. Each Task should iterate from 1 to 1000 and print to the console the following string: “Task #0 – {iteration number}”.

Task 2 (MultiThreading.Task2.Chaining.csproj):
Open Chaining project and write a program that creates a chain of four Tasks:
1st Task creates an array of 10 random integers.
2nd Task multiplies this array with another random integer.
3rd Task sorts this array by ascending.
4th Task calculates the average value.
All these tasks should print the values to console.

Task 3 (MultiThreading.Task3.MatrixMultiplier.csproj, MultiThreading.Task3.MatrixMultiplier.Tests.csproj):
Open MatrixMultiplier project and write a program that multiplies two matrices following the conditions below:
Implement the logic of MatricesMultiplierParallel class using Parallel class. All unit tests within MatrixMultiplier.Tests project should pass successfully.
Create a test inside MatrixMultiplier.Tests project to check which of the multiplier implementations (synchronous or parallel) runs faster. Find out the size that makes parallel multiplication more effective than the regular one.

Task 4 (MultiThreading.Task4.Threads.Join.csproj):
Open Threads.Join project to create a program that recursively creates 10 threads. Each thread should be with the same body and receive a state with an integer number, decrement it, print and pass as a state into the newly created thread. Implement all the following options:
Use Thread class for this task and Join for waiting threads.
Use ThreadPool class for this task and Semaphore for waiting threads.

Task 5 (MultiThreading.Task5.Threads.SharedCollection.csproj):
Open Threads.SharedCollection and write a program that creates two threads and a shared collection:
The 1st task should add 10 elements to the collection
The 2nd task should print all elements in the collection after each adding (in other words, if the collection contains elements from 1 to 10, the second thread should print something like [1], [1, 2], [1, 2, 3], …, [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], the number of elements should increase).
Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.

Task 6 (MultiThreading.Task6.Continuation.csproj):
In the Continuation project you need to create a task which should be attached to the parent with the following continuation criteria:
Continuation task should be executed regardless of the result of the parent task.
Continuation task should be executed when the parent task was completed without success.
Continuation task should be executed when the parent task failed and parent task thread should be reused for continuation
Continuation task should be executed outside of the thread pool when the parent task is cancelled
Demonstrate the work of each case with console utility.


Home task (Optional):
Our task is to create a client and a server for simple enterprise chat.
General requirements: 
Both Client and Server should be implemented as Console or GUI applications (you decide) 
Client and Server interacts with each other using Named Pipes (System.IO.Pipes) or Sockets (System.Net.Sockets) – you decide. For simplicity, connection parameters could be hardcoded. 
Client is a bot which performs the following operations in a loop: 
Connects to the Server with a new name 
Sends several messages to the Server with a short delay between messages. (Messages are retrieved from the list of already predefined messages, the number of messages and delay between messages is random) 
Receives all messages from the Server and displays them on the screen or stores in a text file. 
Disconnects from the Server. 
Repeat the loop until the User stops the Client or exception occurred. 
Server: 
Accepts connections from the Client. On connecting receives the name of the Client.  
Receives messages from the Client and broadcasts them to all other clients connected to this Server . 
Stores a history of N number (defined by you) of messages and sends this collection of messages to the Clients on their initial connection. 
Sends notification to the Clients and safely closes all connections on application close. 

Task 1:
Implement Client and Server using the following approach – one client – one thread. Read and write operations could be synchronous.

Task 2:
Implement Client and Server using one or both approaches:
Classic async operations (BeginXXX/EndXXX) and threads pool for operations
Task Parallel Library

Score board:
0-59% – 5 of 6 required tasks completed and implementation meets all requirements.
60-79% – All 6 required tasks completed, all tests are GREEN and implementation meets all requirements.
80-100% – No major remarks related to clean code principles (SOLID, KISS, DRY, etc.) / Optional task is covered.


Async Programming

Download and unzip AsyncAwait.Tasks.zip archive. Restore packages via “dotnet restore” if needed. Implement the tasks 1 and 2 using templates from the AsyncAwait.Tasks.sln.  
Note: If “dotnet restore” does not work, please make sure the environment variable contains a link to dotnet 

Task 1. Asynchronous calculation and cancellation tokens (AsyncAwait.Task1.CancellationTokens.csproj): 
Task:
Here is a code for application designed to calculate the sum of integer numbers from 0 to N. Please rework the application code to satisfy the following conditions:
The calculation should be asynchronous.
N should be set from Console as user input. User should be able to change the upper limit N in the middle of the calculation process. This change should abort current calculation and start a new one with the new N. 
There should be neither any exceptions nor application falls if the process of calculation restarts. 

Task 2. ASP MVC challenge (AsyncAwait.Task2.CodeReviewChallenge.csproj): 
Task:
Please perform code review of the provided ASP.NET Core application. Pay attention to async operations usage issues.
About application
Web app contains 3 pages, which could be navigated from the main menu: Home, Privacy, Help. Besides that, each page collects statistics (how many times this page was visited).
Probably, the navigations counting code is not optimal and causes the pages  loading slowly.

What you need to do:
1)  Review application code AsyncAwait.CodeReviewChallenge and paying attention to the wrong async code usage. Provide your ideas how these code issues could be resolved. 
2) Improve the code according to your proposals. Verify that application works stable. (Good idea here is to make your changes in a separate branch and then compare both implementations).
This solution also contains a project named ‘CloudServices’. This app emulates multiple calls to the third-party services. As it is a third-party library,  you shouldn’t change this code. All your changes should be made in AsyncAwait.CodeReviewChallenge project.
Discuss your ideas and results with your mentor. Be ready to describe how async code works in depth.

Score board: 

0-59% – Both required tasks have been completed, and implementation meets all requirements.  
60-79% – In the second task, a mentee improved the code (didn't make it worse) and can explain why it was necessary.
80-100% – A mentee understands pros and cons of the provided solutions and there are no major remarks related to clean code principles (SOLID, KISS, DRY, etc.).

topic/message-queues

Tasks 1 (Required):
Introduction
In this task we will create a system for continuous processing of scanned images. 
Traditionally such systems consist of multiple independent services. Services could run on the same PC or multiple servers. For instance, the following setup could be applied: 
Data capture service. Usually, data capture services have multiple instances installed on the multiple servers. The main purpose of these services is documents capturing and documents transferring next to the image transformation servers. 
Image transformation services. Also, there can be multiple instances for balancing workload. Such services could perform the following image processing tasks like format converting, text recognition (OCR), sending to other document processing systems. 
Main processing service. The purpose of the service is to monitor and control other services (data capturing and image transformation). 

We will implement simplified model with 2 elements: Data capture service and Processing service. 
Notes! Please discuss with you mentor the following details prior starting the task: 
Which exact message queue to use (e.g., MSMQ/RabbitMQ/Kafka) 
High-level solution architecture  
* use console application as services 
Collecting data processing results 
Implement the main processing service that should do the following: 
Create new queue on startup for receiving results from Data capture services. 
Listen to the queue and store  all incoming messages in a local folder. 
Implement Data capture service which will listen to a specific local folder and retrieve documents of some specific format (i.e., PDF) and send to Main processing service through message queue. 
Try to experiment with your system like sending large files (300-500 Mb), if necessary, configure your system to use another format (i.e. .mp4). 
For learning purposes assume that there could be multiple Data capture services, but we can have only one Processing server.  
Notes 
One of the challenges in this task is that we have a limit for message size. Message queues have limits for a single message size.  
Please find one of the approaches to bypass this limitation by clicking the link: Message Sequence. 
Discuss your solution with mentor. 
Create UML diagram for the chosen solution (Component diagram). 
http://draw.io/ - tool for drawing UML, approved freeware.

Task 2 (Optional):
Introduction
This task is an addition to the required task. The rRequired task should bemust be completed prior to starting the  complex task. 
Create UML diagrams for the solution from the required task: 

Component Diagram 
Sequence Diagram

Add elements which describe the principles of the provided below implementation provided below. 
Discuss diagrams and implementation details with your mentor: 
What patterns could be used to implement the missing part of the system? 
How system should behave in case of some services are being unavailable (i.e., restarting process of the message queue/processing service/data capture services)? 

Below you can find provided description of the centralized control system which extends the solution from the required task. 
Centralized control 
This mechanism allows to receive information from services about their statuses and send new settings to services if necessary. If service receives new settings, them should be applied for all new messages. 
Solution design: 
Capturing services periodically send current status to the Main processing service: 
Current service status (waiting for files/processing file) 
Current status: 
Max size of the message 
Additional info (if necessary) 
From Main processing service: 
Update status (force update before waiting for the status from the services) 
Change settings: 
Max size of the message 
Additional settings (if necessary) 

Score board:
0-59% – Constrains & Functional requirements are met (pay attention to Clean Code practices).
60-79% – Required task was successfully completed. You are able to answer the “Self-check questions”.
80-100% – Optional task was successfully completed.


Analyzing and profiling tools

Task 1
Here is an example of a method for generating password hash:
public string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
{
var iterate = 10000;
var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
byte[] hash = pbkdf2.GetBytes(20);
byte[] hashBytes = new byte[36];
Array.Copy(salt, 0, hashBytes, 0, 16);
Array.Copy(hash, 0, hashBytes, 16, 20);
var passwordHash = Convert.ToBase64String(hashBytes);
return passwordHash;
}

Try to review and optimize the code to improve the performance of the method. Do not reduce iterations’ number.

Task 2
Analyze ASP.NET MVC app from the ProfileSample.zip. Optimize the logic of retrieving images from the database. DB dump is stored in App_Data folder.

Task 3
Optimize the code of application GameOfLife.zip. It contains memory leak and has poor performance. Try to resolve both issues.

Task 4
We have DumpHomework.zip application which caused an exception and you have a dump file for this exception. Define a place of the error and try to solve it. 

Score board: 
0-59% – Constrains & Functional requirements in tasks 1 – 4 are met with minor issues (pay attention to Clean Code practices). 
60-79% – All functional and non-functional requirements in tasks 1 - 4 are met without issues and you can answer the “Self-check questions”.
80-100% – You can provide an example of how principles learnt are applied (or potentially could be applied) to the current project. 

