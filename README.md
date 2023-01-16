# Chat Room 

# Introduction:

You are implementing a chat room interface in which the user can view chat history at varying levels of time-based aggregation. For example, they can choose to see every chat event as it occurred, or stats about chat events for a given day

You do NOT have to implement the actual chat functionality - we are only interested in how the data is sorted, aggregated and rendered.

# How to run the app

There are 2 options in order to run this app.

 ##1.Console application. 
	
		-You could use a console app which loads a menu the 4 options.
		
		1.Enter the chat 	-> You will need to provide a name
		2.Leave the chat	-> You will need to provide a name
		3.Add comment 		-> You will need to provide the sender and the comment text.
		4.Send high five 	-> You will need to provide the sender name and the recipient name.
		5.Show past activity-> You will need to pass the granularity selecting from the options listed.

![image](https://user-images.githubusercontent.com/99485965/212759010-bfff4ada-d6a8-4f25-9446-e1830f29934f.png)

		
 ##2. Using the API
		
		-If you are willing to create your own app, you could use the Chat API too.
		-For testing purposes, you could just run the API and use Swagger
			1.Open a console and navigate to the repository, "cd ~/ChatAPI"
			2.dotnet run 
			3.Open a browser and navigate to "http://localhost:5047/swagger/index.html"
			4.You can easily make use of the endpoints to accomplish the same thing as with the console app.
			
		-Resources
		
			1.Enter the chat 	-> GET Users/Login
			2.Leave the chat	-> GET Users/Logout
			3.Add comment 		-> POST ChatEvents/message
			4.Send high five 	-> POST ChatEvents/highfive
			5.Show past activity-> GET ChatEvents Granularity -> (Seconds, Minutes, Hours, Days)
 
		
		*I added the Date as parameter in all event creations so you could play around with the activity agreggations.
		
		
![image](https://user-images.githubusercontent.com/99485965/212758424-df315015-6a57-495c-9177-864198dac45e.png)
		
		

