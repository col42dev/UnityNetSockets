# UnityNetSockets

Unity C# Net Sockets (prototype)

On startup a client instance creates a System.Net.Sockets instance with which it connects to a server by specifying an System.Net.IPEndPoint. 
Then clients then sends JSON endcoded as ASCII to the server in batches.
The server is hosted within the same application, on startup it creates a System.Net.Sockets with the same System.Net.IPEndPoint and asyncronously [listens](https://msdn.microsoft.com/en-us/library/system.net.sockets.socket.listen(v=vs.110).aspx) for connections.
When a connection is recieved the callback handler stops listening for new connections and establishes a Read handler with the connected client.
Received data is parsed as JSON.
