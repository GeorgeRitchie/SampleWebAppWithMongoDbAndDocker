# SampleWebAppWithMongoDbAndDocker

# What is georgeritchie/samplewebappwithmongodbanddocker?

georgeritchie/samplewebappwithmongodbanddocker is a sample asp.net application build by using mongoDb database server and docker.

https://github.com/GeorgeRitchie/SampleWebAppWithMongoDbAndDocker

# How to use this image?
## 1. Setup Docker
Make sure you have installed docker by running command "docker" in your command prompt. It must recognize it. 
If not installed, then install it:
* Windows: https://docs.docker.com/desktop/install/windows-install/ 
* Linux: https://docs.docker.com/engine/install/ubuntu/
* Mac: https://docs.docker.com/desktop/install/mac-install/

## 2. Create a folder for this app.
## 3. Create a compose file with following content:
Compose file example:
```
version: '3.4'

services:
  samplewebappwithmongodbanddocker:
    image: georgeritchie/samplewebappwithmongodbanddocker
    container_name: Server
    restart: always
    ports:
      - 5000:80
      - 5001:443
    depends_on: 
        - mongo

  mongo:
    image: mongo
    restart: always
    environment:
      MONGO_INITDB_ROOT_USERNAME: root
      MONGO_INITDB_ROOT_PASSWORD: example

  mongo-express:
    image: mongo-express
    restart: always
    ports:
      - 8081:8081
    environment:
      ME_CONFIG_MONGODB_ADMINUSERNAME: root
      ME_CONFIG_MONGODB_ADMINPASSWORD: example
      ME_CONFIG_MONGODB_URL: mongodb://root:example@mongo:27017/
```
Note that swagger is available from version 1.2.0.0 and higher. To switch on swagger add environment variable for this image:
```
samplewebappwithmongodbanddocker:
    image: georgeritchie/samplewebappwithmongodbanddocker
    container_name: Server
    restart: always
    ports:
      - 5000:80
      - 5001:443
    environment:
      - AddSwaggerToProduction=true  # to swich on swagger, otherwise set false
    depends_on: 
        - mongo
```
## 4. Onpen this folder in command prompt and run command 
```
docker-compose up -d
```
