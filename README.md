# Galaxy maze
> ℹ️ This project was carried out collaboratively as part of a _Team Engineering Project_ course and within the scope of individual bachelor theses.
> My specific contribution to the project involved the design and implementation of a web-based management panel and back-end for this game-based
> learning platform, as described in the bachelor thesis.

[![BtnDemo]](https://yarde.github.io/)
[![BtnThesis]](https://drive.google.com/file/d/16yDk2jfA5EeV-m91dPVTmnt_Kdyvu6MT/view?usp=sharing)

## About
The aim of the project was to create a generic (not related to any school subject) game-based educational platform incorporating adaptive teaching
approach (the difficulty of the game adjusts itself to students' results in the game in real time).

Functions include:
- manage classes:
    - add/modify/remove class,
    - add/modify/remove students;
- manage game scenarios:
    - create/delete scenario,
    - upload plain text files for AI to generate questions based on them,
    - add/modify/remove questions,
    - accept/reject question difficulty proposed by AI,
    - accept/reject questions generated by AI;
- manage sessions:
    - assign a session to a class,
    - track the status of sessions assigned to particular students.
 
Diagrams and technical details are located in [the bachelor thesis](https://drive.google.com/file/d/16yDk2jfA5EeV-m91dPVTmnt_Kdyvu6MT/view?usp=sharing).

## Technology used
- [C#](https://learn.microsoft.com/en-us/dotnet/csharp/),
- [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-7.0),
- [Entity Framework](https://learn.microsoft.com/en-us/ef/),
- [PostgreSQL](https://www.postgresql.org/),
- [Vue](https://vuejs.org/),
- [Materialize CSS](https://materializecss.com/)

## Screenshots
> ℹ️ As unity game was not created by me, but by [Dawid Kaluża](https://github.com/Yarde) it is not shown here. This and other games created by him
> are available [here](https://yarde.itch.io/).

![image](https://github.com/Baciejowski/educational-platform/assets/64860051/0021d595-c039-4db5-a87d-68524f31758d)
![image](https://github.com/Baciejowski/educational-platform/assets/64860051/8923b0a5-613d-4ad0-90a4-86d09d067c82)
![image](https://github.com/Baciejowski/educational-platform/assets/64860051/84fde7bd-6bf1-47b3-8c1f-14e290687282)
![image](https://github.com/Baciejowski/educational-platform/assets/64860051/5e06710b-2d41-47ec-8e6c-bc1e75bd66e3)
![image](https://github.com/Baciejowski/educational-platform/assets/64860051/56359bd6-ca0d-4a49-b3c1-8b3f24863de1)
![image](https://github.com/Baciejowski/educational-platform/assets/64860051/689ea676-db4c-471e-bd17-76c36ed381c9)

[BtnThesis]: https://img.shields.io/badge/Read%20thesis-50b59d?style=for-the-badge&logoColor=white&logo=googledrive
[BtnDemo]: https://img.shields.io/badge/Play%20demo-8AA0BE?style=for-the-badge&logoColor=white&logo=unity
