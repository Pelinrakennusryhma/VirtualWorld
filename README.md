# Virtual World

Multiplayer Virtual World made with Unity.

## Installing
Clone repository to your computer. Open with Unity editor.

## Backend dependency
For user authentication and database to work clone [Virtual World Backend project](https://github.com/Pelinrakennusryhma/Virtual-World-Backend) and follow the instructions in the README to get it up and running locally.

In Assets folder, create a folder called `Config` if it doesn't already exist. In the `Config` folder create `GameConfig.json` file with the following contents:
```
"DEV_IpForClient": "[IP for client to connect to in dev environment, likely your localhost]",
"PROD_IpForClient": "[IP of the deployed unity server]",
"PROD_URLForClient": "[URL for client for SSL enabled connection to work]",
"ipForServer":"[IP for server to run on, likely 0.0.0.0]",
"serverPort":"[Port of your choosing]",
"DEV_clientBackendUrl":"[URL for client to make authentication api calls to in dev environment, E.G http://localhost:3001]",
"PROD_clientBackendUrl":"[(https)URL for client to make authentication api calls to in prod environment]",
"serverBackendUrl": "[URL for server to make api calls to("DataApp" in the backend server)]"
```

## Links
[Virtual World Backend repository](https://github.com/Pelinrakennusryhma/Virtual-World-Backend)
