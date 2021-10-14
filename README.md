# dotnet-neater

**dotnet-neater** is an opinionated code formatter for C# inspired by [Prettier](https://prettier.io/), that can be installed and run as a .NET CLI tool.

## How it works
When invoked, dotnet-neater will discover all the C# files to be formatted, and will do the following for each one:
- Parse the source code into a syntax tree using the [Roslyn](https://github.com/dotnet/roslyn) compiler
- Turn the syntax tree into an intermediate "layout" representation, which specifies how the code should be formatted
- Print the layout representation to a string, according to built-in rules and a handful of optional configuration options
- Write the formatted code back to the source file (if it was changed)

## Installation
The recommended approach is to install dotnet-neater as a local .NET CLI tool in the root of your repository:
```powershell
# Navigate to the root of your repository
cd "./MyApp"

# Create a tool manifest file at `/MyApp/.config/dotnet-tools.json`
dotnet new tool-manifest

# Install dotnet-neater as a local tool
dotnet tool install dotnet-neater
```

Once dotnet-neater has been added to the tools manifest file, you can install it on a fresh clone of the repository by running the following from the root of your repository:
```powershell
dotnet tool restore
```

Note that the manifest file (`dotnet-tools.json`) encodes the version of dotnet-neater being used. This is useful as it ensures consistency across developers working on the same repository.

To update to the latest version of dotnet-neater, simply run the following from the root of your repository:
```powershell
dotnet tool update dotnet-neater
```

If you wish to uninstall dotnet-neater, you can do so by running the following from the root of your repository:
```powershell
dotnet tool uninstall dotnet-neater
```

## Layout Representation
The intermediate "layout" representation of C# code is formed by combining the following primitive **Operations**:
- QQ
