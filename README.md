# Xbox Live Lite 🎮

**Scalable live-service backend for multiplayer games**  
A realistic simulation of **Xbox Live + Azure PlayFab** built from scratch.

This project showcases strong backend engineering skills for gaming, distributed systems, and Azure. Perfect for portfolio and internal mobility toward **Xbox Live Platform**, **Azure Gaming**, or Product Engineering roles in Microsoft Gaming.

---

## ✨ Key Features

- **Authentication** – Player login with JWT + gamertag profiles (level, XP)
- **Matchmaking** – Smart queue by skill/party + automatic session creation
- **Session Management** – Lobbies, join/leave, states (lobby → in-game → finished)
- **Leaderboards** – Global and mode-specific rankings with optimized queries
- **Real-time Telemetry** – Game events + live WebSocket notifications with SignalR
- **Cloud Saves** – Save and load player progress (bonus)
- **Full Observability** – Application Insights + performance metrics

---

## 🛠️ Tech Stack

| Layer             | Technology                          |
|-------------------|-------------------------------------|
| Backend           | .NET 8 Minimal API                  |
| Database          | Azure Cosmos DB (NoSQL)             |
| Real-time         | Azure SignalR Service               |
| Authentication    | JWT + Azure AD B2C (optional)       |
| Monitoring        | Application Insights                |
| Deployment        | Azure App Service / Container Apps  |
| Testing           | xUnit + k6 (load testing)           |

---

## 🏗️ Architecture

![Architecture Diagram](docs/architecture-diagram.png)  
*(Add your Excalidraw or draw.io diagram here)*

Main flow:  
Game Client → API → Services → Cosmos DB + SignalR → Application Insights

---

## 🚀 How to Run

### Locally
```bash
git clone https://github.com/yourusername/xbox-live-lite.git
cd xbox-live-lite
dotnet run --project src/Api
