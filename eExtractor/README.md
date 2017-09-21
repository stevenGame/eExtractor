## eExtractor libary


### How to upload to Nuget
* Package
    ```cmd
    $ ..\tools\nuget.exe pack eExtractor.csproj
    ```
* Upload to Nuget 
  If there are any error check is the API key expire or not
    ```cmd
    $ ..\tools\nuget.exe push DeftSoft.eExtractor.0.1.0.nupkg 76420682-1c39-4658-a848-0787c42947eb -Source https://www.nuget.org/api/v2/package
    ```