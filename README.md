<div align="center">

<img src="Assets/ISM+.png" alt="ISM+ Logo" width="100"/>

# ⚡ ISM+ — Internet Speed Meter Plus

**Know the Limits. Free, Native, & Open Source.**

[![Platform](https://img.shields.io/badge/Platform-Windows%2010%2F11-0078D7?logo=windows)](https://github.com)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com)
[![Microsoft Store](https://img.shields.io/badge/Microsoft%20Store-ISM%2B%20Pro-orange?logo=microsoftstore)](https://apps.microsoft.com/detail/9ncl3jrpkhtw)

</div>

---

## 📖 Overview

**ISM+** is a lightweight, strictly native, and always-on network speed monitor designed entirely for Windows 10 and 11. It lives elegantly in your system tray, providing a real-time, zero-latency view of your download/upload speeds, daily data usage, and historical stats—all in one clean dashboard.

**A First of Its Kind:** Currently, the landscape for Windows network monitors is broken. Existing products are either completely non-functional, aggressively monetized behind expensive paywalls, or closed-source with questionable background processes. **ISM+ is different.** It bridges the gap by offering a fully functional, highly optimized, and beautifully designed native solution that respects your system resources.

**Privacy First:** Your data is your own. ISM+ is strictly privacy-focused—it operates entirely offline, does not track you, and will never steal, harvest, or sell your telemetry data.

This repository contains the **Free (open-source)** edition. A [**Pro edition**](https://apps.microsoft.com/detail/9ncl3jrpkhtw) is available on the Microsoft Store for users who want to push their telemetry tracking even further.

---

### 📸 Screenshots

| Home Tab | Widgets |
| :---: | :---: |
| <img src="Assets/Home.png" width="400" height="250" alt="Screenshot 1"/> | <img src="Assets/Widget.png" width="400" height="250" alt="Screenshot 2"/> |
| **Monitoring Active** | **Monitoring Inactive** |
| <img src="Assets/Monitoring_Active.png" width="400" height="250" alt="Screenshot 3"/> | <img src="Assets/Monitoring_Inactive.png" width="400" height="250" alt="Screenshot 4"/> |
| **Graph Tab** | **Settings** |
| <img src="Assets/Graph.png" width="400" height="250" alt="Screenshot 5"/> | <img src="Assets/Settings.png" width="400" height="250" alt="Screenshot 6"/> |

---

## ✨ Features Breakdown

| Feature | Free | Pro |
|---|:---:|:---:|
| Real-time download / upload speed display | ✅ | ✅ |
| System tray icon with live speed | ✅ | ✅ |
| Taskbar overlay speedometer | ✅ | ✅ |
| Daily data usage stats | ✅ | ✅ |
| Max speed tracking (Daily) | ✅ | ✅ |
| Dark / Light theme | ✅ | ✅ |
| Multiple speed units (KB/s, MB/s, Mbps, …) | ✅ | ✅ |
| Run at startup | ✅ | ✅ |
| **In-app speed test (Ping / DL / UL)** | 🔒 | ✅ |
| **Per-app bandwidth monitor** | 🔒 | ✅ |
| **Real-time speed graph** | 🔒 | ✅ |
| **Average speed tracking** | 🔒 | ✅ |
| **Floating desktop widget** | 🔒 | ✅ |
| **Taskbar Overlay Flip (Data usage view)** | 🔒 | ✅ |

> 🔒 **Locked features** are available in [ISM+ Pro](https://apps.microsoft.com/detail/9ncl3jrpkhtw) on the Microsoft Store.

---

## 🚀 Installation

### Option 1 — Portable (No Install)
1. Download `ISM+.exe` from the Releases Section.
2. Double-click to run instantly.

### Option 2 — Installer (Inno Setup)
1. Download `ISM_Plus_Setup.exe` from the Releases Section.
2. Run the installer — it handles everything automatically.

### Option 3 — Microsoft Store
*Free version Store link coming soon!*

---

## 🛡️ Administrative Privileges

The Free edition of **ISM+** is designed to be incredibly lightweight and does not require administrative privileges for its core functionality. 

For advanced system-level features, such as the **Real-Time App Bandwidth Tracker** (which uses Windows ETW architecture), you must upgrade to **ISM+ Pro**.

---

## 📁 Repository Structure

```text
ISM+/
├── Assets
├── README.md
└── Source
    ├── app.manifest
    ├── App.xaml
    ├── App.xaml.cs
    ├── AssemblyInfo.cs
    ├── Icon1.ico
    ├── MainWindow.xaml
    ├── MainWindow.xaml.cs
    ├── NetMonitor.csproj
    ├── nuget.config
    ├── TaskbarOverlayWindow.xaml
    └── TaskbarOverlayWindow.xaml.cs
```

---

## 🌟 ISM+ Pro

Want the **Floating Desktop Widget** and **Taskbar Overlay Speedometer**?

**[Get ISM+ Pro on the Microsoft Store →](https://apps.microsoft.com/detail/9ncl3jrpkhtw)**

ISM+ Pro unlocks the full potential of your network monitoring:
- 🪟 **Floating Widget** — a customisable semi-transparent speed widget on your desktop
- 📊 **Real-Time Graphs** — beautiful, smooth graphs tracking historical speed
- 🔍 **Per-App Monitor** — track exactly which apps are consuming your bandwidth
- ⏱️ **In-App Speed Test** — ping, download, and upload benchmarks
- 📈 **Advanced Stats** — average speeds and monthly/yearly maximums
- 🔄 **Taskbar Flip View** — tap the taskbar overlay to see total data used

---

## ☕ The Story Behind ISM+

The idea for ISM+ was born out of sheer frustration. I searched the entire internet for a simple, native Windows app that could show me my real-time network speeds directly in the taskbar. What I found was a wasteland—existing apps were either completely non-functional, hidden behind ridiculous subscription paywalls.

So, on **February 28, 2026**, I decided to build it myself.

It took me about 3 months of dedication to build this completely from scratch. It started with just a logo (which I redesigned three times before getting it right), writing lines of core C# telemetry code, debugging, making it perfectly stable across different Windows environments, and constantly sending fresh builds to my friends for rigorous testing. A massive amount of hard work went into making this feel like a seamless part of the Windows OS.

While there is a paid Pro version (which helps support the endless hours of development!), I intentionally designed the Free, open-source version to beautifully handle all the essential tasks most users need.

Please shower your love and support for this app, star the repository, and share it with fellow Windows power users! ❤️

---

<div align="center">
Made with ❤️ for Windows power users.
</div>
