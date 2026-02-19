# City Navigation System

A powerful city navigation system with ASP.NET Core backend and interactive Leaflet.js frontend. Find fastest, shortest, and balanced routes between locations with traffic simulation.

##  Features

- **Multiple Route Types**: Fastest, Shortest, and Balanced paths
- **Traffic Simulation**: Different patterns for morning, afternoon, evening, night
- **Interactive Map**: Built with Leaflet.js
- **Mobile Responsive**: Works perfectly on all devices
- **Dark Theme UI**: Sleek interface with light map
- **Public Sharing**: Easy ngrok integration

##  Tech Stack

- **Backend**: ASP.NET Core 6.0 Web API
- **Frontend**: HTML5, CSS3, JavaScript, Leaflet.js
- **Algorithms**: Dijkstra's Algorithm, Custom Graph Implementation
- **Routing**: 3 optimization strategies (time, distance, balanced)

## Screenshots
Desktop:
<img width="919" height="436" alt="image" src="https://github.com/user-attachments/assets/7a58e432-0b2a-4385-bfca-fcd280e9454c" />

Phone:
<img width="351" height="751" alt="image" src="https://github.com/user-attachments/assets/d760b6ba-d032-4329-9f11-80b4e5ba4cf3" />


##  Quick Start

### Prerequisites
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (optional)

### Run Locally
```bash
# Clone the repository
git clone https://github.com/NotHuz/City-Navigation-System.git

# Navigate to project
cd City-Navigation-System

# Build and run
dotnet run --project CityNavigation.API

# Open browser
https://localhost:7194


Ngrok usage is also supported at the port mentioned above
```
## Additional

-The area on which the project runs can be changed by swapping out the .osm file 
-Currently the nodes have been limited to 20 places for perfomance's sake which can also be altered as needed.
