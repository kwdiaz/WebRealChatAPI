# WebRealChatAPI

## Overview

This project is a real-time chat application built using ASP.NET Core and SignalR. It provides functionalities for user registration, logging in, sending and receiving messages, and tracking online users. The chat application stores messages in a SQL Server database, ensuring persistence.

## Features

- **User Authentication**: Register and log in with username and password.
- **Real-time Messaging**: Users can send and receive messages in real-time using SignalR.
- **Track Online Users**: Display a list of users currently online.
- **Database Storage**: Messages are stored in a SQL Server database for persistence.

## Database Schema

The project uses SQL Server with the following tables:

- **Users**: Manages user accounts for authentication.
- **ChatMessages**: Stores chat messages between users.

### Table Definitions

- **Users**

  - `Id`: Primary Key
  - `Username`: Unique username
  - `PasswordHash`: Hashed password for security
  - `Name`: User's display name
  - `CreatedAt`: Account creation timestamp

- **ChatMessages**

  - `Id`: Primary Key
  - `User`: Username of the sender
  - `Recipient`: Username of the recipient
  - `Message`: Content of the message
  - `Timestamp`: Timestamp of when the message was sent

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (NET 8)
- SQL Server
- [Visual Studio](https://visualstudio.microsoft.com/)
- [Postman](https://www.postman.com/) (optional for testing APIs)

### Installation

1. Clone the repository:
   ```bash
   cd C:\ruta\de\tu\carpeta
   git clone https://github.com/kwdiaz/WebRealChatAPI.git
   ```
2. In the folder where you cloned the repository, open `WebRealChatAPI.sln` in Visual Studio.

### Database Setup

1. Create a new database named `ChatDb` in SQL Server.
2. Execute the following SQL script to create the necessary tables:
   ```sql
   CREATE TABLE Users (
       Id INT PRIMARY KEY IDENTITY(1,1),
       Username NVARCHAR(50) NOT NULL UNIQUE,
       PasswordHash NVARCHAR(512) NOT NULL,
       Name NVARCHAR(100),
       CreatedAt DATETIME DEFAULT GETDATE()
   );

   CREATE TABLE ChatMessages (
       Id INT PRIMARY KEY IDENTITY(1,1),
       User NVARCHAR(50),
       Recipient NVARCHAR(50),
       Message NVARCHAR(MAX),
       Timestamp DATETIME
   );
   ```
   or just run the commands to use entity framework
   ```bash
   dotnet ef migrations add Name_of_the_Commit
   dotnet ef database update
    ```
### Configuration

1. **Change the Connection String**:
   - Open the `appsettings.json` file in the project.
   - Locate the `ConnectionStrings` section and update the `ChatDatabase` string with your SQL Server credentials:
     ```json
     "ConnectionStrings": {
         "ChatDatabase": "Server=localhost; Database=RealChatDb; Integrated Security=True; Trusted_Connection=True; TrustServerCertificate=True;"
     }
     ```
   - Or in case you have SQL Server with username and password:
     ```json
     "ConnectionStrings": {
         "ChatDatabase": "Server=server_name;Database=RealChatDb;User Id=your_id;Password=your_pass;TrustServerCertificate=True;MultipleActiveResultSets=true"
     }
     ```

### Testing the Project

1. **Run the Project**:

   - Launch the project by clicking the green button in Visual Studio. This button has an arrow that allows you to select your preferred web browser to run the application.

2. **Access Swagger UI**:

   - Once the project is running, Swagger UI will automatically open in your browser. This interface provides access to the available APIs.

3. **Register a New User**:

   - Navigate to the `/Auth/Register` endpoint and fill in the required fields using the following schema:
     ```json
     {
       "username": "string",
       "password": "string",
       "name": "string"
     }
     ```

4. **Log In**:

   - After registration, proceed to the `/Auth/Login` endpoint with the following schema:
     ```json
     {
       "username": "string",
       "password": "string"
     }
     ```
   - Upon successful execution, you will receive a JSON Web Token (JWT).

5. **Authenticate API Requests**:

   - To access other protected APIs, click on the lock icon in Swagger UI.
   - In the authorization dialog, enter your token prefixed by the word `Bearer`, formatted like this:
     ```
     Bearer [token]
     ```
   - With the token successfully added, you will now have access to the other available APIs.

6. **Test Chat Functionality**:

   - To test the real-time chat functionality, you can open multiple browser windows or tabs.
   - Use the `/Chat/SendMessage` endpoint to send messages between users and verify that messages are received in real-time.
  
7. **Testing the deploy Version**:

    - Live Deployment: The chat application has been deployed to Azure and is accessible at the following URL:[Real-Time Chat Application](https://webrealchatapi20241017151223.azurewebsites.net/index.html). This link provides access to the live version of the application, where users can register, log in, and interact in real-time.

    - User Experience: The deployed application maintains the real-time capabilities enabled by SignalR, ensuring users can experience seamless messaging. The deployment on Azure also allows for testing scalability and reliability under different conditions, demonstrating how the app handles concurrent users and real-time communication effectively.

