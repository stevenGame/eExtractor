## eExtractor libary


### How to Install

Open Nuget package manager console execute follow up command
```cmd
$ Install-Package DeftSoft.eExtractor
```
### Usage 
call parser from dictionary
```csharp
class Foo {
    public string Foo {get;set;}
    public string Bar {get;set;}
}
 var foo = RegexParser.Build(new Dictionary<string, string> {
                { "Foo", @"regexFoo" },
                {"Bar", @"regexBar" }
            }).Parse<Foo>(text);

```
call parser from file
```csharp

```
### How to upload to Nuget
* Change version number in **eExtractor.nuspec** file
* Package
    ```cmd
    $ ..\tools\nuget.exe pack eExtractor.csproj
    ```
* Upload to Nuget 
  If there are any error check is the API key expire or not
    ```cmd
    $ ..\tools\nuget.exe push DeftSoft.eExtractor.0.1.0.nupkg 76420682-1c39-4658-a848-0787c42947eb -Source https://www.nuget.org/api/v2/package
    ```

### TODO

- [x] change entity to generic function if need support other type
 support generic save with different entity  
- [ ] make ISavable support multiple primary key and other name of ID  
- [ ] Add Excel parser and mapper  
- [ ] Add word parser and mapper  
