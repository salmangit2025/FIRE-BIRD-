# FIRE BIRD 🔥🐦

A small 2D shooting game for Windows desktop, built with **C#**, **WinUI 3**, and **XAML**.

## Features
- Moving firebird target with changing direction
- Mouse aiming reticle
- Click to shoot
- Score increases by 10 for a hit
- Three lives; a missed shot uses one life
- Target speed increases as your score grows
- Game Over screen and restart button
- Dark, responsive interface

## Requirements
- Windows 10 version 1809 or later (Windows 11 recommended)
- Visual Studio 2022
- **.NET desktop development** workload
- **Windows App SDK / WinUI application development** components

## Run the project
1. Extract the ZIP file.
2. Open `Fire_Bird.sln` in Visual Studio 2022.
3. Allow Visual Studio to restore NuGet packages.
4. Set `Fire_Bird` as the startup project.
5. Select `x64` and press **F5**.

If Visual Studio asks to install or update the Windows App SDK, accept the prompt.

## Controls
- Move the mouse to aim.
- Click inside the game area to shoot.
- Hit the bird to gain 10 points.
- A miss costs one life.
- Select **Restart Game** to start again.

## Upload to GitHub
1. Create a new repository on GitHub, for example `Fire-Bird-Game`.
2. Extract this ZIP folder.
3. Upload the project files and folders to the repository, or use Git:

```bash
git init
git add .
git commit -m "Initial Fire Bird game project"
git branch -M main
git remote add origin https://github.com/YOUR-USERNAME/Fire-Bird-Game.git
git push -u origin main
```

Replace `YOUR-USERNAME` with your GitHub username.

## Project structure

```text
Fire_Bird_GitHub_Project/
├── Fire_Bird.sln
├── Fire_Bird.csproj
├── App.xaml
├── App.xaml.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── app.manifest
├── .gitignore
└── README.md
```

## Notes
This is a clean, self-contained version of the Fire Bird game based on the features described for the project. It does not include the older custom image/audio files (`Bird1.png`, `Shoot.mpeg`, `Reload.mpeg`); the target is drawn as an emoji so the project can be shared without extra assets.
