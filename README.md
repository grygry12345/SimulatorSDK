# Covisart Motion Simulator SDK for Unity Game engine 

[![N|Solid](https://raw.githubusercontent.com/COVISART/SimulatorSDK/master/LogoStylesSideBySide.png)](https://unity.com/)

[![Build Status](https://travis-ci.org/COVISART/SimulatorSDK.svg?branch=master&status=pass)](https://travis-ci.org/github/COVISART/SimulatorSDK)

Vehicle simulator based on a motion system developed using virtual and augmented reality technologies. System supports multi axis motion systems. This simulator allows users to use the aircraft they want in a virtual environment. Using AR technology, user can interact with virtual cockpit. Unlimited rotation system provide most realistic flight experience.


  - Virtual Reality
  - Augmented Reality
  - High Precision Motion System 

# Features!

  - Control power situation of all connected motors.
  - Calibrate motion system with one function
  - Calibrate each axis one by one 
  - Send axis data to system on each frame
  - Reset errors on motors at runtime

You can also:
  - Manage joystcik inputs
  - Manage VR headset data to match exact position when rotating system
  - Export documents as Markdown, HTML and PDF

You can find example codes and scene one folders

> We use Resharper when coding,
> Resharper formatting syntax is to make it as readable
> as possible. 

### Tech

Motion Simulator uses a number of open source projects to work properly:

* [TwinCAT 3](https://www.beckhoff.com/TwinCAT3/) - eXtended Automation (XA) to control sensors and motors
* [Visual Studio Professional](https://visualstudio.microsoft.com/vs/professional/) - integrated development environment
* [Angular](https://angular.io/) - a platform for building mobile and desktop web applications.
* [node.js] - evented I/O for the backend

And of course Covisart Motion Simulator itself is open source with a [public repository][dill]
 on GitHub.

### Installation

Covisart Motion Simulator requires [Unity 2019 LTS](https://unity.com/releases/2019-lts?_ga=2.266885710.1795603025.1592424756-2133270114.1577918347) to run.

Download repo and import [CovisartMotionSDK](https://github.com/COVISART/SimulatorSDK/tree/master/Assets/CovisartMotionSDK) and [Plugins](https://github.com/COVISART/SimulatorSDK/tree/master/Assets/Plugins) to your project.

For easy setup, download Unity package from [release](https://github.com/COVISART/SimulatorSDK/releases) and import it.

### Todos

 - Write MORE Tests
 - Add communication control mode
