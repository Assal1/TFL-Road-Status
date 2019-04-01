# TFL Coding Challange

Follow the below steps to build, run and test the road status application.

#  How to build the code

  - Open the solution file 'RoadStatus.sln'.
  - Right click from the solution file and select the option 'Rebuild Solution'

# How to run the output

### Prerequisites
Update AppId and AppKey in the RoadStatus.ConsoleApp application
 Go to App.config in the RoadStatus.ConsoleApp project and update the AppId and AppKey to get the status of the road.

 ### Run the output from 'Windows Poweshell'

  - Open 'Windows Powershell'.
  - Navigate to the console app exe using the command 'pushd file path where the exe file is present'
  ```sh
  Example:
pushd "C:\Development\TFL\RoadStatus\RoadStatus.ConsoleApp\bin\Debug"
```
  - Execute the valid road status scenerio using the command console app's exe file using the command '.\RoadStatus.ConsoleApp.exe Valid Road Name'
  ```sh
  Example:
.\RoadStatus.ConsoleApp.exe A2
```
  - Expected output for the valid road name scenerio
  ```sh
  Output:
The status of road A2 is as follows:
Display Name: A2
Road Status: Closure
Road Status Description: Closure
```
- To know the last status code, run the command 'echo $lastexitcode'
- The result code should be 0 for valid road name and 1 for in-valid road name.
```sh
Example:
$echo lastexitcode
```
```sh
Output:
0
```
 - Execute the invalid road status scenerio using the command console app's exe file using the command '.\RoadStatus.ConsoleApp.exe invalid road name'
  ```sh
  Example:
.\RoadStatus.ConsoleApp.exe A233
```
  - Expected output for the in-valid road name scenerio
  ```sh
A233 is not a valid road.
```
  - To know the last status code, run the command 'echo $lastexitcode'
- The result code should be 0 for valid road name and 1 for in-valid road name.
```sh
Example:
$echo lastexitcode
```
```sh
Output:
1
```
# Test Automation with Specflow and MSTest

### Prerequisites
Install Extension for specflows
 Go to Tools => Extensions and Updates => select Online tab => search for “Specflow” , it will show the extension and you can install it .

On the Build menu, choose Build Solution.

If there are no errors, Test Explorer appears with Road Status listed in the Not Run Tests group.

> Tip 1:
>
>If Test Explorer does not appear after a successful build, choose Test on the menu, then choose Windows, and then choose Test Explorer.



Right click on the feature 'Road Status', to run all the tests ,  selected test or debug it by adding break point to the step definition file.