# WebBlocker CLI 🚫🌐

A lightweight, efficient C# tool designed to manage local website blocking by synchronizing a custom list with the Windows `hosts` file. This project demonstrates safe system-level file manipulation and automated administrative privilege management.

## 🚀 Key Features

* **Smart Host Synchronization:** Implements a "Filter and Merge" algorithm. It uses custom markers (`# BLOCK_START` / `# BLOCK_END`) to safely inject or remove entries without touching existing system configurations.
* **Persistent Configuration:** Automatically manages a `block.sites.data` file located within the system's network drivers directory to ensure settings persist regardless of where the executable is located.
* **Automatic Elevation:** Includes an integrated `app.manifest` to ensure the application always requests the necessary Administrative privileges to modify system files.
* **Dynamic Feedback:** Features a real-time feedback loop that provides immediate confirmation after adding, deleting, or synchronizing sites.

## 🛠️ Technical Stack

* **Language:** C# 12
* **Framework:** .NET 8.0 (Cross-platform Core)

## 📂 Project Structure

* **`Program.cs`**: Contains the main execution loop and user interface logic.
* **`FileManager.cs`**: The core logic engine for reading `block.sites.data` and performing the "Clean & Merge" on the system `hosts` file.
* **`SystemPaths.cs`**: Centralizes all critical system paths and ensures consistent naming for the configuration file.

## ⚙️ Usage

1. **Build the Project:** Open the solution in Visual Studio and build the solution.
2. **Run as Administrator:** Upon launching the `.exe`, Windows will prompt for administrative access. This is required to write to the `drivers/etc/` folder.
3. **Operations:**
* **Add/Delete:** Modify your local list.
* **Apply Changes:** This specific step triggers the synchronization between your `block.sites.data` and the Windows `hosts` file.

## 🛡️ Safety

This application is designed to be "non-destructive." It only modifies content within its own `# BLOCK` tags, ensuring the rest of your Windows networking settings remain untouched.
