<div align="center">

<img src="Assets/ISM+.png" alt="ISM+ Logo" width="100"/>

# ⚡ ISM+: Internet Speed Meter Plus

**Know the Limits. Free, Native, and Open Source.**

[![Platform](https://img.shields.io/badge/Platform-Windows%2010%2F11-0078D7?logo=windows)](https://github.com)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com)
[![Microsoft Store](https://img.shields.io/badge/Microsoft%20Store-ISM%2B%20Pro-orange?logo=microsoftstore)](https://apps.microsoft.com/detail/9ncl3jrpkhtw)

</div>

---

## 📖 Overview

**ISM+** is a lightweight, strictly native, and persistent network throughput monitor built for Windows 10 and 11. It runs as a system tray application, providing a real-time, low-overhead view of download and upload speeds, daily data consumption, and historical statistics, all surfaced through a single, clean dashboard.

**Filling a Gap in the Ecosystem:** The existing landscape of Windows network monitors is largely inadequate. Available tools are either non-functional, locked behind aggressive paywalls, or closed-source with opaque background processes. **ISM+** addresses this by delivering a fully functional, performance-optimized, and natively designed solution that operates with minimal system resource overhead.

**Privacy by Design:** ISM+ is built with a strict offline-only architecture. It makes no outbound network calls, collects no telemetry, and does not transmit or store any user data to external systems.

This repository contains the **Free (open-source)** edition. A [**Pro edition**](https://apps.microsoft.com/detail/9ncl3jrpkhtw) is available on the Microsoft Store for users requiring extended telemetry and monitoring capabilities.

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

### Option 1: Portable (No Install)
1. Download `ISM+.exe` from the Releases section.
2. Double-click to run immediately, no installation required.

### Option 2: Installer (Inno Setup)
1. Download `ISM_Plus_Setup.exe` from the Releases section.
2. Run the installer to complete setup automatically.

### Option 3: Microsoft Store
[![Microsoft Store](https://img.shields.io/badge/Microsoft%20Store-ISM%2B%20-green?logo=microsoftstore)](https://apps.microsoft.com/detail/9nqk8n6m1qz5)

---

## 🛡️ Administrative Privileges

The Free edition of **ISM+** requires no administrative privileges for its core monitoring functionality.

Advanced system-level features, such as the **Real-Time App Bandwidth Tracker**, interface directly with the Windows Event Tracing (ETW) subsystem and require elevated permissions. These features are available exclusively in **ISM+ Pro**.

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

**[Get ISM+ Pro on the Microsoft Store →](https://apps.microsoft.com/detail/9ncl3jrpkhtw)**

ISM+ Pro unlocks the full capability of the monitoring stack:

- 🪟 **Floating Widget**: A customizable, semi-transparent desktop overlay for persistent speed readouts
- 📊 **Real-Time Graphs**: Continuous throughput graphs with historical data retention
- 🔍 **Per-App Monitor**: Process-level bandwidth attribution via Windows ETW instrumentation
- ⏱️ **In-App Speed Test**: Integrated ping, download, and upload throughput benchmarking
- 📈 **Advanced Stats**: Average speed computation alongside monthly and yearly peak tracking
- 🔄 **Taskbar Flip View**: Toggle the taskbar overlay between live speed and cumulative data usage

---

## ☕ The Story Behind ISM+

ISM+ was built to solve a straightforward problem: there was no reliable, native Windows application that could display real-time network speeds in the taskbar. The available options were either non-functional, locked behind subscription paywalls, or both.

On **February 28, 2026**, I started building one from the ground up.

Development took approximately three months: designing and iterating on the UI, writing the core C# network telemetry layer, stabilizing behavior across different Windows configurations, and running iterative builds through real-world testing with a group of friends. A significant portion of the engineering effort went into ensuring the app integrates cleanly with the Windows shell without introducing noticeable overhead.

The Pro version helps sustain continued development, but the Free edition is intentionally scoped to cover the core use cases most users require.

If you find ISM+ useful, please star the repository and share it with fellow Windows power users. ❤️

---

<div align="center">
Made with ❤️ for Windows power users.
</div>