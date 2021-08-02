REQUIREMENTS
    .Net Core 3.1

DIRECTORY STRUCTURE

    - Scripts - database creation scripts
    - DeviceAPI - REST API
    - TestDeviceAPI - Nunit test project

    - BuildDebug.bat        - builds debug configuration
    - BuildRelease.bat      - builds release configuration
    - RunServerDebug.bat    - launches the rest api server (uses HTTPS and port 5001)
    - RunServerRelease.bat  - launches the rest api server (uses HTTPS and port 5001)
    - RunTestDebug.bat      - runs the tests in debug configuration
    - RunTestRelease.bat    - runs the tests in release configuration

INSTRUCTIONS

    -> To build the project
        1. Run BuildRelease.bat

    -> To run endpoint tests with an in memory database (default):

        1. Run RunServerRelease.bat to launch the server 
        2. Run RunTestRelease.to run the tests

            (CTRL+C on the server console stops the server)

    -> To run endpoint tests with a Sqlserver database

        1. Create the database by running the following scripts in a Sql Server tool
            a) Scripts\create_database.sql
            b) Scripts\create_objects.sql
        2. Edit the file DeviceAPI\bin\Release\netcoreapp3.1\appsettings.json
            a) Configure "Test" setting to false ("Test": false)
            b) Configure the SqlServer database connection string in the setting "DeviceAPIContext"
        3. Run RunServerRelease.bat to launch the server 
        4. Run RunTestRelease.to run the tests
    
    
REST ENDPOINT
    
    Uses Windows Authentication

    Base URL https://localhost:5001/
    
    Operations

        GET api/devices                                                 - gets all devices
        GET api/devices/5                                               - gets device with id = 5
        GET api/devices?brand=Brand1                                    - gets all devices with brand = Brand1
        GET api/devices?page=2&rowsPerPage=10                           - gets all devices with pagination (page and rowsPerPage must be both a positive number)
        GET api/devices?brand=Brand1&page=2&rowsPerPage=10              - gets all devices with brand = Brand1 with pagination (page and rowsPerPage must be both a positive number)
        POST api/devices
            content-type: application/json
            body: {"id": 7, "name": "Device7", "brand": "Brand5"}
        DELETE api/devices/5                                            - deletes device with id = 5
        PUT api/devices/5                                               - changes device 5 (id is mandatory in both path and body; can change both name or brand properties or only one of them)
            content-type: application/json
            body: {"id": 5, "name": "Device55", "brand": "Brand55"}

