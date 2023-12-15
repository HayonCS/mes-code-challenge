# Configuration Routes

- `/GetValue`: Returns a key's deserialized value if it exists, or null if it does not.
- `/GetValues`: Returns an array of deserialized values based on a given value keyword.
- `/AddValue`: Adds a given key and JSON value to the database. Returns true if successful, returns a bad response if the key already exists.
- `/UpdateValue`: Updates an existing key's value to a new value. Returns true if successful, returns a bad response if the key does not exist.
- `/DeleteValue`: Deletes a given key from the database. Returns true if successful, returns a bad response if the key does not exist.

# Build A Configuration Storage Api

The Gentex MES team supports many solutions for storing and retrieving configuration for manufacturing applications and processes. In this excercise, you will build a REST api using Asp.Net for updating and retrieving simple key/value data stored in a database. The api should support different actions to allow a client to get, set, and delete configuration values through it. Values should be JSON to the client.

# Requirements

- Use Asp.Net to build a REST api supporting the following operations: search for a list of values by match filter, get value by key, add value, update value by key, and delete value by key.
- Keys should be of type string, and values should be serializable as JSON over http between client and api.
- The data should be stored in a file-based SQL database using [SqlLite](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli). The database should contain a single table with 2 columns for the purpose of this excercise:
  | Key | Value |
  |--|--|
  | key1 | [JSON serializable value] |
  | key2 | [JSON serializable value] |
- The api will need to create the database file and table if it doesn't exist. Use this [Hello World sample](https://github.com/dotnet/docs/tree/main/samples/snippets/standard/data/sqlite/HelloWorldSample) as an example table creation command.
- The api should be able to stop and start again without data loss.

# Tools Needed

Use a text editor like [VS Code](https://code.visualstudio.com/).

Verify dotnet is installed, or install from [here](https://dotnet.microsoft.com/en-us/download):

```bash
dotnet --info
```

# Getting Started

Scaffold out a new dotnet webapi from a console or VS Code terminal:

```bash
dotnet new webapi
```

Run the api from the console/terminal:

```bash
dotnet run
```

Navigate to the open api test page at http://localhost:[port]/swagger. The port number is generated for the project and can be found in the console output when run or in [launchSettings.json](/Properties/launchSettings.json).

```bash
    Now listening on: http://localhost:#####
```

You can use the test page to review your api and test it's behavior.
