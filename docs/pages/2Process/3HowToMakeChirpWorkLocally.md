% There has to be some documentation on how to come from cloning your project to a running system. That is, Adrian or Helge have to know precisely what to do in which order. Likely, it is best to describe how we clone your project, which commands we have to execute, and what we are supposed to see then. 

## Comprehensive guide to run the program locally

Please make sure you have all the right ***.Net 8*** dependencies installed [here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

### How to start the project on localhost via releases

1. Download the newest release for your operating system [here](https://github.com/ITU-BDSA2024-GROUP13/Chirp/releases).  

2. Unzip the file, and navigate to the folder.  

3. Run `$ dotnet Chirp.Web.dll `  

4. Look in your terminal for which port the project is listening on. e.g.  
![alt text](../../images/image.png)

5. Open your browser and type `http://localhost:<port>`  

### How to start the project via cloning the repository

1. Open your terminal and type `$ cd`

2. Clone the repository and type `$ git clone https://github.com/ITU-BDSA2024-GROUP13/Chirp.git`

3. Type `$ cd ./Chirp`

4. Run the program `$ dotnet watch --project ./src/Chirp.Web`

5. Look in your terminal for which port the project is listening on. e.g.  
![alt text](../../images/image.png)
