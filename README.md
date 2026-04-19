# DocViewer

A lightweight desktop document viewer and editor built with .NET (WPF).

## 📌 Overview

DocViewer is a simple, fast tool for opening and viewing different file types through a unified interface.
It converts supported formats to HTML and renders them using an embedded browser, keeping the implementation minimal and easy to maintain.

The project is designed for personal use, focusing on simplicity, extensibility, and low maintenance.

---

## 🧭 How to Use

### 📂 Opening a File

You can open a file in two ways:

* Use the menu: **File → Open**
* Drag and drop a file directly into the application window

---

### ✏️ Editing

* Click the **Edit** button to toggle **Edit Mode**
* Editing is currently supported for:

  * `.txt`
  * `.csv`

---

### 💾 Saving

* Press **Ctrl + S**
* Or use the menu: **File → Save**

---


## ✨ Features

* Open and view multiple file types:

  * `.docx` (rendered as HTML)
  * `.txt`
  * `.csv`
  * `.png`, `.jpg`

* Edit support:

  * `.txt` → plain text editor
  * `.csv` → table/grid editor

* Drag & drop file opening

* Toggle between **View** and **Edit** mode

* Save changes (`Ctrl + S`)

* Adapter-based architecture (easy to extend with new formats)

---

## 🧱 Architecture

* **DocViewer (WPF)** → UI layer
* **DocViewer.Core** → adapters, services, logic
* **DocViewer.Tests** → unit tests

The app uses an adapter pattern:

```
File → Adapter → HTML → WebView2
```

---

## 🚀 Getting Started

1. Clone the repository
2. Build the solution
3. Run the WPF project (`DocViewer`)

---

## 🧪 Testing

Unit tests are included for core logic and adapters.

Run tests via:

```
Test Explorer → Run All
```

---

## 📎 Notes

* Designed for personal use
* Focused on simplicity over full feature parity with professional editors
* Easily extendable by adding new adapters

---

## 🔮 Future Ideas

* Additional file formats (PDF, Markdown)
* Improved CSV parsing
* Search within documents
* Recent files / history
* Save As (saving in a different file)

---

---

## 🔮 Known issues

* Drag & Drop multiple times doesn't work (Windows default 'open file' behavior kicks in)
* 

---
