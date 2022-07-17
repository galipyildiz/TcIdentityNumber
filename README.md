<p align="center">
<b>Check TC Identity Number</b>
<br />
<img src="./identityChecker.png"/>
<br />
Verify TC Identity Number the easiest way
</p>

----
### How to use?
- Install nuget package
  - Package Manager Console
    ```
      Install-Package TcIdentityNumber -Version 1.0.0 
    ``` 
  - Nuget Package Solution
    ![](./assets/installnugetpackage.png)
    Select this package and install your project.
- Enjoy your verify identity number 
    - .Net 6.0 Console Application
    ```cs
    using TCIdentityNumber;

    long identityNumber = 1111111111;
    Console.WriteLine(identityNumber.IsIdentityNumberCorrect());
    Console.WriteLine(await identityNumber.IsIdentityNumberCorrectAsync("Ali", "Veli", 2022));
    ```
    - Output
    ```
    False
    False
    ```

**Note:** If you are correct information, you can reach the correct results. :)