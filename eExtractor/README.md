## eExtractor libary


### How to Install

Open Nuget package manager console execute follow up command
```cmd
$ Install-Package DeftSoft.eExtractor
```
### Usage 
call parser from dictionary
```csharp
 var f1099k = RegexParser.Build(new Dictionary<string, string> {
                { "TaxYear", @"^\d{4}\s+(.*\n){2,6}(Form\s+1099.*-.*K)\\,^\d{4}.*$" },
                {"TaxID", @"PAYEE'S taxpayer identification no(.*\n){2,3}^\d{9}.*$\\,^\d{9}.*$" },
                {
                    "FederalID",
                    @"FILER'S federal identification no(.*\n)+(\d{2}-\d{7}(.*\n))PAYEE'S taxpayer identification no\\,\d{2}-\d{7}(.*$)"
                },
                {"AccountNum", @"Account number.*\n(.*\n){3,5}(\d{16}).*\\,^(\d{16}).*$" },

            }).Parse<Pdf1099K>(text);

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

[x] change entity to generic function if need support other type
 support generic save with different entity  
[o] make ISavable support multiple primary key and other name of ID  
[o] Add Excel parser and mapper  
[o] Add word parser and mapper  
