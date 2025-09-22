Blog RESTful API

A simple RESTful API for a blogging platform built with .NET 8 Minimal APIs, Entity Framework Core, and PostgreSQL. The application is fully containerized using Docker for a consistent and easy-to-manage development environment.

## Features

  - List all blog posts with their respective comment counts.
  - Create new blog posts.
  - Retrieve a specific blog post by its ID, including all its comments.
  - Add a new comment to a specific blog post.
  - Built-in input validation for creating posts and comments.
  - Dockerized environment for seamless setup and deployment.
  - Swagger/OpenAPI documentation for easy API testing and exploration.

-----

## Tech Stack

  - **.NET 8**
  - **C\#**
  - **ASP.NET Core Minimal API**
  - **Entity Framework Core 8**
  - **PostgreSQL**
  - **Docker & Docker Compose**

-----

## Prerequisites

Before you begin, ensure you have the following installed on your machine:

  - **Docker Engine and Docker Compose**: To build and run the application containers. For Windows and macOS, these are typically included with the Docker Desktop installation.
  - **.NET 8 SDK**: Required for running Entity Framework Core commands (like creating new migrations).

-----

## Getting Started (First-Time Setup)

### 1\. Run with Docker Compose

The easiest and recommended way to run the project is by using Docker Compose. This command will build the API image, pull the official PostgreSQL image, and start both containers in a configured network.

From the root directory of the project, run the following command:

```bash
docker-compose up --build
```

  - `--build`: This flag forces Docker to rebuild the API image if there have been any changes to the source code or `Dockerfile`.

### 2\. Verify the Application

Once the containers are running, the application will be available:

  - **API Base URL**: `http://localhost:5000`
  - **Swagger UI (for testing)**: Open your browser and navigate to **`http://localhost:5000/swagger`**

The database schema will be created automatically when the API container starts. The application is configured to apply any pending Entity Framework migrations on startup.

-----

## API Endpoints

You can use the built-in Swagger UI to interact with all API endpoints.

#### `GET /api/posts`

Returns a list of all blog posts with a summary.

  - **Success Response (200 OK)**:
    ```json
    [
      {
        "id": 1,
        "title": "My First Blog Post",
        "commentCount": 2
      }
    ]
    ```

#### `POST /api/posts`

Creates a new blog post.

  - **Request Body**:
    ```json
    {
      "title": "A New Post About .NET",
      "content": "This is the full content of the post."
    }
    ```
  - **Success Response (201 Created)**:
    ```json
    {
      "id": 2,
      "title": "A New Post About .NET",
      "commentCount": 0
    }
    ```

#### `GET /api/posts/{id}`

Retrieves a specific blog post and its associated comments.

  - **Success Response (200 OK)**:
    ```json
    {
      "id": 1,
      "title": "My First Blog Post",
      "content": "Content of the first post.",
      "comments": [
        {
          "id": 1,
          "text": "Great post!"
        },
        {
          "id": 2,
          "text": "Thanks for sharing."
        }
      ]
    }
    ```

#### `POST /api/posts/{id}/comments`

Adds a new comment to a specific blog post.

  - **Request Body**:
    ```json
    {
      "text": "This is a new comment."
    }
    ```
  - **Success Response (201 Created)**:
    ```json
    {
      "id": 3,
      "text": "This is a new comment."
    }
    ```

-----

## Database Migrations
### How to Add a New Migration

1.  **Ensure the project builds successfully.**
2.  **Run the `dotnet ef migrations add` command** from your terminal in the root directory of the project.

<!-- end list -->

```bash
dotnet ef migrations add <YourMigrationName> -o Data/Migrations
```

A new migration file will be generated in the `Data/Migrations` directory. The next time you run `docker-compose up`, the application will automatically apply this new migration to the database.

-----

## Future Improvements

This project provides a solid foundation, but there are many ways it could be extended and improved:

  - **Authentication & Authorization**: Implement JWT-based authentication to secure the endpoints. For example, only authenticated users should be able to create posts or comments.
  - **PUT & DELETE Endpoints**: Add functionality to edit (`PUT /api/posts/{id}`) and delete (`DELETE /api/posts/{id}`) blog posts and comments. This would also require authorization logic to ensure users can only modify their own content.
  - **Pagination**: The `GET /api/posts` endpoint currently returns all posts. For a large number of posts, this is inefficient. Implementing pagination (e.g., using `?page=1&pageSize=10`) is essential.
  - **Global Error Handling**: Implement a custom exception handling middleware to catch unhandled exceptions and return standardized error responses.
  - **Unit & Integration Testing**: Add a separate test project to write unit tests for business logic and integration tests for the API endpoints to ensure reliability.
  - **Structured Logging**: Integrate a logging library like **Serilog** to write structured logs to files or a log aggregator, which is crucial for monitoring and debugging in production.
  - **CI/CD Pipeline**: Set up a Continuous Integration/Continuous Deployment pipeline (e.g., using GitHub Actions) to automatically build, test, and deploy the application.
