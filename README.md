# Flash Fullstack Challenge

This project consists of a fullstack application with a Vue.js frontend, .NET backend, and SQLite database.

## Project Structure

- `frontend/`: Vue.js application built with Vite
- `backend/`: .NET Core Web API
- `database.db`: SQLite database file

## Frontend

The frontend is a Vue.js 3 application using the following technologies:

- Vue Router for navigation
- Pinia for state management
- TailwindCSS for styling
- TypeScript for type safety
- Vitest for unit testing

### Setup and Running

1. Navigate to the frontend directory:

```sh
cd frontend
```

2. Install dependencies:

```sh
yarn
```

3. Run development server:

```sh
yarn dev
```

To run the tests:

```sh
yarn test
```

The application will be available at `http://localhost:5173`

## Backend

### Setup and Running

1. Navigate to the backend directory:

```sh
cd backend
```

2. Restore .NET dependencies:

```sh
dotnet restore
```

3. Run the API:

```sh
dotnet run
```

The API will be available at `http://localhost:5203`

## Database

The application uses SQLite (`database.db`) as its database. The database includes tables for:

- Parking Accounts
- Contacts
- Vehicles

The database file is already configured and ready to use with the backend application.
