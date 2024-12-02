# XBatteryMonitor (XBM)

[![.NET 8](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![GitHub release](https://img.shields.io/github/v/release/dikayx/xbatterymonitor)](https://github.com/dikayx/xbatterymonitor/releases)

XBM is a small companion app for Windows that monitors the battery level of your gamepad and sends a notification when it's low. It's designed to work with the Xbox controller, but supports any gamepad that uses the XInput API. Basically it fulfills the job that the Xbox Accessories app should have done.

> **Note**: This project is in its early stages and may contain bugs. Please report any issues you encounter.

## Features

-   **Low battery notification**: Get a notification when your gamepad's battery is low.
-   **Customizable**: Set the battery level and interval at which the notification is triggered.
-   **Lightweight**: Consumes very little resources.

## Installation

It's simple! Download the latest release from the [releases page](https://github.com/dikayx/xbatterymonitor/releases) and run the installer.

### Requirements

-   Windows 10 or later
-   [.NET 8 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)

## Usage

By default, XBM will start with Windows and run in the background. You can access the settings by right-clicking the tray icon.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.