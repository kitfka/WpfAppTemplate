# WpfAppTemplate
Why? why is this here.

I have used wpf a few times. Every time i learn something new. Another improvement to make to the structure etc.

But porting all the code to a new project is starting to take a lot of time. So lets try to use a template!

## How to install from source

clone and in the folder that contains the solution file use the command

`dotnet new -i .`

To uninstall run 
`dotnet new -u`

And check what to use to install the 

It might be something like:
`dotnet new --uninstall C:\Users\UserName\source\repos\WpfAppTemplate`



## How to use

Run:
`dotnet new -u WPF-FEY`

If your projectname contains a space for example "Another Test" redo the project reference from Another Test to Another Test.Core!
<ProjectReference Include="..\Another Test.Core\Another Test.Core.csproj" />