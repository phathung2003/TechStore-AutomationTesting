# WEBSITE SELLING ELECTRONIC COMPONENTS 📱
***
## Hi Guys 🌷
### Welcome back to my team 🌏 

![panda](https://cdn.tgdd.vn//GameApp/-1//Hinhnenbachugau23-Kichthuoc1920x1080-800x450.jpg)
***
_🌷We are future programmers. We want to bring new and attractive software🌷_

 ### 📌 Target 
👉 Website selling electronic devices
This project we want to achieve is to design a website selling electronic components

### 📘 Technology
📘PHP 📘Laravel 📘Javascript 📘ReactJS
We will use those technologies in our project

### 🍩 My Team
|  Name  |  Work  |  Contact 📞  |
| :----: | :----: | :-------: |
| 👻 Trần Vũ Hoàng Sơn | FullStack | [Tran Vu Hoang Son](https://www.facebook.com/sonhoang2071?mibextid=ZbWKwL) |
| 🐰 Tô Lệ Thanh | FullStack | [Tô Lệ Thanh](https://www.facebook.com/profile.php?id=100015695650617&mibextid=ZbWKwL) |
| 🐧 Trương Quốc Huy | FullStack | [Trương Quốc Huy](https://www.facebook.com/gau.trang.372661?mibextid=ZbWKwL) |

### _💌 Thank you for taking the time to read our profile_
***
# We will clone instructions on github 🐋 
***
__You need to clone github into your directory then run the following command:__

~~~ 
Docker compose up -d 
~~~
The command "docker-compose up -d" is used to start Docker containers defined in a Docker Compose configuration file in detached mode. Let me break down the command for you:

docker-compose: This is the command-line tool for working with Docker Compose, a tool for defining and running multi-container Docker applications. It allows you to define all the services, networks, volumes, and configurations for your application in a single Compose file (usually named docker-compose.yml).

up: This subcommand is used to create and start the containers defined in the Compose file. It starts or restarts all the services as defined in the Compose file.

-d or --detach: This is an optional flag. When you add the -d flag, it runs the containers in "detached" mode, meaning that the containers will run in the background, and the command prompt will be returned to you immediately. This is useful for long-running services where you don't need to see the container's output in your terminal. If you omit this flag, you will see the logs from the containers in your terminal, and you won't be able to run other commands until you stop the containers (by pressing Ctrl+C).

So, when you run docker-compose up -d, you're telling Docker Compose to start all the services defined in your Compose file in the background, allowing you to continue using your terminal for other tasks without being tied to the container's output.

In addition, we have created a file "MakeFile" to help you run the command more easily and concisely, you just need to run the following command:
~~~
Makefile up-d
~~~
***
## _📗Makefile_ 
***
~~~
config:
	- docker compose exec php php artisan config:cache
migrate:
	- docker compose exec php php artisan migrate
migrate-reset:
	- docker compose exec php php artisan migrate:reset
model:
	- docker compose exec php php artisan make:model
view:
	- docker compose exec php php artisan make:view
controller:
	- docker compose exec php php artisan make:controller
stop: 
	- docker compose stop
up-d:
	- docker compose up -d
build:
	- docker compose build
~~~
__Continue, run the following command:__
~~~
Docker compose build
~~~
The command "docker-compose build" is used to build Docker images for services defined in a Docker Compose configuration file. Let me explain the meaning of this command:

docker-compose: This is the command-line tool for working with Docker Compose, which is used for defining and running multi-container Docker applications. Docker Compose allows you to define all the services, their configurations, and how they should interact in a single Compose file (usually named docker-compose.yml).

build: This is a subcommand that you use with Docker Compose. When you run docker-compose build, it tells Docker Compose to build the Docker images specified in your Compose file. Docker Compose will look at the build section within each service definition in your Compose file to determine how to build the image. The build section typically specifies a Dockerfile and a context directory.

The Dockerfile is a configuration file that defines the steps for creating the Docker image.
The context directory is the path where the Docker build process will look for files and directories to include in the image. It is often the directory containing your application code.
Running docker-compose build is useful when you have made changes to your application's source code or Dockerfile and need to update the Docker images. It will build new images based on the updated information specified in your Compose file. Once the images are built, you can use docker-compose up to start containers using these newly built images.

In summary, docker-compose build is used to build Docker images as defined in your Docker Compose configuration, ensuring that your containers are using the latest version of the images, incorporating any changes made to your application code or Dockerfile.
***
__And of course you can run the following command:__
~~~
Makefile build
~~~
__Once you have successfully run the above command, continue running the following command:__
~~~
Docker compose run composer dump-autoload
~~~
The command "docker-compose run composer dump-autoload" appears to be a combination of Docker Compose and Composer commands used to manage dependencies in a PHP project. Let me break it down:

docker-compose: This is the command-line tool for working with Docker Compose, which is used to define and run multi-container Docker applications.

run: This is a subcommand of Docker Compose. It is used to run a one-time command in a service container as specified in your Docker Compose configuration.

composer: This is a reference to Composer, a popular PHP dependency management tool. Composer is used to manage and install PHP libraries and dependencies for a project.

dump-autoload: This is a specific command for Composer. The dump-autoload command is used to generate an optimized autoloader for your PHP project. Autoloaders in PHP help load classes and files automatically without having to include them manually in your code.

So, when you run "docker-compose run composer dump-autoload," you are running the Composer command dump-autoload inside a Docker container specified in your Docker Compose configuration. This is typically used in PHP projects to regenerate the autoloader files after adding or updating dependencies. By doing this inside a container, you ensure that the Composer command runs in an isolated environment with the correct dependencies, without interfering with your local system's PHP setup.

__Make sure you have php loaded. Once you have successfully downloaded php on linux, run the next command:__
~~~
Docker compose exec php php artisan key:generate
~~~
The command "docker-compose exec php php artisan key:generate" is used to generate an application key for a Laravel application running in a Docker container. Let's break down the command step by step:

docker-compose: This is the command-line tool for working with Docker Compose, which is used to define and run multi-container Docker applications.

exec: This subcommand is used to execute a command within a running container. In this case, it is used to run a command inside a Docker container defined in your Docker Compose configuration.

php: This is the first php in the command, and it specifies the name of the service/container in your Docker Compose configuration that you want to run the command within. In this case, it is a container running PHP, likely associated with a Laravel application.

php: This is the second php in the command and indicates that you want to run the PHP interpreter inside the specified container.

artisan: This is the Laravel Artisan command-line tool, which is used to perform various tasks in a Laravel application, such as database migrations, seeding, and key generation.

key:generate: This is a specific Artisan command used to generate a unique application key for a Laravel application. The application key is used for encrypting session data and other security-related features.

So, when you run "docker-compose exec php php artisan key:generate," you are telling Docker Compose to execute the "php artisan key:generate" command inside the specified PHP container. This command generates a new application key for your Laravel application, enhancing its security. This is a common step when setting up a Laravel application, typically done after installing Laravel and setting up the environment.
***
__Next, run the following command:__
~~~
Docker compose run npm install
~~~
The command "docker-compose run npm install" is used to run the "npm install" command inside a Docker container. Let's break down the command step by step:

docker-compose: This is the command-line tool for working with Docker Compose, which is used to define and run multi-container Docker applications.

run: This is a subcommand of Docker Compose, and it's used to run a one-time command inside a service container as specified in your Docker Compose configuration.

npm: This is a reference to Node Package Manager (npm), a popular package manager for JavaScript and Node.js applications. It's used to manage and install JavaScript dependencies and packages.

install: This is a specific command for npm. The "npm install" command is used to install the dependencies specified in a project's "package.json" file.

So, when you run "docker-compose run npm install," you are executing the "npm install" command inside a Docker container. This is typically used in the context of a JavaScript or Node.js project to install the project's dependencies and packages. By running this command inside a container, you ensure that the dependencies are installed in an isolated environment, which can be useful for maintaining consistency and avoiding conflicts between different projects and their dependencies.

~~~
Docker compose run npm run build
~~~
So, when you run "docker-compose run npm run build," you are executing a custom build script defined in a Node.js project inside a Docker container. This command is commonly used in the development and build process of JavaScript applications to automate tasks required for preparing the application for production or distribution. Running it within a Docker container helps ensure consistency in the build environment and avoids conflicts with local development tools and dependencies.
__Finally, run the following command:__
~~~
Docker compose exec php php artisan migrate:reset
~~~
So, when you run "docker-compose exec php php artisan migrate:reset," you are telling Docker Compose to execute the Laravel Artisan command "migrate:reset" inside the specified PHP container. This command is typically used to reverse all previous migrations and undo changes made to the database schema, which can be helpful for tasks like cleaning and resetting a development database or preparing it for testing.
***
***
🌷🌷🌷
### _Thank you and please look forward to our upcoming developments_ 🌱
***
