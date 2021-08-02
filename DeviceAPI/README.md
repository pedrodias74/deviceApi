<!--
*** Thanks for checking out the Best-README-Template. If you have a suggestion
*** that would make this better, please fork the repo and create a pull request
*** or simply open an issue with the tag "enhancement".
*** Thanks again! Now go create something AMAZING! :D
***
***
***
*** To avoid retyping too much info. Do a search and replace for the following:
*** github_username, repo_name, twitter_handle, email, project_title, project_description
-->



<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]



<!-- PROJECT LOGO -->
<br />
<p align="center">
  <a href="https://github.com/github_username/repo_name">
    <img src="images/logo.png" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">project_title</h3>

  <p align="center">
    project_description
    <br />
    <a href="https://github.com/github_username/repo_name"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/github_username/repo_name">View Demo</a>
    ·
    <a href="https://github.com/github_username/repo_name/issues">Report Bug</a>
    ·
    <a href="https://github.com/github_username/repo_name/issues">Request Feature</a>
  </p>
</p>



<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary><h2 style="display: inline-block">Table of Contents</h2></summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgements">Acknowledgements</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

[![Product Name Screen Shot][product-screenshot]](https://example.com)

Device REST API Demo Project

    Directory Structure:

    - Scripts - database creation scripts
    - DeviceAPI - REST API
    - TestDeviceAPI - Nunit test project

    - BuildDebug.bat        - builds debug configuration
    - BuildRelease.bat      - builds release configuration
    - RunServerDebug.bat    - launches the rest api server (uses HTTPS and port 5001)
    - RunServerRelease.bat  - launches the rest api server (uses HTTPS and port 5001)
    - RunTestDebug.bat      - runs the tests in debug configuration
    - RunTestRelease.bat    - runs the tests in release configuration

Here's a blank template to get started:
**To avoid retyping too much info. Do a search and replace with your text editor for the following:**
`github_username`, `repo_name`, `twitter_handle`, `email`, `project_title`, `project_description`


### Built With

* []()
* []()
* []()



<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple steps.

### Prerequisites

* .Net Core 3.1

### Installation

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


<!-- USAGE EXAMPLES -->
## Usage

    REST Endpoint
    
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


<!-- ROADMAP -->
## Roadmap

See the [open issues](https://github.com/github_username/repo_name/issues) for a list of proposed features (and known issues).



<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.



<!-- CONTACT -->
## Contact

Your Name - [@twitter_handle](https://twitter.com/twitter_handle) - email

Project Link: [https://github.com/github_username/repo_name](https://github.com/github_username/repo_name)



<!-- ACKNOWLEDGEMENTS -->
## Acknowledgements

* []()
* []()
* []()





<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/github_username/repo.svg?style=for-the-badge
[contributors-url]: https://github.com/github_username/repo/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/github_username/repo.svg?style=for-the-badge
[forks-url]: https://github.com/github_username/repo/network/members
[stars-shield]: https://img.shields.io/github/stars/github_username/repo.svg?style=for-the-badge
[stars-url]: https://github.com/github_username/repo/stargazers
[issues-shield]: https://img.shields.io/github/issues/github_username/repo.svg?style=for-the-badge
[issues-url]: https://github.com/github_username/repo/issues
[license-shield]: https://img.shields.io/github/license/github_username/repo.svg?style=for-the-badge
[license-url]: https://github.com/github_username/repo/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/github_username

   





