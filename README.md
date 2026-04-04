# LibreCare

LibreCare is a desktop application designed for individuals, families, and
caregivers who manage the health of loved ones.

It helps track doctors, appointments, conditions, medications, and other important information, making caregiving simpler and more organized.

By consolidating critical health data in one place, LibreCare reduces stress and
ensures nothing important is overlooked.

## 🚧 Status
Early development — first feature slice in progress.

---

## 🧠 Overview
This project is a locally-run application built using ASP.NET Razor Pages.
Although it uses web technologies under the hood, it is designed to behave like a desktop application.

Planned characteristics:
- Runs locally (no external hosting required)
- Uses SQLite for lightweight, file-based storage
- May be packaged with Electron.NET for a desktop-like experience
- Potential future distribution via MSI installer

---

## 📸 Screenshots (Mockups)
> Add your mockups here early — this is a big win for clarity

You can include screenshots like this:

![Screenshot 1](docs/screenshots/screenshot1.png)
![Screenshot 2](docs/screenshots/screenshot2.png)

_These are initial mockups and may not reflect the current implementation._

---

## ✨ Goals
- Provide a fast, simple local tool for [your use case here]
- Avoid complex deployment or cloud dependencies
- Keep everything self-contained and easy to install

---

## 🛠 Tech Stack
- ASP.NET Core Razor Pages
- SQLite
- (Planned) Electron.NET
- (Planned) Windows MSI installer

---

## 🚀 Getting Started

### Prerequisites
- .NET SDK (version X.X or later)

### Run locally
```bash
git clone https://github.com/your-username/your-repo.git
cd your-repo
dotnet run
```

Then open your browser to:

```bash
http://localhost:5000
```

---

## **PART 3 — Rest of README**
```markdown
## 📦 Project Structure (early)

/Pages -> Razor Pages UI
/Data -> Database context and models
/wwwroot -> Static assets

---

## 🗺 Roadmap (rough)
- [ ] First working feature ("first slice")
- [ ] Basic UI layout
- [ ] SQLite integration
- [ ] Local data persistence
- [ ] Electron.NET wrapper
- [ ] MSI installer packaging

---

## ⚠️ Notes
- This is an early-stage project — expect breaking changes
- Structure and tooling may evolve significantly

---

## 📄 License
[Choose a license or leave this blank for now]
