# Casa de la Moneda — Palletizing Control System

Industrial palletizing control system developed for FNMT (Fábrica Nacional de Moneda y Timbre) operations.

## Overview

A Windows Forms application built in C#/.NET for managing and controlling automated palletizing processes in an industrial production line. The system includes monitor wrappers for hardware communication, catalog management, and transport line control.

## Project Structure

```
├── DlMonitorWrapper/     # C++/CLI wrapper for hardware monitor communication
├── GeneralMatrix/        # Matrix operations library
├── RCMCommonTypes/       # Shared type definitions
├── RCMGestorCatalogo/    # Catalog management module
├── RCM_Paletizado/       # Main palletizing control application
└── fnmt-paletizado/      # Windows Forms UI (access control, main interface)
```

## Tech Stack

- **Language:** C# (.NET Framework), C++/CLI
- **UI:** Windows Forms
- **Target:** Win32

## Requirements

- Visual Studio 2008+ (`.vcproj` format)
- .NET Framework
- Windows OS

## Getting Started

1. Clone the repository
2. Open the solution file in Visual Studio
3. Build the solution (`Ctrl+Shift+B`)
4. Run the main application from `RCM_Paletizado`

## License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

## Author

**Jose** — [@aifriend](https://github.com/aifriend)
