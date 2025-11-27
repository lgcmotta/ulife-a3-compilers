# MShell - A3 ULife Assignment

This project is the final assignment for my theory of computation and compilers class.

The complete assignment instructions are available in PDF format and only in Portuguese (Brazil). You can find it [here](assets/assingment.pdf).

[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=lgcmotta_ulife-a3-compilers&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=lgcmotta_ulife-a3-compilers) 
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=lgcmotta_ulife-a3-compilers&metric=bugs)](https://sonarcloud.io/summary/new_code?id=lgcmotta_ulife-a3-compilers) 
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=lgcmotta_ulife-a3-compilers&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=lgcmotta_ulife-a3-compilers) 
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=lgcmotta_ulife-a3-compilers&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=lgcmotta_ulife-a3-compilers)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=lgcmotta_ulife-a3-compilers&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=lgcmotta_ulife-a3-compilers)

## Installation 🤓

The application can be installed as a [dotnet global tool](https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools).

### Install the .NET 10 SDK 🔧

Download and install the .NET 10 SDK from the official website: [Download .NET 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

> ⚠️ Download the .NET installer based on your OS and architecture.

### Create a Classic GitHub Personal Access Token (PAT) 🔑

Although the repository is public, GitHub **always requires authentication** when you want to use a GitHub Packages feed (NuGet).  
You must create a **Personal Access Token (classic)** so the `dotnet` CLI can authenticate when adding the package source.

1. Open GitHub and go to **Settings**.
2. In the left sidebar, select **Developer settings**.
3. Click **Personal access tokens**.
4. Choose **Tokens (classic)**.
5. Click **Generate new token** → **Generate new token (classic)**.
6. Give the token a name, set an expiration date, and enable the following permission:

<img width="789" height="389" alt="image" src="https://github.com/user-attachments/assets/1b9855a7-4359-40aa-b63e-c72aa1d68183" />

7. Generate the token and copy the value (you will need it for the `dotnet nuget` commands).

### Register GitHub as a NuGet Source 📂

Locate your NuGet.config file:

| OS                   | NuGet Config File Location        |
|----------------------|-----------------------------------|
| Windows (PowerShell) | `$env:AppData\NuGet\NuGet.Config` |
| Windows (CMD)        | `%APPDATA%\NuGet\NuGet.Config`    |
| Linux                | ~/.nuget/NuGet/NuGet.Config       |
| OSX                  | ~/.nuget/NuGet/NuGet.Config       |

> ⚠️ The `NuGet.config` file name may appear as `NuGet.config` or `NuGet.Config`, depending on the OS.
> On Windows, you can access the directory using `$env:AppData` (PowerShell) or `%APPDATA%` (CMD).

Then run the following command, replacing:

- **<GITHUB_USERNAME>**: Your GitHub username
- **<GITHUB_PAT>**: The Personal Access Token you created
- **<NUGET_CONFIG_FILE_PATH>**: The path from the table above

#### Windows (PowerShell) 🪟

```pwsh
dotnet nuget add source --name github `
  --username <GITHUB_USERNAME> `
  --password <GITHUB_PAT> `
  --store-password-in-clear-text `
  --configfile <NUGET_CONFIG_FILE_PATH> "https://nuget.pkg.github.com/lgcmotta/index.json"
```

#### Linux/MacOS 🍎

```bash
dotnet nuget add source --name github \
  --username <GITHUB_USERNAME> \
  --password <GITHUB_PAT> \
  --store-password-in-clear-text \
  --configfile <NUGET_CONFIG_FILE_PATH> "https://nuget.pkg.github.com/lgcmotta/index.json"
```

### Install the Tool 🧰

With the GitHub NuGet source registered, you can now install the application as a global tool:

```bash
dotnet tool install --global MottaShell.A3.CommandLineInterface
```

After installation, the `msh` command becomes available system-wide. You can verify the installation with:

```bash
msh --version
```

## Usage 💻

The `run` command executes a program with the given file using the `--file` or the `-f` option.

The `repl` command starts the REPL (Read, Eval, Print, and Loop) tool, which evaluates code line by line.

The basic syntax is:

```bash
msh [COMMAND] [options]
```

The `run` commands support the following parameters:

| Option             | Required  | Description                                 |
|--------------------|-----------|---------------------------------------------|
| `-f`  or `--file`  | **true**  | File path containing the `.ms` script file. |