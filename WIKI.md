# How to build and test PSReadLine locally

Contributors to PSReadLine (PSRL) need to build and run the project locally.
This page outlines some common strategies to run a custom build of PSRL on your own local machine.

// https://github.com/PowerShell/PSReadLine/issues/2912

## Using MockPSConsole

This repository comes with a simple emulated console project named `MockPSConsole` that you can run to quickly perform some cursory checks.

To run `MockPSConsole` simply build the solution and run the resulting executable.

- Clone the GitHub repository
- Build the solution using `./build.ps1`
- Open the solution in Visual Studio
	- Set the `MockPSConsole` projects as a Startup Project
	- Press F5 or select `Debug|Start debugging`

This is the quickest and easiest way to test a new feature in a custom PSRL build. However, it suffers from some limitations.

> *Limitations*: Cannot be launched from Visual Studio Code.  
*Limitations*: Does not have any customizations - shortcuts or prompts.  
*Limitations*: As `MockPSConsole` is an emulation, the resulting behaviour may differ from running with different actual terminals.

The following two other strategies are a bit more involved but give better fidelity when testing a custom PSRL build. Both strategies, however, require administrative privileges.

## Replacing the builtin instance of PSRL with a custom build

This strategy consists in replacing the actual binaries from the builtin instance of PSRL with ones that you build locally. This means that you can use your own local version that will be picked up across all terminal environments.

> *Limitations*: requires administrative privileges.  

*Warning*: make sure to backup existing files to support reverting the changes and using the builtin version of PSRL.

## Running the builtin instance of PSRL side by side with a custom build

This strategy enables to run both instances side by side. It means that you can pick and choose which instance you want to run. This strategy is best suited if you have long held contributions to the project but want to keep the existing instance running most of the time.

> *Limitations*: requires administrative privileges.
