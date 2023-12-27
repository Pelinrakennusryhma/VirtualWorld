# Virtual World

Multiplayer Virtual World made with Unity.

## Installing
Clone repository to your computer. Open with Unity editor.

## Backend dependency
For user authentication and database to work clone [Virtual World Backend project](https://github.com/Pelinrakennusryhma/Virtual-World-Backend) and follow the instructions in the README to get it up and running locally.

In Assets folder, create a folder called `Config` if it doesn't already exist. In the `Config` folder create `GameConfig.json` file with the following contents:
```
  "PROD_IpForClient": [IP for client to connect to (AWS server's ip)]
  "PROD_URLForClient": [SSL enabled server address for client to connect to(the server's dns address)]
  "DEV_clientBackendUrl": [Address of the authentication server in DEV mode (likely http://localhost:3001)]
  "PROD_clientBackendUrl": [HTTPS address of the authentication server for client to connect to (likely same as PROD_URLForClient)]
  "serverBackendUrl": [Internal address for server to backend connection. Same in DEV and PROD (likely http://localhost:3002)]
```

## Links
[Virtual World Backend repository](https://github.com/Pelinrakennusryhma/Virtual-World-Backend)
