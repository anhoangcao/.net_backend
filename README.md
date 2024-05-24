# Fullstack .NET & Vue.js Project

This project is a fullstack web application built using .NET Core for the backend and Vue.js for the frontend. It provides a comprehensive solution for managing various resources with a modern web interface.

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Technologies](#technologies)
- [Installation](#installation)
  - [Backend](#backend)
  - [Frontend](#frontend)
- [Usage](#usage)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [License](#license)

## Introduction

This project combines a powerful .NET Core backend with a dynamic Vue.js frontend to create a seamless user experience. It is designed to handle user authentication, data management, and provide a responsive user interface.

## Features

- User authentication and authorization
- CRUD operations for various resources
- Responsive design with Vue.js
- Error handling and logging
- Integration with SQL databases
- RESTful API endpoints
- Swagger for API documentation

## Technologies

### Backend

- .NET Core 5.0 or later
- Entity Framework Core
- Swagger for API documentation

### Frontend

- Vue.js 3 or later
- Vuex for state management
- Axios for API calls
- Vuetify for UI components

## Installation

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or any other supported database

### Backend

1. Clone the repository:

    ```bash
    git clone https://github.com/anhoangcao/.net_backend.git
    cd .net_backend
    ```

2. Set up the database connection string in `appsettings.json`:

    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"
    }
    ```

3. Run the following commands to restore dependencies and build the project:

    ```bash
    dotnet build
    dotnet ef migrations add InitialCreate
    ```

4. Apply database migrations:

    ```bash
    dotnet ef database update
    ```

5. Run the application:

    ```bash
    dotnet run
    ```

### Frontend

1. Navigate to the frontend directory:

    ```bash
    cd ../frontend
    ```

2. Install dependencies:

    ```bash
    npm install
    ```

3. Start the development server:

    ```bash
    npm run dev
    ```

## Usage

### Backend

The backend service exposes several API endpoints for managing resources. You can use tools like [Postman](https://www.postman.com/) to test the APIs.

The base URL for the API is `http://localhost:5000/api`.

### Frontend

The frontend application will be available at `http://localhost:8080`. You can use your browser to interact with the application.

## API Documentation

The API documentation is available at `http://localhost:5000/swagger` once the backend application is running.

## Contributing

Contributions are welcome! Please fork the repository and create a pull request with your changes. Ensure that your code follows the existing style and includes tests for new features or bug fixes.

1. Fork the repository
2. Create a new branch (`git checkout -b feature/your-feature`)
3. Commit your changes (`git commit -m 'Add some feature'`)
4. Push to the branch (`git push origin feature/your-feature`)
5. Create a new pull request

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.
