
# Key Design Decisions for Real-Time Chat Application

## Overview

This document outlines the key design decisions made during the development of the real-time chat application, focusing on architecture, technology choices, and implementation strategies.

## 1. Architecture

- **Clean Architecture**: The application follows a clean architecture pattern, separating concerns into models, services, controllers, and data access layers. This promotes maintainability, scalability, and testability by ensuring that each component has a clear responsibility and dependencies flow in a controlled manner.

- **Layered Separation**: We utilized a clear separation between the UI, API, business logic, and data storage. SignalR was integrated specifically to handle real-time communication, while ASP.NET Core provides the foundational API layer.

## 2. Technology Choices

- **Framework**: ASP.NET Core was chosen for its robustness, performance, and ease of integration with other tools, such as Entity Framework. It also supports building scalable web APIs and SignalR hubs for real-time communication.

- **SignalR for Real-Time Messaging**: SignalR was chosen to handle real-time messaging between users. It allows bi-directional communication between the server and clients, which is essential for a chat application. SignalR simplifies complex real-time scenarios like messaging without the need for manually implementing WebSocket connections.

- **Database**: SQL Server was selected as the database to store user data and messages. SQL Server's transactional capabilities and support for relational data make it a reliable choice for this type of application. It also integrates well with Entity Framework, which helps to simplify database operations.

## 3. Data Storage
### 3.1. Database Architecture

-Architecture for Data Layer: We used a layered data architecture to ensure a clear separation between business logic and data access. Entity Framework serves as the intermediary between the application and SQL Server, providing an abstraction that helps in managing complex data operations while simplifying development.

-Rationale for SQL Server: SQL Server was selected due to its reliability, transactional support, and robust integration with ASP.NET Core. This architecture ensures consistency in data storage, efficient querying, and easy scalability for future requirements.

Table Structure:

Users: A dedicated table to securely manage user accounts and authentication data.

ChatMessages: This table stores all chat messages, including sender, recipient, message content, and timestamps, ensuring that the chat history is persistent and retrievable.

Entity Framework: Entity Framework was used to manage data access. This decision was made to take advantage of its code-first approach, which allows rapid schema updates and integration with ASP.NET Core.

- **Table Structure**:
  - **Users**: A dedicated table to securely manage user accounts and authentication data.
  - **ChatMessages**: This table stores all chat messages, including sender, recipient, message content, and timestamps, ensuring that the chat history is persistent and retrievable.

- **Entity Framework**: Entity Framework was used to manage data access. This decision was made to take advantage of its code-first approach, which allows rapid schema updates and integration with ASP.NET Core.

## 4. Filtering and Tracking Users

- **Tracking Online Users**: A static in-memory data structure (`ConcurrentDictionary`) was used to track online users. This approach was chosen to efficiently manage real-time updates without unnecessary overhead or complex persistence.

- **Filtering Logic**: The server uses a central service layer to manage the filtering logic for user messages and user lists. By encapsulating this logic in a service, we can ensure reusability and maintain a separation of concerns.

## 5. Authentication and Security

- **Password Hashing**: Passwords are hashed using BCrypt to protect against breaches. This decision enhances security, as BCrypt adds a salt and is computationally expensive, making brute-force attacks more difficult.

- **JWT for Authentication**: JSON Web Tokens (JWT) are used for session management. JWT provides a stateless and scalable way to manage user authentication, ideal for distributed and cloud-based environments.

## 6. Testing and Documentation

- **Swagger Integration**: Swagger UI was included to document the APIs and provide an interactive interface for testing endpoints. This decision improves the developer experience and allows other developers or stakeholders to explore the API without writing any client code.

## 7. Assistance from ChatGPT

- **Integration and Error Resolution**: ChatGPT was used for assistance with SignalR integration, as there was initially limited experience with setting up real-time messaging using SignalR. Additionally, ChatGPT was helpful in diagnosing and resolving various errors encountered during development, ensuring the application functioned smoothly.

- **Deployment to Azure**: ChatGPT also provided guidance during the deployment of the application to Azure, helping to resolve several issues that were encountered. Additionally, the video [Netcode-Hub - Deploying to Azure](https://www.youtube.com/watch?v=-wtRY2IepGE&ab_channel=Netcode-Hub) was used as a source of support during the deployment process.

- **Documentation**: ChatGPT was also utilized to help draft documentation, including API descriptions and explanations of key design decisions, providing clarity and structure to the project.

## Conclusion

These design decisions were made to ensure that the chat application is scalable, secure, and easy to maintain. The use of ASP.NET Core, SignalR, and Entity Framework ensures modern web standards are met, while clean architecture principles guide the separation of concerns for future growth and enhancement. Ongoing user feedback and requirements will continue to shape improvements.
